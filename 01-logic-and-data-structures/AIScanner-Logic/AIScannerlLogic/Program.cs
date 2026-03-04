using System;

namespace AIScannerLogic
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("=== security system initialized ===");
            Console.WriteLine();

            ServerScanner.RunScan();

            Console.WriteLine();
            Console.WriteLine("===================================");
            Console.WriteLine();

            AnomalyScorer.RunEvaluation();
        }
    }

    public static class ServerScanner
    {
        public static void RunScan()
        {
            // initialize a 3x3 array (1 = open port, 0 = closed port)
            int[,] serverMatrix = new int[,]
            {
                { 1, 0, 0 },
                { 0, 1, 1 },
                { 1, 0, 0 }
            };

            Console.WriteLine("[*] starting matrix scan for open ports...");

            // scan matrix dynamically using getlength to prevent out-of-bounds
            for (int row = 0; row < serverMatrix.GetLength(0); row++)
            {
                for (int col = 0; col < serverMatrix.GetLength(1); col++)
                {
                    if (serverMatrix[row, col] == 1)
                    {
                        Console.WriteLine($"    -> open port detected at [row: {row}, col: {col}]");
                    }
                }
            }

            Console.WriteLine("[*] scan completed.");
        }
    }

    public static class AnomalyScorer
    {
        public static void RunEvaluation()
        {
            Console.Write("[?] enter anomaly score (e.g. 0,85): ");
            string input = Console.ReadLine();

            // parse input safely to prevent runtime formatting crashes
            if (double.TryParse(input, out double score))
            {
                ClassifyScore(score);
            }
            else
            {
                Console.WriteLine("    [x] error: input is not a valid decimal number.");
            }
        }

        private static void ClassifyScore(double score)
        {
            // evaluate score severity using cascaded if-else conditions
            if (score >= 08)
            {
                Console.WriteLine("    [!] alert: critical anomaly detected.");
            }
            else if (score >= 0.5)
            {
                Console.WriteLine("    [-] warning: suspicious activity found.");
            }
            else
            {
                Console.WriteLine("    [+] info: system status normal.");
            }
        }
    }
}