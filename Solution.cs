using System;
using System.Collections.Generic;

namespace Solution
{
    class Solution
    {
        class Reading(int id, int latitude, int longitude)
        {
            int id = id;
            int latitude = latitude;
            int longitude = longitude;

            public int Id
            {
                get {return id;}
                set {id = value;}
            }

            public int Latitude
            {
                get {return latitude;}
                set {latitude = value;}
            }
            
            public int Longitude
            {
                get {return longitude;}
                set {longitude = value;}
            }
        }

        protected bool CompareReadings(Reading r1, Reading r2)
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
        protected Reading Search(List<Reading> readings, Func<Reading, Reading, bool> compare, Reading target)
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

        protected void JSONifyResult()
        {
            
        }
    }
}