param(
    [Parameter (Mandatory = $false)] 
    [ValidateSet ('all', 'net31', 'net6', 'net7')] 
    [String[]]$IncludeVersions = 'net31',
    [String]$pfxFolder,
    [String]$NugetVersion
)

## To generate these packages locally, build the 3 solutions Microsoft.PowerFx.sln, Microsoft.PowerFx.Net6.sln and Microsoft.PowerFx.Net7.sln in Release mode
## run this Powershell script from the command line .\GeneratePackages.ps1 after having defined the following variables
##   $NugetVersion="0.2.4-preview.20230101-2000"  ## version of nuget package to create
##   $env:BUILD_SOURCESDIRECTORY = "C:\Data\Power-Fx" ## location of source directory

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

$IncludeVersions = [string[]]($IncludeVersions | Select-Object -Unique)
$all = $false

if ($IncludeVersions.Length -eq 0)
{
    $IncludeVersions = [string[]]@('net31')
}

if ($IncludeVersions -contains 'all')
{
    $all = $true
    $IncludeVersions = [string[]]@('net31', 'net6', 'net7')
}

$idSuffix = [System.String]::Join('-', [System.Linq.Enumerable]::Select($IncludeVersions, [func[string,string]]{$args[0].Substring(0,1).ToUpper()+$args[0].Substring(1) }));

Write-Host "### Generate packages for $IncludeVersions"
Write-Host "NugetVersion = $NugetVersion"
Write-Host "IdSuffix = $idSuffix"

if ([System.String]::IsNullOrEmpty($pfxFolder))
{
    $pfxFolder = "."
}

$nuspecRoot = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "$pfxFolder\src\nuspecs\")
cd $nuspecRoot
$pfxHash = (git log -n 1 --pretty=%H).ToString()

