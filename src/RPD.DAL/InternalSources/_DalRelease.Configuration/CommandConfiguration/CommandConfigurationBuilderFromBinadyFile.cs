using System;
using System.Collections.Generic;
using System.IO;
using AlienJust.Support.IO;

namespace RPD.DalRelease.Configuration.CommandConfiguration {
	class CommandConfigurationBuilderFromBinadyFile : ICommandConfigurationBuilder {
		private readonly string _filename;

		public CommandConfigurationBuilderFromBinadyFile(string filename) {
			_filename = filename;
		}

		public ICommandConfiguration BuildConfiguration() {
			// PERFOMANCE INCREASE POSSIBILITY: need fastest work with stream, closing it, then filling fields and vars
			using (var br = new AdvancedBinaryReader(File.OpenRead(_filename), false)) { 
				br.BaseStream.Seek(2048, SeekOrigin.Begin);
				var memoryStatusByte = br.ReadByte();
				var configStatusByte = br.ReadByte();

				var verificationStatusBytes = br.ReadBytes(32);

				var meterChannelConfigVerificationStatuses = new List<IMetersChannelsConfigurationVerificationStatus>();
				for(int i = 0; i < verificationStatusBytes.Length; ++i) {
					var status = verificationStatusBytes[i];
					ChannelConfigVerificationStatus st;
					switch (status) {
						case 0:
							st = ChannelConfigVerificationStatus.NoMeterFoundInTable;
							break;
						case 1:
							st = ChannelConfigVerificationStatus.VerificationSuccess;
							break;
						case 2:
							st = ChannelConfigVerificationStatus.ErrorDuringConfigurationWriting;
							break;
						case 3:
							st = ChannelConfigVerificationStatus.NoLinkWithMeter;
							break;
						case 4:
							st = ChannelConfigVerificationStatus.VerificationInProgress;
							break;
						default:
							st = ChannelConfigVerificationStatus.Unknown;
							break;
					}
					meterChannelConfigVerificationStatuses.Add(new MetersChannelsConfigurationVerificationStatusSimple(i, st));
				}
				br.ReadByte();
				var second = br.ReadByte();
				var minute = br.ReadByte();
				var hour = br.ReadByte();
				var day = br.ReadByte();
				var month = br.ReadByte();
				var year = br.ReadByte();

				DateTime? dateTime;
				try {
					dateTime = new DateTime(2000 + year, month, day, hour, minute, second);
				}
				catch {
					dateTime = null;
				}

				var psnProtocolType = br.ReadUInt16();

				return new CommandConfigurationSimple(
					dateTime,
					(memoryStatusByte & 0x01) == 0x01,
					(memoryStatusByte & 0x02) == 0x02,
					(memoryStatusByte & 0x04) == 0x04,
					(memoryStatusByte & 0x08) == 0x08,

					(configStatusByte & 0x01) == 0x01,
					(configStatusByte & 0x02) == 0x02,
					(configStatusByte & 0x04) == 0x04,
					(configStatusByte & 0x08) == 0x08,
					(configStatusByte & 0x10) == 0x10,
					(configStatusByte & 0x20) == 0x20,
					(configStatusByte & 0x40) == 0x40,

					meterChannelConfigVerificationStatuses,
					psnProtocolType
					);
			}
		}
	}
}