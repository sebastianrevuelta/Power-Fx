param(
    [Parameter (Mandatory = $false)] 
    [ValidateSet ('all', 'net31', 'net6', 'net7')] 
    [String[]]$IncludeVersions = 'net31',
    [String]$pfxFolder,
    [String]$NugetVersion,
    [String]$Config = 'Release',
    ## These parameters are only used by the build system
    [Boolean]$UseDrop = $false,    
    [string]$n31 = 'false',
    [string]$n60 = 'false',
    [string]$n70 = 'false'
)

## To generate these packages locally, build the 3 solutions Microsoft.PowerFx.sln, Microsoft.PowerFx.Net6.sln and Microsoft.PowerFx.Net7.sln in Release mode
## set env variable $env:BUILD_SOURCESDIRECTORY = "C:\Data\Power-Fx" ## location of source directory
## run this Powershell script from the command line `.\GeneratePackages.ps1 -NugetVersion "0.2.4-preview.20230101-2000" -IncludeVersions 'net31' -Config 'Release'` 

## Used to format XML file
function Format-XML ([xml]$xml, $indent=2)
{
    $StringWriter = New-Object System.IO.StringWriter
    $XmlWriter = New-Object System.XMl.XmlTextWriter $StringWriter
    $xmlWriter.Formatting = “indented”
    $xmlWriter.Indentation = $Indent    
    $xml.WriteContentTo($XmlWriter)
    $XmlWriter.Flush()
    $StringWriter.Flush()
    $StringWriter.ToString()
}

if ($n31 -or $n60 -or $n70)
{
    $v = [System.Collections.ArrayList]::new()
    if ([bool]$n31) { [void]$v.Add('net31') }
    if ([bool]$n60) { [void]$v.Add('net6') }
    if ([bool]$n70) { [void]$v.Add('net7') }
    if ($v.ToArray().Length -eq 3) { $IncludeVersions = 'all' } else { $IncludeVersions = [string]::Join(",", $v.ToArray()) }
}

$schema = "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd"
$IncludeVersions = [string[]]($IncludeVersions | Select-Object -Unique)
$all = $false

## If an empty version list is specified, generate .Net Core 3.1 only
if ($IncludeVersions.Length -eq 0)
{
    $IncludeVersions = [string[]]@('net31')
}

## If 'all' is specified, generate '.Net Core 3.1', '.Net 6.0' and '.Net 7.0'
if ($IncludeVersions -contains 'all')
{
    $all = $true
    $IncludeVersions = [string[]]@('net31', 'net6', 'net7')
}

## Will generate 'Net31-Net7' when IncludeVersions is 'net31,net7'
$idSuffix = [System.String]::Join('-', [System.Linq.Enumerable]::Select($IncludeVersions, [func[string,string]]{$args[0].Substring(0,1).ToUpper()+$args[0].Substring(1) }));

## Use current folder by default
if ([System.String]::IsNullOrEmpty($pfxFolder))
{
    $pfxFolder = "."
}

if ($UseDrop)
{   
    if ($pfxFolder -eq ".") { $y = "" } else { $y = "..\" }
    $dropFolder = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "$pfxFolder\src\nuspecs\$y..\..\..\drop")) 
    Write-Host "Using drop folder " $dropFolder
    $m = Select-String ".*Net[0-9]+\.(.*)" -InputObject ([System.IO.Directory]::EnumerateDirectories($dropFolder) | ? { $_ -match 'Microsoft.PowerFx' })[0]
    $NugetVersion = $m.Matches.Groups[1].Value
}

Write-Host "### Generate packages for $IncludeVersions"
Write-Host "NugetVersion = $NugetVersion"
Write-Host "IdSuffix = $idSuffix"

$nuspecRoot = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "$pfxFolder\src\nuspecs\")
$nuspecRoot = [System.IO.Path]::GetFullPath($nuspecRoot)
cd $nuspecRoot
$pfxHash = (git log -n 1 --pretty=%H).ToString() ## Get last git commit hash

