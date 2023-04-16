using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;


namespace Sprint2.models
{
    //class used to sore globaly accessible data such as login status
    //This uses the singleton pattern
    public class GlobalAccess
    {
        //if no globalAccess then create new one else return static variable
        public static GlobalAccess globalAccess
        {
            get
            {
                if(globalAccess == null)
                {
                    return new GlobalAccess();
                }
                else { return globalAccess; }
            }
            set { globalAccess = value; }
        }

        public bool isLoggedIn = false;

        public Employee currentEmployee { get; private set; }


        public GlobalAccess() { 
            
        
        }
    }
}