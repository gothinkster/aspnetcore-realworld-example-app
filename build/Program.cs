using System;
using System.Collections.Generic;
using System.IO;
using static Bullseye.Targets;
using static SimpleExec.Command;
using GlobExpressions;

public static class Program
{
    private const string Clean = "clean";
    private const string Build = "build";
    private const string Test = "test";
    private const string Publish = "publish";

    static void Main(string[] args)
    {
        Target(Clean,
            ForEach( "publish", "**/bin", "**/obj"),
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

        Target(Build, () => Run("dotnet", "build . -c Release"));

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
                    $"publish {project} -c Release -f netcoreapp3.1 -o ./publish --no-restore --no-build --verbosity=normal");
            });

        Target("default", DependsOn(Publish), () => Console.WriteLine("Done!"));
        RunTargetsAndExit(args);
    }
}