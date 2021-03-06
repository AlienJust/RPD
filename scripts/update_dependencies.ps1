#=======Конфигурация скрипта======
#Путь к каталогу, который содержит подкаталоги с зависимостями
$dependencies_dir = "\\10.0.0.201\temp\Assemblies\RPD\Rpd.Dal"
#$dependencies_dir = "d:\Yandex.Disk\work\RPD\DAL"

$target_dirs = @("..\bin\RPD\Debug",
"..\bin\RPD\Release",
"..\src\packages\RPD.DAL")

#директории, которые нужно удалить перед тем как будут скопированы target_dirs
$target_remove_dirs = @("..\bin\RPD\Debug\defaults",
"..\bin\RPD\Release\defaults",
"..\src\packages\RPD.DAL\defaults")

#=======Тело скрипта======
$latestPostfix = ".latest"

#путь к файлу этого скрипта
$script_path = split-path -parent $MyInvocation.MyCommand.Path

. ($script_path +"\tools.ps1")

testPathOrExit($dependencies_dir)

#нормализуем target_dirs
for ($i = 0; $i -lt $target_dirs.length; $i++)
{
    $target_dirs[$i] = $script_path + "\" + $target_dirs[$i];
}

#нормализуем target_remove_dirs
for ($i = 0; $i -lt $target_remove_dirs.length; $i++)
{
    $target_remove_dirs[$i] = $script_path + "\" + $target_remove_dirs[$i];
}

foreach ($path in $target_dirs) 
{
    testPathOrExit($path)
}

$latest_dir = get-childitem ($dependencies_dir + "\*" + $latestPostfix)

if ($latest_dir -eq $null)
{
    write-host "Директория $dependencies_dir не содержит поддиректорий оканчивающихся на $latestPostfix" -foreground red
    exit
}

$latest_dir = $latest_dir.FullName

trap 
{
    "Ошибка: $_"
    exit   
}

foreach ($path in $target_remove_dirs)
{
    remove-item -path $path -recurse -force -ErrorAction SilentlyContinue
}

foreach ($path in $target_dirs)
{
    copy-item -path ($latest_dir + "\*") -destination $path -recurse -force
}

write-host "Файлы успешно скопированы из $latest_dir в $target_dirs"





