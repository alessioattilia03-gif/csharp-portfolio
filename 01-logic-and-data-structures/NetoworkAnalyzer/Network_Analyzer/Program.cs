using System;
using System.Collections.Generic;

/// <summary>
/// Network Traffic & Threat Analyzer
/// Demonstrates robust collection manipulation: arrays, lists, and dictionaries.
/// Handles edge cases to prevent runtime exceptions.
/// </summary>
class ThreatAnalyzer
{
    // Constants 
    private const int IpPartsLength = 4;
    private const int MaxOctetValue = 255;

    static void Main(string[] args)
    {
        string[] ipBlacklist = {
            "192.168.1.105", "10.0.0.23", "172.16.254.1", "8.8.8.8", "1.1.1.1",
            "127.0.0.1", "185.220.101.45", "45.33.32.156", "203.0.113.42", "91.214.124.18"
        };

        // Dictionary persistence outside the loop 
        Dictionary<string, int> enemiesFeed = new Dictionary<string, int>
        {
            {"193.201.224.212", 0}, {"80.248.237.155", 0}, {"103.212.69.4", 0}
        };

        while (true)
        {
            Console.WriteLine("\n────────────────────────────────────────────────");
            Console.WriteLine(" [1] Filter BlackList (Array Search)");
            Console.WriteLine(" [2] Quarantine Log (List Manipulation)");
            Console.WriteLine(" [3] Enemies Map (Dictionary Persistence)");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" [0] Exit");
            Console.ResetColor();
            Console.WriteLine("────────────────────────────────────────────────");
            Console.Write(" > Choose an option: ");

            string inputUtente = Console.ReadLine();

            switch (inputUtente)
            {
                case "1":
                    Console.Write("Enter IP: ");
                    string inputIP = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(inputIP)) { Console.WriteLine("Error: String is empty."); break; }

                    if (ValidateIPv4(inputIP)) 
                    {
                        bool res = false;
                        for (int i = 0; i < ipBlacklist.Length; i++) 
                        {
                            if (ipBlacklist[i] == inputIP) { res = true; break; }
                        }
                        if (res) Console.WriteLine("🚨 Alert: IP found in Blacklist!");
                        else Console.WriteLine("😀 IP not found, clean traffic!");
                    }
                    else { Console.WriteLine("Error: Not a valid IPv4 address."); }
                    WaitForKey();
                    break;

                case "2":
                    List<string> listLog = new List<string> { "LOGIN_SUCCESS", "SQL_INJECTION_ATTEMPT", "TIMEOUT_EXCEEDED", "BUFFER_OVERFLOW_DETECTED" };
                    List<string> quarantine = new List<string>();

                    // Backward loop to safely remove items 
                    for (int i = listLog.Count - 1; i >= 0; i--)
                    {
                        if (listLog[i].Contains("INJECTION") || listLog[i].Contains("OVERFLOW"))
                        {
                            quarantine.Add(listLog[i]);
                            listLog.RemoveAt(i);
                        }
                    }

                    Console.WriteLine("\nNow the list of threats consist of:");
                    foreach (var threat in quarantine) { Console.WriteLine($"⚠️ {threat}"); }
                    WaitForKey();
                    break;

                case "3":
                    Console.Clear();
                    Console.WriteLine("=== ENEMIES ATTACK MAP ===");
                    Console.Write("Enter source IP: ");
                    string inputIP3 = Console.ReadLine()?.Trim();

                    if (ValidateIPv4(inputIP3))
                    {
                        if (enemiesFeed.ContainsKey(inputIP3)) enemiesFeed[inputIP3]++; 
                        else enemiesFeed.Add(inputIP3, 1);

                        Console.WriteLine("\n✅ Updated Attack Map:");
                        foreach (var entry in enemiesFeed) { Console.WriteLine($"- IP: {entry.Key} | Attacks: {entry.Value}"); }
                    }
                    else { Console.WriteLine("Error: Invalid Format."); }
                    WaitForKey();
                    break;

                case "0":
                    Console.WriteLine("Exiting program...");
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    WaitForKey();
                    break;
            } // End Switch
        } // End While
    }

    static bool ValidateIPv4(string ip)
    {
        if (string.IsNullOrEmpty(ip)) return false;
        string[] parts = ip.Split('.');
        if (parts.Length != IpPartsLength) return false;

        foreach (string part in parts)
        {
            if (!int.TryParse(part, out int octet) || octet < 0 || octet > MaxOctetValue) return false;
        }
        return true;
    }

    static void WaitForKey()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }
}