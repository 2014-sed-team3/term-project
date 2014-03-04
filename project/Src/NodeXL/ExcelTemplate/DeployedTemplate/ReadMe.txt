
There are multiple versions of the NodeXLGraph.xltx template file in this
solution:

1. ExcelTemplate\NodeXLGraph.xltx    This is the template file without any
   custom properties.

2. ExcelTemplate\bin\Debug\NodeXLGraph.xltx    Visual Studio creates this at
   build time by making a copy of ExcelTemplate\NodeXLGraph.xltx and adding
   custom properties named _AssemblyLocation, _AssemblyName and Solution ID.
   The _AssemblyLocation property has this value:
   
       Smrf.NodeXL.ExcelTemplate.vsto|aa51c0f3-62b4-4782-83a8-a15dcdd17698
	   |vstolocal

3. ExcelTemplate\Publish\NodeXLGraph.xltx    Visual Studio creates this at
   publish time by making a copy of ExcelTemplate\NodeXLGraph.xltx and adding
   custom properties named _AssemblyLocation, _AssemblyName and Solution ID.
   The _AssemblyLocation property has this value:

	   http://www.nodexlgraphgallery.org/NodeXLSetup/Smrf.NodeXL.
	   ExcelTemplate.vsto|aa51c0f3-62b4-4782-83a8-a15dcdd17698

   (The URL here came from the "Installation Folder URL" in the Publish
   properties for the ExcelTemplate project.)

   This is the version that Microsoft intended users to manually copy to their
   computer and open.  Microsoft's scheme isn't appropriate here, though,
   because we don't want to force the user to manually copy anything.  Instead,
   we want him to automatically get the template during initial deployment, and
   to automatically get the latest template with each new NodeXL release.

   Thus, ExcelTemplate\Publish\NodeXLGraph.xltx is part of the publish package
   but does not actually get deployed to the user's computer.  It's ignored.
   
4. ExcelTemplate\DeployedTemplate\NodeXLGraph.xltx    This was manually copied
   from ExcelTemplate\bin\Debug\NodeXLGraph.xltx.  THIS is the template file
   that actually gets deployed to the user's computer when he installs the
   published solution.  It's in a DeployedTemplate subfolder, which makes it
   part of the deployed ClickOnce package.  The _AssemblyLocation property has
   this value:

       Smrf.NodeXL.ExcelTemplate.vsto|aa51c0f3-62b4-4782-83a8-a15dcdd17698|
	   vstolocal

   That value is the wrong one for the user's computer, so a post-deployment
   action (implemented in the ExcelTemplatePostDeploymentAction project)
   changes it to the correct value:

	   http://www.nodexlgraphgallery.org/NodeXLSetup/Smrf.NodeXL.
	   ExcelTemplate.vsto|aa51c0f3-62b4-4782-83a8-a15dcdd17698

   With this scheme, each version gets deployed with its own template file.
   The user doesn't have to manually copy anything, and he always uses the
   latest template file .


IMPORTANT NOTE:

If you edit the ExcelTemplate\NodeXLGraph.xltx file, you must also do the
following:

1. Build the solution.  This updates ExcelTemplate\bin\Debug\NodeXLGraph.xltx.

2. Copy ExcelTemplate\bin\Debug\NodeXLGraph.xltx to
   ExcelTemplate\DeployedTemplate\NodeXLGraph.xltx.  This updates the template
   file that gets deployed to the user's computer.  See NodeXL\ExcelTemplate
   \HowToPublishThisProject.txt for more details.