foreach ($nuspecFile in (Get-Item ($nuspecRoot + "*.nuspec") | % { $_.FullName }))
{ 
    $fileNameNoExt = [System.IO.Path]::GetFileNameWithoutExtension($nuspecFile)

    Write-Host "Processing $fileNameNoExt..."

    $pkgRoot = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "$pfxFolder\src\nuspecs\$fileNameNoExt")
        
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

    if ($fileNameNoExt -ne "Microsoft.PowerFx.Core.Tests")
    {
        $files = [System.Collections.ArrayList]::new()
        $fileRoot = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "$pfxFolder\src\libraries\$fileNameNoExt\bin\Release")        

        if ($IncludeVersions -contains 'net31') { $files.AddRange((Get-ChildItem ($fileRoot + '\netstandard2.0') -Recurse -File -Exclude @("*.deps.json", "*.pdb", "*.dbg") | % { $_.FullName })) }
        if ($IncludeVersions -contains 'net6')  { $files.AddRange((Get-ChildItem ($fileRoot + '\net6.0')         -Recurse -File -Exclude @("*.deps.json", "*.pdb", "*.dbg") | % { $_.FullName })) }
        if ($IncludeVersions -contains 'net7')  { $files.AddRange((Get-ChildItem ($fileRoot + '\net7.0')         -Recurse -File -Exclude @("*.deps.json", "*.pdb", "*.dbg") | % { $_.FullName })) }

        $include = [string[]]$nuspec.package.files.include
        $exclude = [string[]]$nuspec.package.files.exclude

        if ($include -ne $null)
        {
            $files = [System.Linq.Enumerable]::Where([string[]]$files, [Func[string, bool]] { $x = $args[0]; [System.Linq.Enumerable]::Any($include, [Func[string, bool]] { $x -match $args[0].Replace("*","").Replace(".","\.") }) })
        }
        if ($exclude -ne $null)
        {
            $files = [System.Linq.Enumerable]::Where([string[]]$files, [Func[string, bool]] { $x = $args[0]; [System.Linq.Enumerable]::Any($exclude, [Func[string, bool]] { $x -notmatch $args[0].Replace("*","").Replace(".","\.") }) })
        }

        foreach ($file in $files)
        {
            $xfile = $nuspec.CreateElement("file", "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd")
            
            $src = $nuspec.CreateAttribute("src")
            $src.Value = $file
            [void]$xfile.Attributes.Append($src)

            $target = $nuspec.CreateAttribute("target")
            $relative = "\lib" + $file.Substring($fileRoot.Length)
            $target.Value = $relative
            [void]$xfile.Attributes.Append($target)

            [void]$nuspec.package.files.AppendChild($xfile)
        }

        if ($fileNameNoExt -ne "Microsoft.PowerFx.Interpreter")
        {
            $xfile = $nuspec.CreateElement("file", "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd")
            $src = $nuspec.CreateAttribute("src")
            $icon = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "$pfxFolder\src\libraries\$fileNameNoExt\icon.png")
            $src.Value = $icon
            [void]$xfile.Attributes.Append($src)

            $target = $nuspec.CreateAttribute("target")        
            $target.Value = "\icon.png"
            [void]$xfile.Attributes.Append($target)

            [void]$nuspec.package.files.AppendChild($xfile)
        }
        
        foreach ($comment in $nuspec.SelectNodes("//comment()"))
        {
            [void]$comment.ParentNode.RemoveChild($comment)
        }
    }
    else
    {            
        $files = [System.Collections.ArrayList]::new()
        $fileRoot = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "$pfxFolder\src\tests\$fileNameNoExt\bin\Release")

        if ($IncludeVersions -contains 'net31') { $files.AddRange((Get-ChildItem -Recurse -Path @("$fileRoot\netcoreapp3.1\ExpressionTestCases\*", "$fileRoot\netcoreapp3.1\IntellisenseTests\*") | % { $_.FullName })) }
        if ($IncludeVersions -contains 'net6')  { $files.AddRange((Get-ChildItem -Recurse -Path @("$fileRoot\net6.0\ExpressionTestCases\*",        "$fileRoot\net6.0\IntellisenseTests\*")        | % { $_.FullName })) }
        if ($IncludeVersions -contains 'net7')  { $files.AddRange((Get-ChildItem -Recurse -Path @("$fileRoot\net7.0\ExpressionTestCases\*",        "$fileRoot\net7.0\IntellisenseTests\*")        | % { $_.FullName })) }

        $include = [string[]]$nuspec.package.files.include
        $exclude = [string[]]$nuspec.package.files.exclude

        if ($include -ne $null)
        {
            $files = [System.Linq.Enumerable]::Where([string[]]$files, [Func[string, bool]] { $x = $args[0]; [System.Linq.Enumerable]::Any($include, [Func[string, bool]] { $x -match $args[0].Replace("*","").Replace(".","\.") }) })
        }
        if ($exclude -ne $null)
        {
            $files = [System.Linq.Enumerable]::Where([string[]]$files, [Func[string, bool]] { $x = $args[0]; [System.Linq.Enumerable]::Any($exclude, [Func[string, bool]] { $x -notmatch $args[0].Replace("*","").Replace(".","\.") }) })
        }

        foreach ($file in $files)
        {
            $xfile = $nuspec.CreateElement("files", "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")
            
            $include = $nuspec.CreateAttribute("include")
            $include.Value = "any" + $file.Substring($fileRoot.Length)
            [void]$xfile.Attributes.Append($include)

            $buildAction = $nuspec.CreateAttribute("buildAction")            
            $buildAction.Value = "Content"
            [void]$xfile.Attributes.Append($buildAction)

            [void]$nuspec.package.metadata.contentFiles.AppendChild($xfile)
        }

        $files = [System.Collections.ArrayList]::new()
        if ($IncludeVersions -contains 'net31') { $files.AddRange((Get-ChildItem ($fileRoot + '\netcoreapp3.1') -Recurse -File -Exclude @("*.deps.json", "*.pdb", "*.dbg") | % { $_.FullName })) }
        if ($IncludeVersions -contains 'net6')  { $files.AddRange((Get-ChildItem ($fileRoot + '\net6.0')        -Recurse -File -Exclude @("*.deps.json", "*.pdb", "*.dbg") | % { $_.FullName })) }
        if ($IncludeVersions -contains 'net7')  { $files.AddRange((Get-ChildItem ($fileRoot + '\net7.0')        -Recurse -File -Exclude @("*.deps.json", "*.pdb", "*.dbg") | % { $_.FullName })) }

        foreach ($file in $file)
        {
            $xfile = $nuspec.CreateElement("file", "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")
            
            $src = $nuspec.CreateAttribute("src")
            $src.Value = $file
            [void]$xfile.Attributes.Append($src)

            $target = $nuspec.CreateAttribute("target")
            $relative = "\lib" + $file.Substring($fileRoot.Length)
            $target.Value = $relative
            [void]$xfile.Attributes.Append($target)

            [void]$nuspec.package.files.AppendChild($xfile)
        }

        $xfile = $nuspec.CreateElement("file", "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")
        $src = $nuspec.CreateAttribute("src")
        $icon = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "$pfxFolder\src\tests\$fileNameNoExt\icon.png")
        $src.Value = $icon
        [void]$xfile.Attributes.Append($src)

        $target = $nuspec.CreateAttribute("target")        
        $target.Value = "\icon.png"
        [void]$xfile.Attributes.Append($target)

        [void]$nuspec.package.files.AppendChild($xfile)

        foreach ($comment in $nuspec.SelectNodes("//comment()"))
        {
            [void]$comment.ParentNode.RemoveChild($comment)
        }
    }
    
    $xmlContent = Format-XML($nuspec.InnerXml)
    Set-Content $newNuspecFile -Value $xmlContent

    nuget.exe pack $newNuspecFile 
}

