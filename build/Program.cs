using System;
using System.Collections.Generic;
using System.IO;
using GlobExpressions;
using static Bullseye.Targets;
using static SimpleExec.Command;

const string Clean = "clean";
const string Build = "build";
const string Test = "test";
const string Format = "format";
const string Publish = "publish";

Target(Clean,
    ForEach("publish", "**/bin", "**/obj"),
    dir =>
    {
        IEnumerable<string> GetDirectories(string d)
        {
            return Glob.Directories(".", d);
        }

        void RemoveDirectory(string d)
        {
            if (Directory.Exists(d))
            {
                Console.WriteLine($"Cleaning {d}");
                Directory.Delete(d, true);
            }
        }

        foreach (var d in GetDirectories(dir))
        {
            RemoveDirectory(d);
        }
    });


Target(Format, () =>
{
    Run("dotnet", "tool restore");
    Run("dotnet", "format --check");
});

Target(Build, DependsOn(Format), () => Run("dotnet", "build . -c Release"));

Target(Test, DependsOn(Build),
    () =>
    {
        IEnumerable<string> GetFiles(string d)
        {
            return Glob.Files(".", d);
        }

        foreach (var file in GetFiles("tests/**/*.csproj"))
        {
            Run("dotnet", $"test {file} -c Release --no-restore --no-build --verbosity=normal");
        }
    });

Target(Publish, DependsOn(Test),
    ForEach("src/Conduit"),
    project =>
    {
        Run("dotnet",
            $"publish {project} -c Release -f net5.0 -o ./publish --no-restore --no-build --verbosity=normal");
    });

Target("default", DependsOn(Publish), () => Console.WriteLine("Done!"));
await RunTargetsAndExitAsync(args);

