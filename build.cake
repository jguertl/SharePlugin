#addin "Cake.FileHelpers"

var TARGET = Argument ("target", Argument ("t", "Default"));

var version = EnvironmentVariable ("APPVEYOR_BUILD_VERSION") ?? Argument("version", "0.0.9999");

Task ("Default").Does (() =>
{

    const string sln = "./Share.sln";
    const string cfg = "Release";

	NuGetRestore (sln);

    if (!IsRunningOnWindows ())
        DotNetBuild (sln, c => c.Configuration = cfg);
    else
        MSBuild (sln, c => { 
            c.Configuration = cfg;
            c.MSBuildPlatform = MSBuildPlatform.x86;
        });
});

Task ("NuGetPack")
	.IsDependentOn ("Default")
	.Does (() =>
{
	NuGetPack ("./Share.Plugin.nuspec", new NuGetPackSettings { 
		Version = version,
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./",
		BasePath = "./",
		ToolPath = "./tools/nuget3.exe"
	});	
});


RunTarget (TARGET);