foreach ($nuspecFile in (Get-Item ($nuspecRoot + "*.nuspec") | % { $_.FullName }))
{ 
    $fileNameNoExt = [System.IO.Path]::GetFileNameWithoutExtension($nuspecFile)
    Write-Host
    Write-Host "Processing $fileNameNoExt..."
    $pkgRoot = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "$pfxFolder\src\nuspecs\$fileNameNoExt")
    $pkgRoot = [System.IO.Path]::GetFullPath($pkgRoot)
        
    if (Test-Path -Path $pkgRoot)
    {
        Remove-Item -Path $pkgRoot -Recurse
    }    

    [void](New-Item $pkgRoot -ItemType Directory)
    $newNuspecFile = [System.IO.Path]::Combine($pkgRoot, "$fileNameNoExt.nuspec")
    Copy-Item $nuspecFile -Destination $newNuspecFile    
    [xml]$nuspec = Get-Content $newNuspecFile 

    $nuspec.package.metadata.version = $NugetVersion
    
    if (-not $all)
    {        
        $nuspec.package.metadata.id = $nuspec.package.metadata.id + "." + $idSuffix;

        $newName = [System.IO.Path]::Combine($pkgRoot, ($nuspec.package.metadata.id + ".nuspec"))
        $newName2 = [System.IO.Path]::GetFileName($newName)
        Rename-Item $newNuspecFile $newName2

        $newNuspecFile = $newName
    }

    $pkgId = $nuspec.package.metadata.id
    Write-Host "Package Id" $pkgId
    Write-Host "Nuspec file" $newNuspecFile

    if ($nuspec.package.metadata.repository -ne $null)
    {
        $nuspec.package.metadata.repository.commit = $pfxHash
    }

    ## Remove the parts we don't need
    if (!($IncludeVersions -contains 'net7'))
    {
        $toRemove = $nuspec.package.metadata.dependencies.ChildNodes[2]
        [void]$toRemove.ParentNode.RemoveChild($toRemove)
    }

    if (!($IncludeVersions -contains 'net6'))
    {
        $toRemove = $nuspec.package.metadata.dependencies.ChildNodes[1]
        [void]$toRemove.ParentNode.RemoveChild($toRemove)
    }

    if (!($IncludeVersions -contains 'net31'))
    {
        $toRemove = $nuspec.package.metadata.dependencies.ChildNodes[0]
        [void]$toRemove.ParentNode.RemoveChild($toRemove)
    }

    ## Update package dependency versions (replace set of stars with current version)
    foreach ($group in $nuspec.package.metadata.dependencies.group)
    {
        foreach ($dep in $group.dependency)
        {
            if ($dep.version -eq "****")
            {
                $dep.version = $NugetVersion
                
                if (-not $all)
                {
                    $dep.id = $dep.id + "." + $idSuffix;
                }
            }
        }
    }

    ## Microsoft.PowerFx.Core.Tests has a special treatment which is managed seperately
    if ($fileNameNoExt -ne "Microsoft.PowerFx.Core.Tests")
    {
        $frameworks = @{"net31"="netstandard2.0"; "net6"="net6.0"; "net7"="net7.0"}
        $fileRoot = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "$pfxFolder\src\libraries\$fileNameNoExt\bin\$config")
        $icon = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "$pfxFolder\src\libraries\$fileNameNoExt\icon.png")
    }
    else
    {
        $frameworks = @{"net31"="netcoreapp3.1"; "net6"="net6.0"; "net7"="net7.0"}
        $fileRoot = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "$pfxFolder\src\tests\$fileNameNoExt\bin\$config")
        $icon = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "$pfxFolder\src\tests\$fileNameNoExt\icon.png")
    }
            
    $files = [System.Collections.ArrayList]::new()
    if ($UseDrop)
    {
        foreach ($nugetFolder in [System.IO.Directory]::EnumerateDirectories($dropFolder) | ? { $_ -match ($fileNameNoExt + ".Net") })
        {   
            $fmk = (Select-String ($fileNameNoExt + "\.(Net[0-9]+)\.") -InputObject $nugetFolder).Matches.Groups[1].Value
            $files.AddRange((Get-ChildItem @(($nugetFolder + "\lib"), ($nugetFolder + "\content"), ($nugetFolder + "\contentFiles")) -Recurse -File | % { $frameworks[$fmk] + "|" + $_.FullName + "|" + $nugetFolder}))
        }
    }
    else
    {        
        $fileRoot = [System.IO.Path]::GetFullPath($fileRoot)

        if ($IncludeVersions -contains 'net31') { $files.AddRange((Get-ChildItem -Recurse -Path ("$fileRoot\"+$frameworks["net31"]) -File -Exclude @("*.deps.json", "*.dev.json", "*.pdb", "*.dbg") | % { $frameworks["net31"] + "|" + $_.FullName })) }
        if ($IncludeVersions -contains 'net6')  { $files.AddRange((Get-ChildItem -Recurse -Path ("$fileRoot\"+$frameworks["net6"])  -File -Exclude @("*.deps.json", "*.dev.json", "*.pdb", "*.dbg") | % { $frameworks["net6"]  + "|" + $_.FullName })) }
        if ($IncludeVersions -contains 'net7')  { $files.AddRange((Get-ChildItem -Recurse -Path ("$fileRoot\"+$frameworks["net7"])  -File -Exclude @("*.deps.json", "*.dev.json", "*.pdb", "*.dbg") | % { $frameworks["net7"]  + "|" + $_.FullName })) }
    }

    ## Identify the list of files we need to include/exclude
    $include = [object[]]$nuspec.package.files.include
    $exclude = [string[]]$nuspec.package.files.exclude

    if (-not $UseDrop)
    {
        if ($include -ne $null)
        {
            $files = [System.Linq.Enumerable]::Where([string[]]$files, [Func[string, bool]] { $x = $args[0]; [System.Linq.Enumerable]::Any($include, [Func[object, bool]] { $x -match $args[0].'#text'.Replace("\","\\").Replace("*","").Replace(".","\.") }) })
        }
        if ($exclude -ne $null)
        {
            $files = [System.Linq.Enumerable]::Where([string[]]$files, [Func[string, bool]] { $x = $args[0]; [System.Linq.Enumerable]::Any($exclude, [Func[string, bool]] { $x -notmatch $args[0].Replace("\","\\").Replace("*","").Replace(".","\.") }) })
        }
    }

    $lst = [System.Collections.ArrayList]::new()
    foreach ($fileZ in $files)
    {
        $fmk = $fileZ.Split('|')[0]        
        $file = $fileZ.Split('|')[1]
        
        foreach ($inc in [System.Linq.Enumerable]::ToList([System.Linq.Enumerable]::Where($include, [Func[object, bool]] { $file -match $args[0].'#text'.Replace("\","\\").Replace("*","").Replace(".","\.") })))
        {   
            if ($UseDrop) 
            { 
                $fileRoot = $fileZ.Split('|')[2]
                $t = $file.Substring($fileRoot.Length)
            }        
            else
            {
                $t = $inc.Folder.Replace("FRAMEWORK", $fmk) + $file.Substring($fileRoot.Length + $fmk.Length + 2)
            }
            
            if (-not $lst.Contains($t))
            {
                [void]$lst.Add($t)      
                $xfile = $nuspec.CreateElement("file", $schema)
            
                $src = $nuspec.CreateAttribute("src")
                $src.Value = $file
                [void]$xfile.Attributes.Append($src)

                $target = $nuspec.CreateAttribute("target")                     
                $target.Value = $t                
                [void]$xfile.Attributes.Append($target)

                Write-Host "Adding" $target.Value
                [void]$nuspec.package.files.AppendChild($xfile)
            }
        }        
    }

    $xfile = $nuspec.CreateElement("file", $schema)
    $src = $nuspec.CreateAttribute("src")    
    $src.Value = $icon
    [void]$xfile.Attributes.Append($src)

    $target = $nuspec.CreateAttribute("target")        
    $target.Value = "\icon.png"
    [void]$xfile.Attributes.Append($target)

    [void]$nuspec.package.files.AppendChild($xfile)
    

    ## Remove comments
    foreach ($comment in $nuspec.SelectNodes("//comment()"))
    {
        [void]$comment.ParentNode.RemoveChild($comment)
    }

    ## Remove include/exclude parts which are not following the schema
    $nm = [System.Xml.XmlNamespaceManager]::new($nuspec.NameTable)
    $nm.AddNamespace("n", $schema)

    foreach ($inc in $nuspec.SelectNodes("//n:include", $nm))
    {
        [void]$inc.ParentNode.RemoveChild($inc)
    }
    foreach ($exc in $nuspec.SelectNodes("//n:exclude", $nm))
    {
        [void]$exc.ParentNode.RemoveChild($exc)
    }    
    
    $xmlContent = Format-XML($nuspec.InnerXml)
    Set-Content $newNuspecFile -Value $xmlContent

    nuget.exe pack $newNuspecFile 
}
