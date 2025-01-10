using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter the file path for the list of names (e.g., names.txt):");
        string namesFilePath = Console.ReadLine();

        Console.WriteLine("Enter the file path for the list of dates (e.g., dates.txt):");
        string datesFilePath = Console.ReadLine();

        Console.WriteLine("Enter the month and year for this roster (e.g., January 2025):");
        string monthYear = Console.ReadLine();

        // Load names and dates
        List<string> names = LoadFile(namesFilePath);
        List<string> dates = LoadFile(datesFilePath);

        if (names.Count == 0 || dates.Count == 0)
        {
            Console.WriteLine("Error: One or both files are empty or invalid.");
            return;
        }

        // Shuffle names for randomness
        Random rng = new Random();
        Shuffle(names, rng);

        // Generate the duty roster
        Dictionary<string, string> dutyRoster = AssignDuties(names, dates, rng);

        // Output the duty roster
        string outputFilePath = $"DutyRoster_{monthYear.Replace(" ", "_")}.txt";
        SaveDutyRoster(dutyRoster, monthYear, outputFilePath);

        Console.WriteLine($"Duty roster generated successfully: {outputFilePath}\n");

        // Print the roster to the console
        Console.WriteLine($"Duty Roster for {monthYear}");
        Console.WriteLine(new string('-', 30));
        foreach (var entry in dutyRoster)
        {
            Console.WriteLine($"{entry.Key}: {entry.Value}");
        }
    }

    static List<string> LoadFile(string filePath)
    {
        try
        {
            return new List<string>(File.ReadAllLines(filePath));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading file {filePath}: {ex.Message}");
            return new List<string>();
        }
    }

    static void Shuffle(List<string> list, Random rng)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            string value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    static Dictionary<string, string> AssignDuties(List<string> names, List<string> dates, Random rng)
    {
        Dictionary<string, string> roster = new Dictionary<string, string>();
        int nameIndex = 0;

        foreach (string date in dates)
        {
            roster[date] = names[nameIndex];
            nameIndex = (nameIndex + 1) % names.Count; // Cycle through names
        }

        return roster;
    }

    static void SaveDutyRoster(Dictionary<string, string> dutyRoster, string monthYear, string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Duty Roster for {monthYear}");
                writer.WriteLine(new string('-', 30));

                foreach (var entry in dutyRoster)
                {
                    writer.WriteLine($"{entry.Key}: {entry.Value}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving duty roster: {ex.Message}");
        }
    }
}
