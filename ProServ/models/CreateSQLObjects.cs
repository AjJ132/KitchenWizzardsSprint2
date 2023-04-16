using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProServ.models
{
    public class CreateSQLObjects
    {
        public List<Employee> employees { get; set; }
        public List<LoginCredentials> loginCredentials { get; set; }

        //this is used to insert some standard empployees and passwords into the database
        public CreateSQLObjects()
        {
            this.employees = new List<Employee>();
            this.loginCredentials = new List<LoginCredentials>();

            this.employees.Add(new Employee("AJ", "Johnson", "Chef", "aj132"));
            this.loginCredentials.Add(new LoginCredentials("aj132_password"));

            this.employees.Add(new Employee("John", "Smith", "Waiter", "johns42"));
            this.loginCredentials.Add(new LoginCredentials("johns42_password"));

            this.employees.Add(new Employee("Jane", "Doe", "Busboy", "janed87"));
            this.loginCredentials.Add(new LoginCredentials("janed87_password"));


        }

        public async void InsertStandardDatabaseObjects()
        {
            int index = 0;
            foreach (var employee in employees)
            {
                await GlobalAccess.globalAccess.dbManager.InsertEmployee(employee, this.loginCredentials[index]);


                index++;
            }
        }

    }
}
