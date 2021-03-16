using System;
using System.Collections.Generic;
using System.Text;

namespace seniorCapstone.Tables
{
	class AmbeeSoilTable
	{
        public class SoilCurrent
        {
            public string id { get; set; }
            public string scanTime { get; set; }
            public int soilTemp { get; set; }
            public int soilMoisture { get; set; }
        }
    }
}
