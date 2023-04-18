using SQLite;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProServ.models
{
    public class Zone
    {
        [PrimaryKey, AutoIncrement]
        public int zoneID { get; set; } = 101;

        public string zoneHexColor { get; set; }

        public Zone()
        {
        }

        public Zone(int id, string zoneHexColor)
        {
            this.zoneID = id;
            this.zoneHexColor = zoneHexColor;
        }
    }
}
