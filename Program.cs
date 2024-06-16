namespace CSharp_FileIO;

class Program
{
    /// <summary>
    /// Main entry point
    /// Query all user input here
    /// </summary>
    static void Main(string[] args)
    {
        Console.Write("Hello! Please specify a file name: ");
        string fname = Console.ReadLine();
        List<string> keys = new List<string>();
        List<int> values = new List<int>();
        var sucess = ReadConfigFile(fname, keys, values);
        if (sucess)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                Console.WriteLine($"{keys[i]}: {values[i]}");
            }
        }

        string newKey = string.Empty;
        do
        {  
            Console.Write("Add/update key (blank line to exit): ");
            newKey = Console.ReadLine();
            Console.Write("Enter value: ");
            int newValue = int.Parse(Console.ReadLine());
            AddOrUpdateValue(keys, values, newKey, newValue);
        } while (!string.IsNullOrWhiteSpace(newKey));
        PersistKeys(fname, keys, values);
        Console.WriteLine("Data saved!");
    }

    /// <summary>
    /// Add or update a key value pair to config file
    /// if the key exists, update the value
    /// if the key does not exist, add the key value pair
    /// </summary>
    static bool AddOrUpdateValue(List<string> keys, List<int> values, string key, int value)
    {
        if (keys.Contains(key))
        {
            var index = keys.IndexOf(key);
            values[index] = value;
            return true;
        }
        else
        {
            keys.Add(key);
            values.Add(value);
        }
        return false;
    }

    /// <summary>
    /// Save the keys and values to a file
    /// </summary>
    private static void PersistKeys(string fname, List<string> keys, List<int> values)
    {
        string[] lines = new string[keys.Count];
        for (int i = 0; i < keys.Count; i++)
        {
            lines[i] = $"{keys[i]}:{values[i]}";
        }
        File.WriteAllLines(fname, lines);
    }

    /// <summary>
    /// Read the config file and populate the keys and values
    /// </summary>
    static bool ReadConfigFile(string fname, List<string> key, List<int> values)
    {
        try
        {
            var content = File.ReadAllLines(fname);
            ParseContent(content, key, values);
            return true;
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return false;
    }

    /// <summary>
    /// Parse the content of the file
    /// for each line, split the line by ':'
    /// the first part is the key, the second part is the value
    /// </summary>
    private static void ParseContent(string[] content, List<string> key, List<int> values)
    {
        foreach (var line in content)
        {
            var parts = line.Split(':');
            key.Add(parts[0]);
            values.Add(int.Parse(parts[1]));
        }
    }
}
