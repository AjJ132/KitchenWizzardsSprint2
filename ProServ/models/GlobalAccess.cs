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

        

        public Employee currentEmployee { get; private set; }


        public GlobalAccess() {

            //create database
            this.dbManager = new DBManager();
            _ = StartDatabase();

           
           
            Debug.WriteLine("Created Global Access");
        
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


        public bool isLoggedIn()
        {
            if (this.currentEmployee == null)
            {
                return false;
            }
            else { return true; }
        }

        public void SetLogin(Employee newEmployee)
        {
            this.currentEmployee = newEmployee;

        }

        public void LogOut()
        {
            //setting the employee to null is bound to cause issues will have to do for now
            this.currentEmployee = null;
        }
    }
}