using System;
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

        public List<Item> items { get; set; }

        

        //this is used to insert some standard empployees and passwords into the database
        public CreateSQLObjects()
        {
            this.employees = new List<Employee>();
            this.loginCredentials = new List<LoginCredentials>();
            this.zones = new List<Zone>();
            this.tables = new List<Table>();
            this.items = new List<Item>();

            this.employees.Add(new Employee("AJ", "Johnson", "Chef", 2848 ,"aj132"));
            this.loginCredentials.Add(new LoginCredentials("aj132_password"));

            this.employees.Add(new Employee("John", "Smith", "Waiter",1028 ,"johns42"));
            this.loginCredentials.Add(new LoginCredentials("johns42_password"));

            this.employees.Add(new Employee("Jane", "Doe", "Busboy", 4096 ,"janed87"));
            this.loginCredentials.Add(new LoginCredentials("janed87_password"));

            this.employees.Add(new Employee("Joe", "Johnson", "Manager", 8965, "jj2"));
            this.loginCredentials.Add(new LoginCredentials("jj2ps"));

            this.zones.Add(new Zone(101, "#FF5733"));
            this.zones.Add(new Zone(102, "#01D1FB"));
            this.zones.Add(new Zone(103, "#99FA77"));

            //Food items
            this.items.Add(new Item("J Burger", "Burgers", 6.99));
            this.items.Add(new Item("Bacon Cheeseburger", "Burgers", 7.99));
            this.items.Add(new Item("Carolina Burger", "Burgers", 7.99));
            this.items.Add(new Item("Chicken Wrap", "Wraps", 6.49));
            this.items.Add(new Item("Club Sandwich", "Sandwiches", 7.49));
            this.items.Add(new Item("Garlic Chicken Pasta", "Entrees", 10.99));
            this.items.Add(new Item("Steak", "Entrees", 18.99));
            this.items.Add(new Item("Buffalo Wings", "Appetizers", 8.99));
            this.items.Add(new Item("Mozzarella Sticks", "Appetizers", 6.99));
            this.items.Add(new Item("Italian Salad", "Salads", 7.99));
            this.items.Add(new Item("Caesar Salad", "Salads", 6.99));



            CreateTables();
        }

        public void CreateTables()
        {
            //left section tables
            this.tables.Add(new Table(4, 0, 1, 0, 0, 0));
            this.tables.Add(new Table(4, 0, 1, 0, 0, 1));
            this.tables.Add(new Table(4, 0, 1, 0, 1, 0));
            this.tables.Add(new Table(4, 0, 1, 0, 1, 1));
            this.tables.Add(new Table(4, 0, 1, 0, 2, 0));
            this.tables.Add(new Table(4, 0, 1, 0, 2, 1));
            this.tables.Add(new Table(4, 0, 1, 0, 3, 0));
            this.tables.Add(new Table(4, 0, 1, 0, 3, 1));
            this.tables.Add(new Table(4, 0, 1, 0, 4, 0));
            this.tables.Add(new Table(4, 0, 1, 0, 4, 1));
            this.tables.Add(new Table(4, 0, 1, 0, 5, 0));
            this.tables.Add(new Table(4, 0, 1, 0, 5, 1));

            //middle section below the bar
            this.tables.Add(new Table(4, 0, 2, 0, 4, 2));
            this.tables.Add(new Table(4, 0, 2, 0, 4, 3));
            this.tables.Add(new Table(4, 0, 2, 0, 5, 2));
            this.tables.Add(new Table(4, 0, 2, 0, 5, 3));

            //right section tables
            this.tables.Add(new Table(4, 0, 3, 0, 0, 4));
            this.tables.Add(new Table(4, 0, 3, 0, 0, 5));
            this.tables.Add(new Table(4, 0, 3, 0, 1, 4));
            this.tables.Add(new Table(4, 0, 3, 0, 1, 5));
            this.tables.Add(new Table(4, 0, 3, 0, 2, 4));
            this.tables.Add(new Table(4, 0, 3, 0, 2, 5));
            this.tables.Add(new Table(4, 0, 3, 0, 3, 4));
            this.tables.Add(new Table(4, 0, 3, 0, 3, 5));
            this.tables.Add(new Table(4, 0, 3, 0, 4, 4));
            this.tables.Add(new Table(4, 0, 3, 0, 4, 5));
            this.tables.Add(new Table(4, 0, 3, 0, 5, 4));
            this.tables.Add(new Table(4, 0, 3, 0, 5, 5));


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

            foreach(var item in items)
            {
                await GlobalAccess.globalAccess.dbManager.InsertItem(item);
            }
        }

    }
}
