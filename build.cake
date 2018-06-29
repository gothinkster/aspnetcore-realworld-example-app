var target = Argument("target", "Default");
var tag = Argument("tag", "cake");

Task("Restore")
  .Does(() =>
{
    DotNetCoreRestore(".");
});

Task("Build")
  .Does(() =>
{
    DotNetCoreBuild(".");
});

Task("Test")
  .Does(() =>
{
    var files = GetFiles("tests/**/*.csproj");
    foreach(var file in files)
    {
        DotNetCoreTest(file.ToString());
    }
});

Task("Publish")
  .Does(() =>
{
    var settings = new DotNetCorePublishSettings
    {
        Framework = "netcoreapp2.1",
        Configuration = "Release",
        OutputDirectory = "./publish",
        Runtime = "linux-x64",
        VersionSuffix = tag
    };
                
    DotNetCorePublish("src/Conduit", settings);
});

Task("Default")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Publish");

 Task("Rebuild")
    .IsDependentOn("Restore")
    .IsDependentOn("Build");


RunTarget(target);