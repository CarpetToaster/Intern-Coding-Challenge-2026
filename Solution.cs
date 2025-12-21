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
        * Remember to use Delegate(args) {compareReadings(args);} as the argument for comparator
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
                    return r;
                }
            }
        }
    }
}