using System.Text.Json;
using Iot.Device.Common;

// I'm taking this challenge as an opportuniy to somewhat familiarize myself with C#,
// following basic conventions and familiarizing myself with building the exectuable, most notably.

// I ran this through VSCode with the Solution.slnx file included in the parent directory of the repo clone.
// if there are any issues regarding line 2, please use dotnet add package Iot.Device.Bindings --version 4.0.1.

// this was written in .NET 10.0.1
namespace Solution
{
    class Solution
    {
        public class Reading
        {
            public int Id { get; set; }  
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public static bool CompareReadings(Reading r1, Reading r2)
        {
            GreatCircle.DistAndDir(r1.Latitude, r1.Longitude, r2.Latitude, r2.Longitude, out double dist, out _);
            if (Math.Abs(dist) < 100)
            {
                return true;
            }
            else
            {
                return false;
            }

        } 

        // using a dictionary or other hashmap here could be an optiomization.
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
                    Id = int.Parse(fields[0]),
                    Latitude = double.Parse(fields[1]),
                    Longitude = double.Parse(fields[2])
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

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            foreach (string readingString in splitReadings)
            {
                string compare = readingString.Trim();
                if (compare[^1] != '}')
                {
                    compare += "}";
                }
                Reading reading = JsonSerializer.Deserialize<Reading>(compare, options)!;
                buf.Add(reading);
            }
        }

        public static void Main()
        {
            string[] filenames =
            [
                "../../../../SensorData1.csv", // "../../../../SensorData1.csv",
                "../../../../SensorData2.json", // "../../../../SensorData2.json",
                "../../../../Output.json" //"../../../../Output.json"
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
                pairs.Add(r.Id.ToString(), match.Id);
            }

            JSONifyResult(filenames[2], pairs);
        }
    }
}