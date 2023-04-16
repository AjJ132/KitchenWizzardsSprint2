using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using SQLite;
using System.Diagnostics.CodeAnalysis;

namespace ProServ.models
{
    public class Employee
    {
        [PrimaryKey, AutoIncrement,]
        public int Id { get; set; } = 1001;

        [DisallowNull]
        public string firstName { get; set; }
        [DisallowNull]
        public string lastName { get; set; }

        //going to do a string for now but this may change in the future and be a class using polymorphism and extending
        public string employeeType { get; set; }

        [DisallowNull]
        public string userName { get; set; }


        public Employee()
        {

        }


        //this will be responsible for instatiating an employee by their id and a database search
        public Employee(string fname, string lname, string employeeType, string username)
        {
            this.firstName = fname;
            this.lastName = lname;
            this.employeeType = employeeType;
            this.userName = username;

        }
    }
}