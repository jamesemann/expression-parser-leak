using System.Diagnostics;
using AdaptiveExpressions;

namespace Repro;

public class Program
{
    public static async Task Main()
    {
        for (var i = 0; i <= int.MaxValue; i++)
        {
            var expressionParser = new ExpressionParser();

            // simulate a reasonably long expression to increase consumption 
            var expression = $"{i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i} + {i}";

            // subject under test
            expressionParser.Parse(expression);

            if (i % 10000 == 0)
            {
                // every 10k iterations, gc collect and then log the memory consumption of this process

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                MemoryDetails();
                Thread.Sleep(1000);
            }
        }
    }

    private static void MemoryDetails()
    {
        string FormatMemory(long bytes)
        {
            const int scale = 1024;
            var memoryUnits = new[]
            {
                "B", "KB", "MB", "GB", "TB",
            };

            var i = 0;
            var memorySize = bytes;
            while (memorySize >= scale && i < memoryUnits.Length - 1)
            {
                memorySize /= scale;
                i++;
            }

            return $"{memorySize:0.##} {memoryUnits[i]}";
        }
        
        Console.WriteLine($"Memory used: {FormatMemory(Process.GetCurrentProcess().PrivateMemorySize64)}");
    }
}