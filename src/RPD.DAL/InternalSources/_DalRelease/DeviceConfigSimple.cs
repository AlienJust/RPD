using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using RPD.DAL;
using RPD.DalRelease.PSN.Config;
using RPD.EventArgs;

namespace RPD {
	class DeviceConfigSimple: IDeviceConfiguration {
		public IRpdMeter CreateMeter() {
			throw new NotImplementedException();
		}

		public ObservableCollection<IRpdMeter> RpdMeters { get; set; }

		public ObservableCollection<IPsnMeter> PsnMeters { get; set; }

		public string LocomotiveName { get; set; }

		public int SectionNumber { get; set; }

		public string Comment { get; set; }

		public int Address { get; set; }

		public int NetAddress { get; set; }

		public bool UseHammingForNand { get; set; }

		public bool LogPsn { get; set; }

		public bool SaveByteInterval { get; set; }

		public bool ResetFaultsDump { get; set; }

		public IDeviceDiagnostic Diagnostic { get; set; }

		public PsnProtocolVersion PsnVersion { get; set; }

		public void Read(string path, Action<OnCompleteEventArgs> onComplete) {
			throw new NotImplementedException();
		}

		public void Write(string path, Action<OnCompleteEventArgs> onComplete) {
			throw new NotImplementedException();
		}

		/*public bool LoadAttributesFromXml(string filename) {
			throw new NotImplementedException();
		}

		public XmlDocument SaveAttributesToXml() {
			throw new NotImplementedException();
		}

		public void LoadRpdConfigFromXml(string filename) {
			throw new NotImplementedException();
		}

		public XmlDocument SaveRpdConfigToXml() {
			throw new NotImplementedException();
		}

		public void LoadPsnConfigFromXml(string filename) {
			throw new NotImplementedException();
		}

		public XmlDocument SavePsnConfigToXml() {
			throw new NotImplementedException();
		}*/

		public void ClearArchives(string driveLetter) {
			throw new NotImplementedException();
		}

		public void ClearArchivesAsync(string driveLetter, Action<OnCompleteEventArgs> onComplete) {
			throw new NotImplementedException();
		}

		public void TestLinkWithMetersAsync(string driveLetter, Action<OnCompleteEventArgs> onComplete) {
			throw new NotImplementedException();
		}

		public void WriteFirmware(string firmwireHexFilename, string deviceDriveLetter, Action<OnCompleteEventArgs> onComplete) {
			throw new NotImplementedException();
		}

		public void SendTime(DateTime time, string deviceDriveLetter, Action<OnCompleteEventArgs> onComplete) {
			throw new NotImplementedException();
		}

		public IList<ICyclePsnCmdPartInfo> CycledCommandPartInfos {
			get { throw new NotImplementedException(); }
		}
	}
}