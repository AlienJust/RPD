using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.InternalKitchen.Extensions;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal class PsnProtocolXmlSerializer {
		public const string RootNodeName = "PsnConfiguration";
		const string ParamNameAddress = "#ADDR";
		const string ParamNameCommand = "#NCMD";
		const string ParamNameCrcLow = "#CRCL";
		const string ParamNameCrcHigh = "#CRCH";


		private readonly string _xmlFileName;

		public PsnProtocolXmlSerializer(string xmlFileName) {
			_xmlFileName = xmlFileName;
		}

		public IPsnProtocolConfiguration LoadProtocol() {
			var xdoc = XDocument.Load(_xmlFileName);
			var rootNode = xdoc.Root;
			if (rootNode == null) throw new Exception("Не удалось найти начальный узел XML");
			if (rootNode.Name != RootNodeName) throw new Exception("Название начального узла XML должно быть " + RootNodeName);

			var configName = rootNode.Attribute("Name").Value;
			var configVersion = rootNode.Attribute("Version").Value;
			var configDescription = rootNode.Attribute("Description").Value;
			var configId = rootNode.Attribute("Id").Value;
			var configRpdId = rootNode.Attribute("RpdId").Value;
			IPsnProtocolConfigurationInformtaion configInfo = new PsnConfigurationInformationSimple(configName, configDescription, configVersion, new IdentifierStringToLowerBased(configId), configRpdId);

			var commandInfos = new List<IPsnProtocolCommandConfiguration>();
			var commandParts = new List<IPsnProtocolCommandPartConfiguration>();

			var commandsNode = rootNode.Element("Commands");
			if (commandsNode == null) throw new Exception("Не удалось найти тэг команд");
			var cmdMaskNodes = commandsNode.Elements("CmdMask");
			foreach (var cmdMaskNode in cmdMaskNodes) {
				var atrName = cmdMaskNode.Attribute("Name");
				if (atrName == null) continue;

				var requestNode = cmdMaskNode.Element("Request");
				if (requestNode == null) continue;

				var replyNode = cmdMaskNode.Element("Reply");
				if (replyNode == null) continue;

				bool isRequestRequired;
				var isRequestRequiredAttribute = replyNode.Attribute("IsRequestRequired");
				if (isRequestRequiredAttribute == null) {
					isRequestRequired = false;
				}
				else {
					try {
						isRequestRequired = bool.Parse(isRequestRequiredAttribute.Value);
					}
					catch {
						continue;
					}
				}

				var name = atrName.Value;

				var idNode = cmdMaskNode.Attribute("Id");
				IIdentifier id = idNode != null ? new IdentifierStringToLowerBased(idNode.Value) : new IdentifierStringToLowerBased(name + "@" + configId);

				commandInfos.Add(new PsnCommandInfo { Name = name, Id = id });

				commandParts.Add(LoadCommandPart(requestNode, name + ": запрос", PsnProtocolCommandPartType.Request, id));
				commandParts.Add(LoadCommandPart(replyNode, name + ": ответ", isRequestRequired ? PsnProtocolCommandPartType.ReplyWithRequiredRequest : PsnProtocolCommandPartType.Reply, id));
			}

			var meters = new List<IPsnProtocolMeterConfiguration>();
			var metersNode = rootNode.Element("PsnMeters");
			if (metersNode == null) throw new Exception("Не удалось найти тэг ПСН измерителей");
			var meterNodes = metersNode.Elements("PsnMeter");

			foreach (var meterNode in meterNodes) {
				var atrName = meterNode.Attribute("Name");
				if (atrName == null) continue;

				var atrAddr = meterNode.Attribute("Address");
				if (atrAddr == null) continue;

				var meter = new PsnProtocolMeterConfigurationSimple { Address = atrAddr.Value.ToInt(), Name = atrName.Value };
				meters.Add(meter);
			}

			var cyclePsnCmdPartInfos = commandParts.OfType<IPsnProtocolCommandPartConfigurationCycle>().ToList();

			var psnDeviceConfigurations = new List<IPsnProtocolDeviceConfiguration>();
			foreach (var psnMeterInfo in meters) {
				//var signalConfigurations = new List<IPsnProtocolDeviceSignalConfiguration>();
				//foreach (var psnCommandInfo in commandParts)
				//{
				//signalConfigurations.AddRange(PsnSignalConfigurationsMaker.MakePsnSignalConfigurationsForSomePsnMeterAddress(psnCommandInfo, psnCommandInfo.PartType == PsnProtocolCommandPartType.Request, psnMeterInfo.Address));
				//}

				psnDeviceConfigurations.Add(new PsnDeviceConfigurationSimple(psnMeterInfo.Name, psnMeterInfo.Address));
			}

			var psnVirtualDevicesNode = rootNode.Element("PsnMergedlDevices");
			if (psnVirtualDevicesNode == null) return new PsnProtocolConfigurationSimple(configInfo, psnDeviceConfigurations, cyclePsnCmdPartInfos, commandParts, commandInfos, null);

			var mergedDevices = psnVirtualDevicesNode.Elements("PsnMergedlDevice").Select(n1 =>
				new PsnMergedDeviceSimple(n1.Attribute("Name").Value, n1.Elements("PsnMergedParameter").Select(n2 =>
					new PsnMergedParameterSimple(
						new IdentifierStringToLowerBased(n2.Attribute("Id").Value),
						n2.Attribute("Name").Value,
						n2.Attribute("Expression").Value,
						bool.Parse(n2.Attribute("IsMsIntegrated").Value),
						n2.Elements("PsnMergedParameterPart").Select(n3 =>
						new PsnMergedParameterPartSimple(new IdentifierStringToLowerBased(n3.Attribute("RealId").Value), n3.Attribute("ExpressionName").Value))))));

			return new PsnProtocolConfigurationSimple(configInfo, psnDeviceConfigurations, cyclePsnCmdPartInfos, commandParts, commandInfos, mergedDevices);
		}

		private IPsnProtocolCommandPartConfiguration LoadCommandPart(XElement node, string partName, PsnProtocolCommandPartType partType, IIdentifier parentCommandId) {
			var defParams = LoadDefinedValues(node).ToArray();
			var varParams = LoadVariableValues(node).ToArray();



			var address = PsnCmdPartExt.GetDefParamInfo(defParams, ParamNameAddress);
			var commandCode = PsnCmdPartExt.GetDefParamInfo(defParams, ParamNameCommand);

			var crcHigh = PsnCmdPartExt.GetVarParamInfo(varParams, ParamNameCrcHigh);
			var crcLow = PsnCmdPartExt.GetVarParamInfo(varParams, ParamNameCrcLow);

			var atrLen = node.Attribute("Length");
			var len = atrLen != null ? byte.Parse(atrLen.Value) : (byte)0;

			var msNode = node.Attribute("CycleMsTime");

			var defParamsWithoutAddrAndCmd = defParams.Where(dp => dp.Name != ParamNameAddress && dp.Name != ParamNameCommand).ToArray();
			if (msNode != null)
				return new CyclePsnCommandPartInfoInfo {
					PartName = partName,
					PartType = partType,
					DefParams = defParamsWithoutAddrAndCmd,
					VarParams = varParams,
					Address = address,
					CommandCode = commandCode,
					CrcHigh = crcHigh,
					CrcLow = crcLow,
					Length = len,
					CyclePeriod = TimeSpan.FromMilliseconds(int.Parse(msNode.Value)),
					CommandId = parentCommandId,
					Id = new IdentifierStringToLowerBased(partName + "@" + parentCommandId)
				};
			return new PsnCommandPartInfo {
				PartName = partName,
				PartType = partType,
				DefParams = defParamsWithoutAddrAndCmd,
				VarParams = varParams,
				Address = address,
				CommandCode = commandCode,
				CrcHigh = crcHigh,
				CrcLow = crcLow,
				Length = len,
				CommandId = parentCommandId,
				Id = new IdentifierStringToLowerBased(partName + "@" + parentCommandId)
			};
		}

		private IEnumerable<IPsnProtocolParameterDefinedConfiguration> LoadDefinedValues(XElement node) {
			var result = new List<IPsnProtocolParameterDefinedConfiguration>();
			var defValNodes = node.Elements("DefVal");//.Select(e => new PsnParamDefinedByte{DefinedValue = e.Attribute("value").Value.ToInt(), Name = e.Attribute("name").Value});
			foreach (var element in defValNodes) {
				var xAttributeValue = element.Attribute("Value");
				if (xAttributeValue == null) continue;
				var xAttributeName = element.Attribute("Name");
				if (xAttributeName == null) continue;
				var xAttributePos = element.Attribute("Position");
				if (xAttributePos == null) continue;
				var xAttributeLength = element.Attribute("Length");
				if (xAttributeLength == null) continue;

				var xAttributeId = element.Attribute("Id");
				if (xAttributeId == null) continue;

				var value = xAttributeValue.Value.ToInt();
				var name = xAttributeName.Value;
				var pos = xAttributePos.Value.ToPos();
				var id = xAttributeId.Value;


				var len = int.Parse(xAttributeLength.Value);
				if (len == 8 && pos.Bit == 0)
					result.Add(
						new PsnParamDefinedByte(
							id,
							name,
							pos.Byte,
							value));
			}
			return result;
		}

		private IEnumerable<IPsnProtocolParameterConfigurationVariable> LoadVariableValues(XElement node) {
			var result = new List<IPsnProtocolParameterConfigurationVariable>();

			//var varValueNodes = node.Elements("VarVal");
			//foreach (var element in varValueNodes) {
			foreach (var element in node.Elements()) {
				if (element.Name == "VarVal") {
					var xAttributeName = element.Attribute("Name");
					if (xAttributeName == null) continue;
					var xAttributePos = element.Attribute("Position");
					if (xAttributePos == null) continue;
					var xAttributeLength = element.Attribute("Length");
					if (xAttributeLength == null) continue;

					var atrMultiplier = element.Attribute("Multiplier");
					var multiplier = atrMultiplier != null ? double.Parse(atrMultiplier.Value, CultureInfo.InvariantCulture) : 1.0;

					var atrSigned = element.Attribute("IsSigned");
					var signed = atrSigned != null && bool.Parse(atrSigned.Value);

					var xAttributeId = element.Attribute("Id");
					if (xAttributeId == null) continue;

					var name = xAttributeName.Value;
					var pos = xAttributePos.Value.ToPos();
					var len = int.Parse(xAttributeLength.Value);
					var id = xAttributeId.Value;

					if (len == 1) {
						result.Add(new PsnParamVariableBit(
							id,
							name,
							pos.Byte,
							pos.Bit));
						//var xAttributeFdef = element.Attribute("FaultDefinition");
						//if (xAttributeFdef == null) result.Add(new PsnParamVariableBit { Name = parentName + name, PositionByte = pos.Byte, PositionBit = pos.Bit });
						//else {
						//var fdef = xAttributeFdef.Value.ToPos();
						//result.Add(new VariableBitPsnParamFaultDefine { Name = parentName + name, PositionByte = pos.Byte, PositionBit = pos.Bit, RpdConfPosByte = fdef.Byte, RpdConfPosBit = fdef.Bit });
						//}
					}
					else if (len == 8 && pos.Bit == 0) {
						if (!signed)
							result.Add(new PsnParamVariableByte(
								id,
								name,
								pos.Byte, multiplier));
						else
							result.Add(new PsnParamVariableSByte(
								id,
								name,
								pos.Byte, multiplier));
					}
					else {
						if (!signed)
							result.Add(new PsnParamVariableMultibit(
								id,
								name,
								pos.Byte, pos.Bit, len, multiplier));
						else
							result.Add(new PsnParamVariableSMultibit(
								id,
								name,
								pos.Byte, pos.Bit, len, multiplier));
					}
				}
				else if (element.Name == "CpzPrm")
				{
					var xAttributeName = element.Attribute("Name");
					if (xAttributeName == null) continue;
					var xAttributeExpression = element.Attribute("Expression");
					if (xAttributeExpression == null) continue;
					var xAttributeId = element.Attribute("Id");
					if (xAttributeId == null) continue;

					result.Add(new PsnParamVariableComposed(
						xAttributeId.Value,
						xAttributeName.Value,
						xAttributeExpression.Value));
				}
				else if (element.Name == "BitPrm")
				{
					var xAttributeId = element.Attribute("Id");
					if (xAttributeId == null) continue;
					var id = xAttributeId.Value;

					var xAttributeName = element.Attribute("Name");
					if (xAttributeName == null) continue;
					var name = xAttributeName.Value;

					var xAttributeByte = element.Attribute("Byte");
					if (xAttributeByte == null) continue;
					int bbyte;
					try {
						bbyte = int.Parse(xAttributeByte.Value);
					}
					catch {
						continue;
					}


					var xAttributeBit = element.Attribute("Bit");
					if (xAttributeBit == null) continue;
					int bit;
					try {
						bit = int.Parse(xAttributeBit.Value);
					}
					catch {
						continue;
					}

					var xAttributeValueInverted = element.Attribute("IsValueInverted");
					if (xAttributeValueInverted == null) continue;
					bool isValueInverted;
					try {
						isValueInverted = bool.Parse(xAttributeValueInverted.Value);
					}
					catch {
						continue;
					}
					if (!isValueInverted) {
						result.Add(new PsnParamVariableBit(
							id,
							name,
							bbyte,
							bit));
					}
					else {
						result.Add(new PsnParamVariableBitInverted(
							id,
							name,
							bbyte,
							bit));
					}
					//var xAttributeFdef = element.Attribute("FaultDefinition");
					//if (xAttributeFdef == null) result.Add(new PsnParamVariableBit { Name = namePreffix + name, PositionByte = pos.Byte, PositionBit = pos.Bit });
					//else
					//{
					//var fdef = xAttributeFdef.Value.ToPos();
					//result.Add(new VariableBitPsnParamFaultDefine { Name = namePreffix + name, PositionByte = pos.Byte, PositionBit = pos.Bit, RpdConfPosByte = fdef.Byte, RpdConfPosBit = fdef.Bit });
					//}
				}
			}
				
			return result;
		}
	}
}