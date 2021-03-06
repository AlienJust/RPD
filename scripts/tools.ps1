function testPathOrExit($path)
{
    if ((test-path -path $path) -ne $True)
    {
        write-host "Ошибка: директория $path не существует" -foreground red
        exit
    }
}

#Получает версию из свойств файла
function getFileVersion
{
param($file)   
 
    return (get-childitem $file).VersionInfo.FileVersion
}

<#
Создаёт в заданном каталоге подкаталог с номером версии и копирует указанные файлы в этот подкаталог.
$src_dir - директория с файлами которые нужно скопировать.
$dest_dir - директория, в которой нужно создать подкаталог с версией.
$version - номер версии.
$version_postfix - постфикс, который будет добавлен к названию подкаталога с номер версии. Если в $dest_dir 
существует хотя бы одна директория с таким же постфиксом, то постфик будет удалён из имени этой директории. Может быть $null.
$overwrite - нужно ли перезаписывать имеющийся подкаталог.
$excludes - массив имен файлов, которые не нужно копировать, можно использовать символы подстановки.
$useOnlyIncludes - если true, то будут скопированы только файлы $includes, при этом $excludes не учитывается.
#>
function copyAll
{
param
(
$src_dir,
$dest_dir,
$version,
$version_postfix,
$overwrite,
$excludes,
$includes,
$useOnlyIncludes
)
    testPathOrExit($src_dir);
    testPathOrExit($dest_dir);    
    
    $target_dir = $dest_dir + "\" + $version + $version_postfix
    
    write-host "target_dir = $target_dir"
    
    #Проверяем существует ли такая директория
    if ((test-path $target_dir) -eq $True)   
    {
        if ($overwrite)
        {
            remove-item $target_dir -recurse
        }
        else
        {
            write-host "Директория $target_dir уже существует. Данные не перезаписаны."
            exit
        }
    } 
    
    #Переименовываем предыдущую версию
    if ($version_postfix -ne $null)
    {
        $dirs =@(get-childitem ($dest_dir + "\*" + $version_postfix))   
        write-host $dirs
        foreach ($dir in $dirs)
        {
            $newName = $dir.name -replace $version_postfix, ""
            $dir | rename-item -NewName $newname
        }
    }
    
    new-item $target_dir -type directory | out-null
    
    trap 
    {
        "Ошибка: $_"
        exit   
    }
    
    if ($useOnlyIncludes -ne $True)
    {
        copy-item -path ($src_dir + "\*") -destination $target_dir -exclude $excludes -recurse -force
    }
    else
    {
        copy-item -path ($src_dir + "\*") -destination $target_dir -include $includes -recurse -force
    }

    write-host "Файлы успешно скопированы в $target_dir"
    
    return $target_dir
}

#copyAll -src_dir "..\bin\RPD\Debug" -dest_dir "\\redmine\temp\Assemblies\RPD\Rpd.Presentation" -version "0.9.21" -version_postfix ".latest"
