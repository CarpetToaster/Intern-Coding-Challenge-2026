using System.Text.Json;
using System.Xml;

namespace Solution
{
    class Solution
    {
        public class Reading
        {
            public int id { get; set; }  
            public double latitude { get; set; }
            public double longitude { get; set; }
        }


        public static bool CompareReadings(Reading r1, Reading r2)
        {
            Console.WriteLine(r1.id + ", " + r2.id + ": " + Math.Abs(r1.latitude - r2.latitude) + ", " + Math.Abs(r1.longitude - r2.longitude));
            if (Math.Abs(r1.latitude - r2.latitude) < 0.0015 && Math.Abs(r1.longitude - r2.longitude) < 0.0015)
            {
                return true;
            }
            else
            {
                return false;
            }

        } 

        public static Reading? Search(List<Reading> readings, Reading target)
        {
            foreach (Reading r in readings)
            {
                if (CompareReadings(target, r))
                {
                    readings.Remove(r);
                    return r;
                }
            }
            return null;
        }

        public static void JSONifyResult(string filename, Dictionary<string, int> pairs)
        {
            var entries = pairs.Select
            (
                i => string.Format("\t\"{0}\": {1}", i.Key, i.Value) 
            );
            string finalResult = "{\n" + string.Join(",\n", entries) + "\n}";
            File.WriteAllText(filename, finalResult);
        }

        public static void ReadCSV(string filename, List<Reading> buf)
        {
            string allReadings = File.ReadAllText(filename);
            List<string> splitReadings = new(allReadings.Split("\n"));
            splitReadings.RemoveAt(0);

            foreach (string readingString in splitReadings)
            {
                string[] fields = readingString.Split(",");
                if (fields[0] == "")
                {
                    continue;
                }
                Reading reading = new()
                {
                    id = int.Parse(fields[0]),
                    latitude = double.Parse(fields[1]),
                    longitude = double.Parse(fields[2])
                };
                buf.Add(reading);
            }
        }

        public static void ReadJSON(string filename, List<Reading> buf)
        {
            string allReadings = File.ReadAllText(filename);
            allReadings = allReadings.Replace("[", "");
            allReadings = allReadings.Replace("]", "");
            string[] splitReadings = allReadings.Split("},", StringSplitOptions.RemoveEmptyEntries);

            foreach (string readingString in splitReadings)
            {
                string compare = readingString.Trim();
                if (compare[^1] != '}')
                {
                    compare += "}";
                }
                Reading reading = JsonSerializer.Deserialize<Reading>(compare)!;
                buf.Add(reading);
            }
        }

        public static void Main()
        {
            string[] filenames =
            [
                "../../../../SensorData1.csv",
                "../../../../SensorData2.json",
                "../../../../Output.json"
            ];

            Dictionary<string, int> pairs = [];

            List<Reading> sensor1Data = [];
            List<Reading> sensor2Data = [];

            ReadCSV(filenames[0], sensor1Data);
            ReadJSON(filenames[1], sensor2Data);

            foreach (Reading r in sensor1Data)
            {
                Reading? match = Search(sensor2Data, r);
                if (match == null)
                {
                    continue;
                }
                pairs.Add(r.id.ToString(), match.id);
            }

            JSONifyResult(filenames[2], pairs);
        }
    }
}