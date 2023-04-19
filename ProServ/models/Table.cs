using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProServ.models
{
    public class Table : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int tableId { get; set; } = 101;
        public int seatsAvaliable { get; set; }
        public int seatsTaken { get; set; }
        public int zoneID { get; set; }

        //0 = ready 1 = busy 2 = dirty
        [Ignore]
        private int _tableStatus { get; set; }
        public int tableStatus
        {
            get { return _tableStatus; }
            set
            {
                if (_tableStatus != value)
                {
                    _tableStatus = value;
                    OnPropertyChanged(nameof(tableStatus));
                }
            }
        }

        public int rowID { get; set; }
        public int columnID { get; set; }

        [Ignore]
        public CustomerTab currentTab { get; private set; }

        public Table()
        {

        }

       public Table(int tableId, int seatsAvaliable, int seatsTaken, int zoneID, int tableStatus, int rowID, int columnID)
        {
            this.tableId = tableId;
            this.seatsAvaliable = seatsAvaliable;
            this.seatsTaken = seatsTaken;
            this.zoneID = zoneID;
            this.tableStatus = tableStatus;
            this.rowID = rowID;
            this.columnID = columnID;
        }

        public Table(int seatsAvaliable, int seatsTaken, int zoneID, int tableStatus, int rowID, int columnID)
        {
            this.tableId = tableId;
            this.seatsAvaliable = seatsAvaliable;
            this.seatsTaken = seatsTaken;
            this.zoneID = zoneID;
            this.tableStatus = tableStatus;
            this.rowID = rowID;
            this.columnID = columnID;
        }

        public void AssignZoneID(int id)
        {
            this.zoneID = id;
        }

        public void SetCustomerTab(CustomerTab tab)
        {
            this.currentTab = tab;
        }





        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
