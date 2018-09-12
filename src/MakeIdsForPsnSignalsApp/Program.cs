using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DataAbstractionLevel.Low.PsnConfig;

namespace MakeIdsForPsnSignalsApp
{
	class Program {
		private const string TypeStartArgs = "-type:";
		private const string FileStartArg = "-file:";

		private static ConsoleColor _normalForeColor;
		private static ConsoleColor _normalBackColor;
		private static void Main(string[] args) {
			try {
				_normalForeColor = Console.ForegroundColor;
				_normalBackColor = Console.BackgroundColor;

				var type = args.First(a => a.StartsWith(TypeStartArgs)).Substring(TypeStartArgs.Length);
				var file = args.First(a => a.StartsWith(FileStartArg)).Substring(FileStartArg.Length);

				var psnConfigs = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "defaults")).GetFiles(file);

				if (type == "addonly") {
					//var psnConfigs = (new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "defaults"))).GetFiles(file);
					foreach (var psnConfig in psnConfigs) {
						Console.WriteLine("Working with document: " + psnConfig.FullName);
						ApplyIds(psnConfig.FullName, false);
					}
				}
				else if (type == "replace") {
					//var psnConfigs = (new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "defaults"))).GetFiles(file);
					foreach (var psnConfig in psnConfigs) {
						Console.WriteLine("Working with document: " + psnConfig.FullName);
						ApplyIds(psnConfig.FullName, true);
					}
				}
				else if (type == "getinfo") {
					var rpdIds = new List<Tuple<int, string, Guid>>();
					foreach (var psnConfig in psnConfigs)
					{
						try {
							var loader = new PsnProtocolConfigurationLoaderFromXml(psnConfig.FullName);
							var config = loader.LoadConfiguration();
							rpdIds.Add(new Tuple<int, string, Guid>(int.Parse(config.Information.RpdId), psnConfig.FullName, Guid.Parse(config.Id.IdentyString)));
							Console.WriteLine("RPD ID = " + config.Information.RpdId + " for config file: " + psnConfig.FullName);
						}
						catch (Exception ex) {
							Console.WriteLine(ex);
						}
					}
					Console.WriteLine();
					Console.Write("Ordering configurations by RPD ID... ");
					var orderedIds = rpdIds.OrderBy(i => i).ToList();
					Console.WriteLine("done, listing:----------------------------------------------------");
					Console.WriteLine();

					foreach (var orderedId in orderedIds) {
						Console.WriteLine("RPD ID = " + orderedId.Item1 + " for config file: " + orderedId.Item2);
					}

					Console.WriteLine();
					Console.WriteLine("Grouping to display items with same RpdId attribute: ==================================================");
					Console.WriteLine();
					
					var groupsByRpdId = orderedIds.GroupBy(item => item.Item1).OrderBy(g => g.Key);
					foreach (var g in groupsByRpdId) {
						var itemsCountInGroup = g.Count();
						if (itemsCountInGroup > 1) {
							SetConsoleErrorColors();
							Console.WriteLine(" RpdId = " + g.Key + " is taken by " + itemsCountInGroup + " configs : ----------------------------");
							SetConsoleNormalColors();

							var orderedGroup = g.OrderBy(t => t.Item2); // odering group items by psn config name
							foreach (var item in orderedGroup) {
								Console.WriteLine("RPD ID = " + item.Item1 + " for config file: " + item.Item2);
							}
							Console.WriteLine();
						}
					}

					Console.WriteLine();
					Console.WriteLine("Grouping to display items with same Id attribute: =====================================================");
					Console.WriteLine();

					var groupsById = orderedIds.GroupBy(item => item.Item3).OrderBy(g => g.Key);
					foreach (var g in groupsById) {
						var itemsCountInGroup = g.Count();
						if (itemsCountInGroup > 1) {
							SetConsoleErrorColors();
							Console.WriteLine(" Id = " + g.Key + " is taken by " + itemsCountInGroup + " configs : -------------------------------");
							SetConsoleNormalColors();

							var orderedGroup = g.OrderBy(t => t.Item2); // odering group items by psn config name
							foreach (var item in orderedGroup) {
								Console.WriteLine("ID = " + item.Item3 + " for config file: " + item.Item2);
							}
							Console.WriteLine();
						}
					}


					Console.WriteLine();
					Console.WriteLine("Summary:===============================================================================================");
					Console.WriteLine();
					int maxRpdId = rpdIds.Max(tuple=>tuple.Item1);
					SetConsoleAccentedColors();
					Console.WriteLine("Max RPD ID is: " + maxRpdId);
					SetConsoleNormalColors();

					for (int i = 1; i < maxRpdId + 5; ++i) {
						if (rpdIds.All(tuple => tuple.Item1 != i)) {
							Console.WriteLine("Free RPD ID is: " + i);
						}
					}
				}
			}
			catch (Exception ex) {
				Console.WriteLine("Arguments: -type:<worktype> -file:<filemask>");

				Console.WriteLine("<worktype> 'addonly' to add missed IDs");
				Console.WriteLine("<worktype> 'replace' to rewrite all IDs");

				Console.WriteLine("<filemask> is a masked filename, e.g. 'psn.*.xml'");
			}
		}

		static void ApplyIds(string filename, bool isReapplyNeeded) {
			var doc = XDocument.Load(filename);
			const string rootNodeName = "PsnConfiguration";
			var rootNode = doc.Element(rootNodeName);
			if (rootNode == null) {
				throw new Exception("Cannot get node <" + rootNodeName + "/>");
			}
			const string cmdsNodeName = "Commands";
			var cmdsNode = rootNode.Element(cmdsNodeName);
			if (cmdsNode == null) {
				throw new Exception("Cannot get node <" + cmdsNodeName + "/>");
			}

			const string cmdNodeName = "CmdMask";
			var cmdNodes = cmdsNode.Elements(cmdNodeName);
			foreach (var cmdNode in cmdNodes) {
				var cmdPartNodes = new List<XElement>();
				const string requestNodeName = "Request";
				const string replyNodeName = "Reply";
				var requestNode = cmdNode.Element(requestNodeName);
				if (requestNode != null) {
					cmdPartNodes.Add(requestNode);
				}

				var replyNode = cmdNode.Element(replyNodeName);
				if (replyNode != null) {
					cmdPartNodes.Add(replyNode);
				}


				foreach (var cmdPartNode in cmdPartNodes) {
					var paramNodes = new List<XElement>();
					paramNodes.AddRange(cmdPartNode.Elements("VarVal"));
					paramNodes.AddRange(cmdPartNode.Elements("DefVal"));
					paramNodes.AddRange(cmdPartNode.Elements("BitPrm"));
					paramNodes.AddRange(cmdPartNode.Elements("CpzPrm"));

					foreach (var paramNode in paramNodes) {
						var idAttribute = paramNode.Attribute("Id");
						if (idAttribute == null) {
							paramNode.Add(new XAttribute("Id", Guid.NewGuid()));
							Console.WriteLine("Attribute <Id> was added to node " + paramNode.Name);
						}
						else if (isReapplyNeeded) {
							Console.WriteLine("Attribute <Id> value changed " + paramNode.Name);
							idAttribute.Value = Guid.NewGuid().ToString();
						}
					}
				}
			}
			doc.Save(filename);
		}

		static void SetConsoleErrorColors() {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.BackgroundColor = ConsoleColor.Black;
		}

		static void SetConsoleAccentedColors() {
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.BackgroundColor = ConsoleColor.Black;
		}
		static void SetConsoleNormalColors() {
			Console.ForegroundColor = _normalForeColor;
			Console.BackgroundColor = _normalBackColor;
		}
	}
}
