write-host "Installer"

$script_path = split-path -parent $MyInvocation.MyCommand.Path

. ($script_path +"\tools.ps1")
. ($script_path +"\configs\create_installer_config.ps1")

$dest_dir = $script_path + "\" + $bins_dir

#получаем версию фалйа RPD и удаляем лишний .0 в конце
$version = (getFileVersion -file ($dest_dir + "\" + $version_file))#.TrimEnd(".0")

$dal_version = getFileVersion -file ($dest_dir + "\" + "RPD.DAL.dll")
$dal_version = ($dal_version -split "\.")[-1]

$full_version = $version + "." + $dal_version 
write-host "Версия RPD = $version, версия RPD.DAL = $dal_version. Полная версия $full_version."


$result_dir = copyAll -src_dir $dest_dir -dest_dir $target_dir -version $full_version -version_postfix $null -overwrite $True -excludes $excludes - includes $includes -useOnlyIncludes $useOnlyIncludes

#Удаляем мусорные файлы.

remove-item -path ($result_dir + "\*.xml") -exclude @("DefaultConnectionPoints.xml", "defaultlayout.xml")

remove-item -path ($result_dir + "\*.pdb") -exclude @("RPD.SciChartControl.pdb",  
"RPD.DAL.pdb",
"RPD.Configurator.pdb",
"RPD.pdb")

#Копируем файл с историей
copy-item -path ($script_path + "\..\src\history.txt") -destination $result_dir 

#Запускаем сборщик инсталлятора
#
$env:RPDVER = $full_version
&"$script_path\nsis.cmd" $script_path\..\src\installer\installer.nsi
#