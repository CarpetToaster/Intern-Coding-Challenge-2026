class Solution
{
    public class Reading
    {
        public int Id { get; set; }  
        public int Latitude { get; set; }
        public int Longitude { get; set; }
    }


    public static bool CompareReadings(Reading r1, Reading r2)
    {
        if (Math.Abs(r1.Latitude - r2.Latitude) < 0.001 && Math.Abs(r1.Longitude - r2.Longitude) < 0.001)
        {
            return true;
        }
        return false;
    } 

    /**
    * Remember to use Delegate(args) {compareReadings(args);} as the argument for comparator.
    * In this case, the callback isn't necessary since a) only one type of comparison will take place,
    * and b) an interface with a comparison function could also be used. I went with the callback since
    * it was just interesting to learn about, and not something I've used in a while.
    */
    public static Reading Search(List<Reading> readings, Func<Reading, Reading, bool> compare, Reading target)
    {
        if (compare == null)
        {
            return null;
        }

        foreach (Reading r in readings)
        {
            if (compare(target, r) == true)
            {
                readings.Remove(r);
                return r;
            }
        }
    }

    public static void JSONifyResult(string filename, Dictionary<string, int> pairs)
    {
        var entries = pairs.Select
        (
            i => string.Format("{0}: {1}", i.Key, i.Value) 
        );
        string finalResult = "{\n" + string.Join(",\n", entries) + "\n}";
        File.WriteAllText(filename + ".json", finalResult);
    }

    public static void ReadCSV(string filename, List<Reading> buf)
    {
        string allReadings = File.ReadAllText(filename);
        List<string> splitReadings = new(allReadings.split("\n"));
        splitReadings.RemoveAt(0);

        foreach (string readingString in splitReadings)
        {
            string[] fields = readingString.Split(",");
            Reading reading = new()
            {
                Id = fields[0],
                Latitude = fields[1],
                Longitude = fields[2]
            };
            buf.Add(reading);
        }
    }

    public static void ReadJSON(string filename, List<Reading> buf)
    {
        string allReadings = File.ReadAllText(filename);
        allReadings = allReadings.Replace("[", "");
        allReadings = allReadings.Replace("]", "");
        string[] splitReadings = allReadings.Split(",");

        foreach (string readingString in splitReadings)
        {
            Reading reading = JsonSerializer.Deserialize<Reading>(readingString)!;
            buf.Add(reading);
        }
    }
}
