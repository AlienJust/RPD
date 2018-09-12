using System.IO;
using System.Reflection;

namespace RPD.Supports
{
	/// <summary>
	/// Класс полезностей
	/// </summary>
	public class Usefull
	{
		/// <summary>
		/// Получает путь к папке со сборкой
		/// </summary>
		public static string AssemblyFolderName
		{
			get { return Path.GetDirectoryName(Assembly.GetAssembly(typeof (Usefull)).Location); }
		}

		/// <summary>
		/// Убирает лишний символ \\ в конце строки - нормализует путь к директории
		/// </summary>
		/// <param name="path">Строка-путь к директории</param>
		/// <returns>Нормализованный путь</returns>
		public static string CheckRemoveBackslashFromPath(string path)
		{
			string result = path;
			if (path.EndsWith("\\"))
			{
				result = path.Substring(0, path.Length - 1);
			}
			return result;
		}

		/// <summary>
		/// Получает название физического диска используя механизм WMI, если название не найдено - возвращает пустую строку
		/// </summary>
		/// <param name="driveLetter">Буква логического тома</param>
		/// <returns>Название физического диска вида \\.\PHYSICALDRIVEX</returns>
		public static string GetPhysicalDriveName(string driveLetter)
		{
			//MessageBox.Show(RPD.DAL.WinAPI.GetPhysicalDeviceName("C"));
			//System.IO.DriveInfo x = new System.IO.DriveInfo("C");
			//System.IO.
			//MessageBox.Show(x.Name);
			if (driveLetter.Length == 1)
			{
				using (System.Management.ManagementObject mo = new System.Management.ManagementObject(@"Win32_LogicalDisk='" + driveLetter + ":'"))
				{
					foreach (System.Management.ManagementObject b in mo.GetRelated("Win32_DiskPartition"))
					{
						foreach (System.Management.ManagementBaseObject c in b.GetRelated("Win32_Diskdrive"))
							//Console.WriteLine("{0}", c["Name"]);
							return c["Name"].ToString();
					}
				}
			}
			return "";
		}

		/// <summary>
		/// Получает путь к папке с программой
		/// </summary>
		public static string StartupDirPath
		{
			get { return Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName); }
		}
	}
}
