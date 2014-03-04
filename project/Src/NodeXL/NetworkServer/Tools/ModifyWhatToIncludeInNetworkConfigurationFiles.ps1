
$sUsage = @"
For each NodeXL Network Server network configuration file in a specified
folder, this PowerShell script adds a NonRepliesToNonMentionsEdges option to
the NetworkConfiguration/TwitterSearchNetworkConfiguration/WhatToInclude
element.

This can be used to update network configuration files that were created for
earlier versions of the NodeXL Network Server that did not support this option.

Sample usage:

PowerShell .\ModifyWhatToIncludeInNetworkConfigurationFiles "C:\ConfigFiles"
"@

Write-Host ""

if ($args.Length -ne 1)
{

    Write-Host $sUsage
    Exit
}

[String]$sFolder = $args[0]

if (-not [System.IO.Directory]::Exists($sFolder) )
{
    Write-Host ("The folder " + $sFolder + " doesn't exist.") `
        -ForegroundColor red

    Exit
}

foreach ($oXmlFileInfo in get-childitem ($sFolder + "\*") -include *.xml)
{
    [System.Xml.XmlDocument]$oXmlDocument = `
        New-Object -TypeName System.Xml.XmlDocument

    [String]$sFilePath = $oXmlFileInfo.FullName

    try
    {
        $oXmlDocument.Load($sFilePath)
    }
    catch [System.Exception]
    {
        Write-Host ("Skipping " + $sFilePath `
            + ", which is not a valid XML file.")

        continue
    }

    Write-Host "Adding option to" $sFilePath

    # Fix erroneous end-of-comments.

    (Get-Content $sFilePath) | `
        Foreach-Object {$_ -replace "-->>", "-->"} | Set-Content $sFilePath

    # Do actual replacement.

    (Get-Content $sFilePath) | `
        Foreach-Object {$_ -replace "MentionsEdges</WhatToInclude>", `
            "MentionsEdges,NonRepliesToNonMentionsEdges</WhatToInclude>"} | `
        Set-Content $sFilePath
}
