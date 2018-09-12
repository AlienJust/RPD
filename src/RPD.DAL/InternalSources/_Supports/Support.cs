using System;
using System.IO;
using System.Reflection;

namespace RPD.Supports {
	internal static class Support {
		public const string NlogCfgPath = "NLog.config"; // TODO: move logs to app data logs

		public static string GetAppDataDirectoryPathAndCreateItIfNeeded() {
			var assembly = Assembly.GetEntryAssembly();
			var company = (AssemblyCompanyAttribute) Attribute.
				GetCustomAttribute(assembly, typeof (AssemblyCompanyAttribute));
			var product = (AssemblyProductAttribute) Attribute.
				GetCustomAttribute(assembly, typeof (AssemblyProductAttribute));

			string applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create);
			applicationDataPath = Path.Combine(applicationDataPath, company.Company);
			applicationDataPath = Path.Combine(applicationDataPath, product.Product);
			if (!Directory.Exists(applicationDataPath))
				Directory.CreateDirectory(applicationDataPath);
			return applicationDataPath;
		}
	}
}
