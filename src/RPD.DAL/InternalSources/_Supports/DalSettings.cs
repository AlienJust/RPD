using System;
using System.IO;
using System.Xml.Linq;

namespace RPD.Supports
{
	internal class DalSettings {
		public static DateTime StartupTime = DateTime.Now;

		#region Singletone

		private static readonly Lazy<DalSettings> Inst = new Lazy<DalSettings>(() => new DalSettings());

		public static DalSettings Instance {
			get { return Inst.Value; }
		}

		#endregion

		public PsnParser PsnParserType { get; private set; }
		public bool UsePsnCache { get; private set; }
		public TimeSpan PsnLogSplitTime { get; private set; }

		private DalSettings() {
			PsnParserType = PsnParser.Full;
			UsePsnCache = false;

			LoadFromFile(Path.Combine(Usefull.AssemblyFolderName, "defaults", "DalSettings.xml"));
		}

		private void LoadFromFile(string fileName) {
			try {
				//var xml = new XmlDocument();
				//xml.Load(fileName);
				var xml = XDocument.Load(fileName);

				// Выбрать первый узел, имя которого Settings
				var node = xml.Element("Settings");
				if (node != null) {
					var psnNode = node.Element("Psn");
					PsnParserType = (PsnParser) Enum.Parse(typeof (PsnParser), psnNode.Attribute("ParserType").Value);
					UsePsnCache = bool.Parse(psnNode.Attribute("UseCache").Value);
					PsnLogSplitTime = TimeSpan.FromMilliseconds(int.Parse(psnNode.Attribute("PageSplitTimeMs").Value));
				}
			}
			catch {
				// TODO: Avoid empty catch block
			}
		}
	}

	/// <summary>
	/// Тип парсера магистрали ПСН
	/// </summary>
	internal enum PsnParser
	{
		/// <summary>
		/// Полный парсинг
		/// </summary>
		Full = 0,

		/// <summary>
		/// Быстрый парсинг (только первое значение из страницы)
		/// </summary>
		Fast = 1,

		/// <summary>
		/// Усреднение всех значений страницы ПСН в одну точку (для снижения нагрузки на график)
		/// </summary>
		Average = 2
	};
}
