
REM  Copy everything.

rd /s /q C:\Temp\NodeXLNetworkServerRelease

xcopy bin\Debug C:\Temp\NodeXLNetworkServerRelease /s /i


REM  Delete unneeded files.

del C:\Temp\NodeXLNetworkServerRelease\*.pdb

del C:\Temp\NodeXLNetworkServerRelease\*.xml

rd /s /q "C:\Temp\NodeXLNetworkServerRelease\SampleNodeXLWorkbook"

rd /s /q "C:\Temp\NodeXLNetworkServerRelease\SplashScreen"

del "C:\Temp\NodeXLNetworkServerRelease\NodeXLExcelTemplate.chm"


REM  Get the documentation.

copy "Documents\NodeXLNetworkServerFAQ.docx" "C:\Temp\NodeXLNetworkServerRelease"

copy "SampleNetworkConfiguration.xml" "C:\Temp\NodeXLNetworkServerRelease"


REM  Get the real template file, which has the correct _AssemblyLocation value.
REM  NodeXLNetworkServer.exe looks for the template in the DeployedTemplate
REM  folder.

copy "..\ExcelTemplate\Publish\NodeXLGraph.xltx" "C:\Temp\NodeXLNetworkServerRelease\DeployedTemplate\NodeXLGraph.xltx"


start C:\Temp\NodeXLNetworkServerRelease
