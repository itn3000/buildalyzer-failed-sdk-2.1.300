using System;
using System.Linq;
using Buildalyzer;
using Microsoft.Build;
using Microsoft.Build.Framework;
using System.IO;

namespace buildalyzertest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                System.Runtime.Loader.AssemblyLoadContext.Default.Resolving += (ctx, name) =>
                {
                    Console.WriteLine($"asm name={name}");
                    return null;
                };
                // another workaround, read required assemblies before compile.
                foreach(var dllname in new string[]{"NuGet.Versioning", "NuGet.Common","NuGet.Frameworks"})
                {
                    const string sdkdir = @"C:\Program Files\dotnet\sdk\2.1.300";
                    System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(sdkdir, $"{dllname}.dll"));
                }
                var manageropts = new AnalyzerManagerOptions();
                manageropts.LogWriter = Console.Out;
                manageropts.LoggerVerbosity = LoggerVerbosity.Normal;
                var manager = new AnalyzerManager(manageropts);
                var analyzer = manager.GetProject(args[0]);
                var project = analyzer.Compile();
                if (project != null)
                {
                    foreach (var x in project.Items)
                    {
                        var meta = x.Metadata.Select(y => $"{y.Name}={y.EvaluatedValue}");
                        Console.WriteLine($"{x.ItemType},{string.Join("|", meta)}");
                    }
                }
                else
                {
                    Console.WriteLine($"project instance is null");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
            }
        }
    }
}
