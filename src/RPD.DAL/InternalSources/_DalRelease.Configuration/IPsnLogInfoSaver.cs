using RPD.DAL;

namespace RPD.DalRelease.Configuration.System.Contracts {
	interface IPsnLogFragmentInfoSaver
	{
		void Save(IPsnLogFragmentInfo psnLogFragmentInfo);
	}
}