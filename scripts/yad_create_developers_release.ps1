param(
$overwrite=$False #Нужно ли перезаписывать, если такая версия уже существует.
)

<#
Visual Studio post-build event command line: call powershell set-executionpolicy remotesigned ; $(SolutionDir)..\scripts\create_developers_release.ps1
#>

write-host "Deployment"

$script_path = split-path -parent $MyInvocation.MyCommand.Path

. ($script_path +"\tools.ps1")
. ($script_path +"\configs\yad_developers_release_config.ps1")

$dest_dir = $script_path + "\" + $bins_dir

$version = getFileVersion -file ($dest_dir + "\" + $version_file)
write-host "Версия $version"

copyAll -src_dir $dest_dir -dest_dir $target_dir -version $version -version_postfix ".latest" -overwrite $overwrite -excludes $excludes -includes $includes -useOnlyIncludes $useOnlyIncludes