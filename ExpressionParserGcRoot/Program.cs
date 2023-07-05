using System.Diagnostics;
using AdaptiveExpressions;

namespace Repro;

public class Program
{
    public static async Task Main()
    {
        for (var i = 0; i < int.MaxValue; i++)
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
        // Get the current process
        var currentProcess = Process.GetCurrentProcess();

        // Retrieve the memory usage
        var memoryUsed = currentProcess.PrivateMemorySize64;

        // Convert the memory usage to a human-readable format
        var formattedMemoryUsed = FormatMemory(memoryUsed);

        // Print the memory usage
        Console.WriteLine($"Memory used: {formattedMemoryUsed}");
    }

    private static string FormatMemory(long bytes)
    {
        const int scale = 1024;
        string[] memoryUnits =
        {
            "B", "KB", "MB", "GB", "TB",
        };

        var i = 0;
        double memorySize = bytes;
        while (memorySize >= scale && i < memoryUnits.Length - 1)
        {
            memorySize /= scale;
            i++;
        }

        return $"{memorySize:0.##} {memoryUnits[i]}";
    }
}