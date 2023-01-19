param(
    [Parameter (Mandatory = $false)] 
    [ValidateSet ('all', 'net31', 'net6', 'net7')] 
    [String[]]$IncludeVersions = 'net31',
    [String]$pfxFolder
)

## To generate these packages locally, build the 3 solutions Microsoft.PowerFx.sln, Microsoft.PowerFx.Net6.sln and Microsoft.PowerFx.Net7.sln in Release mode
## run this Powershell script from the command line .\GeneratePackages.ps1 after having defined the following variables
##   $env:NugetVersion="0.2.4-preview.20230101-2000"  ## version of nuget package to create
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

$IncludeVersions = $IncludeVersions | Select-Object -Unique

if ($IncludeVersions.Length -eq 0)
{
    $IncludeVersions = @('net31')
}

if ($IncludeVersions -contains 'all')
{
    $IncludeVersions = @('net31', 'net6', 'net7')
}

Write-Host "### Generate packages for $IncludeVersions"

$nuspecRoot = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "($pfxFolder)\src\nuspecs\")
cd $nuspecRoot
$pfxHash = (git log -n 1 --pretty=%H).ToString()

foreach ($nuspecFile in (Get-Item ($nuspecRoot + "*.nuspec") | % { $_.FullName }))
{ 
    $fileNameNoExt = [System.IO.Path]::GetFileNameWithoutExtension($nuspecFile)

    Write-Host "Processing $fileNameNoExt..."

    $pkgRoot = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "($pfxFolder)\src\nuspecs\$fileNameNoExt")
        
    if (Test-Path -Path $pkgRoot)
    {
        Remove-Item -Path $pkgRoot -Recurse
    }    

    [void](New-Item $pkgRoot -ItemType Directory)
    $newNuspecFile = [System.IO.Path]::Combine($pkgRoot, "$fileNameNoExt.nuspec")
    Copy-Item $nuspecFile -Destination $newNuspecFile
    
    [xml]$nuspec = Get-Content $newNuspecFile 

    $nuspec.package.metadata.version = $env:NugetVersion

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
                $dep.version = $env:NugetVersion
            }
        }
    }

    if ($fileNameNoExt -ne "Microsoft.PowerFx.Core.Tests")
    {
        $files = [System.Collections.ArrayList]::new()
        $fileRoot = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "($pfxFolder)\src\libraries\$fileNameNoExt\bin\Release")        

        if ($IncludeVersions -contains 'net31') { $files.AddRange((Get-ChildItem ($fileRoot + '\netstandard2.0') -Recurse -File -Exclude @("*.deps.json", "*.pdb", "*.dbg") | % { $_.FullName })) }
        if ($IncludeVersions -contains 'net6')  { $files.AddRange((Get-ChildItem ($fileRoot + '\net6.0')         -Recurse -File -Exclude @("*.deps.json", "*.pdb", "*.dbg") | % { $_.FullName })) }
        if ($IncludeVersions -contains 'net7')  { $files.AddRange((Get-ChildItem ($fileRoot + '\net7.0')         -Recurse -File -Exclude @("*.deps.json", "*.pdb", "*.dbg") | % { $_.FullName })) }

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
            $icon = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "($pfxFolder)\src\libraries\$fileNameNoExt\icon.png")
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
        $fileRoot = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "($pfxFolder)\src\tests\$fileNameNoExt\bin\Release")

        if ($IncludeVersions -contains 'net31') { $files.AddRange((Get-ChildItem -Recurse -Path @("$fileRoot\netcoreapp3.1\ExpressionTestCases\*", "$fileRoot\netcoreapp3.1\IntellisenseTests\*", "$fileRoot\netcoreapp3.1\TestRunnerTests\*") | % { $_.FullName })) }
        if ($IncludeVersions -contains 'net6')  { $files.AddRange((Get-ChildItem -Recurse -Path @("$fileRoot\net6.0\ExpressionTestCases\*",        "$fileRoot\net6.0\IntellisenseTests\*",        "$fileRoot\net6.0\TestRunnerTests\*")        | % { $_.FullName })) }
        if ($IncludeVersions -contains 'net7')  { $files.AddRange((Get-ChildItem -Recurse -Path @("$fileRoot\net7.0\ExpressionTestCases\*",        "$fileRoot\net7.0\IntellisenseTests\*",        "$fileRoot\net7.0\TestRunnerTests\*")        | % { $_.FullName })) }

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
        $icon = [System.IO.Path]::Combine($env:BUILD_SOURCESDIRECTORY, "($pfxFolder)\src\tests\$fileNameNoExt\icon.png")
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

    .\nuget.exe pack $newNuspecFile 
}

