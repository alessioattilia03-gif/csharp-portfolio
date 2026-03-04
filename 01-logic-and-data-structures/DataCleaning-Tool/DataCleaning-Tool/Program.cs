using System;
using System.Collections.Generic;

namespace DataPipeline
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("=== data cleaning pipeline initialized ===");
            Console.WriteLine();

            DataSanitizer.RunSanitization();

            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine();

            LockoutManager.SimulateLockout();

            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine();

            LogExtractor.ExtractIp();
        }
    }

    // caso 1: sanitizzazione estrema
    public static class DataSanitizer
    {
        public static void RunSanitization()
        {
            // raw array containing dirty inputs
            string[] dirtyData = { "  admin_user  ", "", "   ", null, "null", "guest", "NULL" };
            List<string> cleanList = new List<string>();

            Console.WriteLine("[*] starting data sanitization process...");

            foreach (string entry in dirtyData)
            {
                // block nulls, empty strings, whitespaces and literal "null" strings safely
                if (!string.IsNullOrWhiteSpace(entry) && entry.ToLower() != "null")
                {
                    cleanList.Add(entry.Trim());
                }
            }

            Console.WriteLine($"    [+] recovered {cleanList.Count} valid entries:");
            foreach (string cleanEntry in cleanList)
            {
                Console.WriteLine($"        -> {cleanEntry}");
            }
        }
    }

    // caso 2: gestione lockout con nullable types
    public static class LockoutManager
    {
        public static void SimulateLockout()
        {
            // nullable integer initialized to null
            int? failedAttempts = null;

            Console.WriteLine("[*] starting lockout simulation.");
            Console.WriteLine("    press enter to simulate a failed login (type 'exit' to abort).");

            while (true)
            {
                Console.Write("    [?] action: ");
                string input = Console.ReadLine();

                if (input?.ToLower() == "exit")
                {
                    Console.WriteLine("    [-] simulation aborted.");
                    break;
                }

                // safely initialize or increment the nullable variable
                if (!failedAttempts.HasValue)
                {
                    failedAttempts = 1;
                }
                else
                {
                    failedAttempts++;
                }

                // safely evaluate the value to prevent null reference exceptions
                if (failedAttempts.HasValue && failedAttempts.Value >= 3)
                {
                    Console.WriteLine("    [!] security alert: account locked. maximum attempts reached.");
                    break;
                }
                else
                {
                    Console.WriteLine($"    [-] attempt failed. total failures: {failedAttempts.Value}/3");
                }
            }
        }
    }

    // caso 3: estrazione dati strutturati
    public static class LogExtractor
    {
        public static void ExtractIp()
        {
            string rawLog = "error_code:404|ip:10.0.0.5|msg:not_found";

            Console.WriteLine("[*] processing raw log string...");
            Console.WriteLine($"    raw data: {rawLog}");

            // split the string using the pipe character to isolate fragments
            string[] blocks = rawLog.Split('|');

            // verify array length to prevent index out of bounds exceptions
            if (blocks.Length > 1)
            {
                string ipFragment = blocks[1];
                Console.WriteLine($"    [+] extracted target: {ipFragment}");
            }
            else
            {
                Console.WriteLine("    [x] error: log format does not match expected structure.");
            }
        }
    }
}