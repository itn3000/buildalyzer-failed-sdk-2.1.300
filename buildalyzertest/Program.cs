using System;
using System.Linq;
using Buildalyzer;
using Microsoft.Build;
using Microsoft.Build.Framework;

namespace buildalyzertest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
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
