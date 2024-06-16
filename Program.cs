namespace CSharp_FileIO;

class Program
{
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
            if (string.IsNullOrWhiteSpace(newKey))
            {
                PersistKeys(fname, keys, values);
                return;
            }
            else
            {
                Console.Write("Enter value: ");
                int newValue = int.Parse(Console.ReadLine());
                if (keys.Contains(newKey))
                {
                    var index = keys.IndexOf(newKey);
                    values[index] = newValue;
                }
                else
                {
                    keys.Add(newKey);
                    values.Add(newValue);
                }
            }
        } while (!string.IsNullOrWhiteSpace(newKey));
    }

    private static void PersistKeys(string fname, List<string> keys, List<int> values)
    {
        string[] lines = new string[keys.Count];
        for (int i = 0; i < keys.Count; i++)
        {
            lines[i] = $"{keys[i]}:{values[i]}";
        }
        File.WriteAllLines(fname, lines);
        Console.WriteLine("Data saved!");
    }

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
