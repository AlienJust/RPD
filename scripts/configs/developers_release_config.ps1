#Директория, которая содержит откомпилированные файлы (указывать путь относительно текущего местоположения срипта).
$bins_dir = "..\bin\RPD\Debug";

#Имя файла в $bins_dir из которого будет браться версия релиза (версия берётся из св-в файла).
$version_file = "RPD.exe"

#Директория, в которую нужно скопировать
$target_dir = "\\10.0.0.201\temp\Assemblies\RPD\Rpd.Presentation"

#Файлы, которые не нужно копировать, перечислять через запятую, можно использовать символы подстановки.
$excludes = 
"defaults",
"rpd.dal*", 
"*.vshost.*",
"version.txt",
"history.txt",
"AlienJust.*",
"System.Data.SQLite.dll",
"NCalc.dll",
"nunit.framework.*",
"RPD.Presentation.Tests.*",
"Moq.*"


#Если true, то будут скопированы только файлы $includes из $bins_dir, при этом $excludes не учитывается.
$useOnlyIncludes = $False

$includes = "defaults",
"RPD.DAL.xml",
"RPD.DAL.dll",
"RPD.DAL.pdb",
"RPD.DAL.dll"

