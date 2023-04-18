﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.DocumentStructures;

namespace ProServ.models
{
    public class CreateSQLObjects
    {
        public List<Employee> employees { get; set; }
        public List<LoginCredentials> loginCredentials { get; set; }

        public List<Zone> zones { get; set; }
        public List<Table> tables { get; set; }

        

        //this is used to insert some standard empployees and passwords into the database
        public CreateSQLObjects()
        {
            this.employees = new List<Employee>();
            this.loginCredentials = new List<LoginCredentials>();
            this.zones = new List<Zone>();
            this.tables = new List<Table>();

            this.employees.Add(new Employee("AJ", "Johnson", "Chef", "aj132"));
            this.loginCredentials.Add(new LoginCredentials("aj132_password"));

            this.employees.Add(new Employee("John", "Smith", "Waiter", "johns42"));
            this.loginCredentials.Add(new LoginCredentials("johns42_password"));

            this.employees.Add(new Employee("Jane", "Doe", "Busboy", "janed87"));
            this.loginCredentials.Add(new LoginCredentials("janed87_password"));

            this.zones.Add(new Zone(101, "#FF5733"));
            this.zones.Add(new Zone(102, "#01D1FB"));
            this.zones.Add(new Zone(103, "#99FA77"));

            CreateTables();
        }

        public void CreateTables()
        {
            //left section tables
            this.tables.Add(new Table(4, 0, 101, 0, 0, 0));
            this.tables.Add(new Table(4, 0, 101, 0, 0, 1));
            this.tables.Add(new Table(4, 0, 101, 0, 1, 0));
            this.tables.Add(new Table(4, 0, 101, 0, 1, 1));
            this.tables.Add(new Table(4, 0, 101, 0, 2, 0));
            this.tables.Add(new Table(4, 0, 102, 0, 2, 1));
            this.tables.Add(new Table(4, 0, 101, 0, 3, 0));
            this.tables.Add(new Table(4, 0, 101, 0, 3, 1));
            this.tables.Add(new Table(4, 0, 101, 0, 4, 0));
            this.tables.Add(new Table(4, 0, 101, 0, 4, 1));
            this.tables.Add(new Table(4, 0, 101, 0, 5, 0));
            this.tables.Add(new Table(4, 0, 101, 0, 5, 1));

            //middle section below the bar
            this.tables.Add(new Table(4, 0, 102, 0, 4, 2));
            this.tables.Add(new Table(4, 0, 102, 0, 4, 3));
            this.tables.Add(new Table(4, 0, 102, 0, 5, 2));
            this.tables.Add(new Table(4, 0, 102, 0, 5, 3));

            //right section tables
            this.tables.Add(new Table(4, 0, 103, 0, 0, 4));
            this.tables.Add(new Table(4, 0, 103, 0, 0, 5));
            this.tables.Add(new Table(4, 0, 103, 0, 1, 4));
            this.tables.Add(new Table(4, 0, 103, 0, 1, 5));
            this.tables.Add(new Table(4, 0, 103, 0, 2, 4));
            this.tables.Add(new Table(4, 0, 103, 0, 2, 5));
            this.tables.Add(new Table(4, 0, 103, 0, 3, 4));
            this.tables.Add(new Table(4, 0, 103, 0, 3, 5));
            this.tables.Add(new Table(4, 0, 103, 0, 4, 4));
            this.tables.Add(new Table(4, 0, 103, 0, 4, 5));
            this.tables.Add(new Table(4, 0, 103, 0, 5, 4));
            this.tables.Add(new Table(4, 0, 103, 0, 5, 5));


        }

        public async void InsertStandardDatabaseObjects()
        {
            int index = 0;
            foreach (var employee in employees)
            {
                await GlobalAccess.globalAccess.dbManager.InsertEmployee(employee, this.loginCredentials[index]);


                index++;
            }

            foreach (var zone in zones)
            {
                await GlobalAccess.globalAccess.dbManager.InsertZone(zone);
            }

            foreach(var table in tables)
            {
                await GlobalAccess.globalAccess.dbManager.InsertTable(table);
            }
        }

    }
}
