using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ProServ.models;
using ProServ.Views;

namespace ProServ
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
      
        protected override void OnStartup(StartupEventArgs e)
        {
            

            GlobalAccess.CreateGlobalAccess();


            //only use this to populate the database with some standard employees
            //CreateSQLObjects testObjects = new CreateSQLObjects();
            //testObjects.InsertStandardDatabaseObjects();

        
        }
    }
}
