using System.IO;
using System.Reflection;

namespace RPD.Supports
{
	/// <summary>
	/// ����� �����������
	/// </summary>
	public class Usefull
	{
		/// <summary>
		/// �������� ���� � ����� �� �������
		/// </summary>
		public static string AssemblyFolderName
		{
			get { return Path.GetDirectoryName(Assembly.GetAssembly(typeof (Usefull)).Location); }
		}

		/// <summary>
		/// ������� ������ ������ \\ � ����� ������ - ����������� ���� � ����������
		/// </summary>
		/// <param name="path">������-���� � ����������</param>
		/// <returns>��������������� ����</returns>
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
		/// �������� �������� ����������� ����� ��������� �������� WMI, ���� �������� �� ������� - ���������� ������ ������
		/// </summary>
		/// <param name="driveLetter">����� ����������� ����</param>
		/// <returns>�������� ����������� ����� ���� \\.\PHYSICALDRIVEX</returns>
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
		/// �������� ���� � ����� � ����������
		/// </summary>
		public static string StartupDirPath
		{
			get { return Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName); }
		}
	}
}
