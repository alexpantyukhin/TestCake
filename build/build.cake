#tool "nuget:?package=NUnit.ConsoleRunner"
#tool "nuget:?package=vswhere"
#tool "nuget:?package=NuGet.CommandLine"
#addin "nuget:?package=Cake.Git&version=1.0.1"
#addin "nuget:?package=Cake.FileHelpers&version=5.0.0"

var target = Argument("target", "Test");
var configuration = Argument("configuration", "Debug");
var frameworks = new string[]{
  "net48",
  "net6.0"
  };

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    CleanDirectory($"../NUnitTestCake/bin/");
    CleanDirectory($"../TestCake/bin/");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetBuild("../TestCake.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .DoesForEach(frameworks , framework =>
{
  var bin = Directory($"./../NUnitTestCake/bin/Debug/{framework}");
  var nunitTestFilesPattern = $"{bin.ToString()}/NUnitTestCake.dll";
  var output = Directory("./output");
  var outputFileName = $"test-results-junit-{framework}.xml";

  var nunit3Settings = new NUnit3Settings 
        {
          NoResults = false,
          WorkingDirectory = bin,
          Results = new List<NUnit3Result>() { 
            new NUnit3Result() 
            {
              FileName = $"{output.ToString()}/{outputFileName}",
              Transform = "nunit3-junit.xslt",
            },
          },
        };


  Information($"nunitTestFilesPattern: {nunitTestFilesPattern}");
  NUnit3(nunitTestFilesPattern, nunit3Settings);
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);