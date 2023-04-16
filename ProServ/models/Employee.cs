using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;



namespace Sprint2.models
{
    public class Employee
    {
        public int Id { get; }
        public string firstName { get; private set; }
        public string lastName { get; private set; }

        //going to do a string for now but this may change in the future and be a class using polymorphism and extending
        public string employeeType { get; private set; }


        public Employee()
        {

        }


        //this will be responsible for instatiating an employee by their id and a database search
        public Employee(int id)
        {

        }
    }
}