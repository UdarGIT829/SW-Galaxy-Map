using System;
using System.Collections.Generic;

namespace StarMap
{
    public class sSystemDetails
    {
        public string name { get; set;}
        public string description { get; set; }
        Tuple<int,int> coordinates;
        public string sectorName { get; set; }
        string regionName;
        string[] celestialBodies;
        string capital;

        public sSystemDetails()
        {
        }
        public sSystemDetails(string inName, Tuple<int,int> inCoords, string inSectorName)
        {
            name = inName;
            coordinates = inCoords;
            sectorName = inSectorName;
        }

        public sSystemDetails(string inName, Tuple<int,int> inCoords, string inSectorName, string inRegion)
        {
            name = inName;
            coordinates = inCoords;
            sectorName = inSectorName;
            regionName = inRegion;
        }
    }


}