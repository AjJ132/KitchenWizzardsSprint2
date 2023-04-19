using System;
using System.Collections.Generic;
using System.Linq;
using ProServ.Database;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProServ.models
{
    //class used to sore globaly accessible data such as login status
    //This uses the singleton pattern
    public class GlobalAccess
    {
        //if no globalAccess then create new one else return static variable
        public static GlobalAccess globalAccess { get; private set; }

        public DBManager dbManager { get; private set; } 

        private List<Zone> zones { get; set; }

        private List<Table> tables { get; set; }

        public int runningTabId = 0;

        

        public Employee currentEmployee { get; private set; }


        public GlobalAccess() {
            Debug.WriteLine("Started Global Access");

            InitSequence();

            Debug.WriteLine("Created Global Access");
        
        }

        public async Task InitSequence()
        {

            Debug.WriteLine("Started Init Sequence");

            //create database
            this.dbManager = new DBManager();
            await StartDatabase();

            this.runningTabId = await this.dbManager.GetRunningTabId();
            if(runningTabId == -1)
            {
                runningTabId = 1;
            }

            await ImportZones();

            await ImportTables();


            Debug.WriteLine("Finished Init Sequence");
        }

        public async Task StartDatabase()
        {
            bool success = await dbManager.InitDatabase();
            if (!success)
            {
                //create logic to exit application if database isnt created
                Debug.WriteLine("Something went wrong with the database");
            }
            return;
        }

        public static void CreateGlobalAccess()
        {
            if (globalAccess == null)
            {
                globalAccess = new GlobalAccess();
            }
            else return;
        }


        //import zones from database
        private async Task ImportZones()
        {
            this.zones = new List<Zone>();
            this.zones = await this.dbManager.GetZones();
        }

        public List<Zone> GetZones() { return this.zones; }

        private async Task ImportTables()
        {
            this.tables = new List<Table>();
            this.tables = await this.dbManager.GetTables();
        }

        public List<Table> GetTables()
        {
            return this.tables;
        }

        public string GetZoneHexByID(int id)
        {
            return this.zones.Where(n => n.zoneID == id).Select(p => p.zoneHexColor).FirstOrDefault();
        }


        public bool isLoggedIn()
        {
            if (this.currentEmployee == null)
            {
                return false;
            }
            else { return true; }
        }

        public void LogIn(Employee newEmployee)
        {
            //Sets the current emlployee
            this.currentEmployee = newEmployee;

        }

        public void LogOut()
        {
            //setting the employee to null is bound to cause issues will have to do for now
            this.currentEmployee = null;
        }
    }
}