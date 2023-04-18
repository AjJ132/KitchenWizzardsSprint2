using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProServ.models;
using SQLite;

namespace ProServ.Database
{
    public class DBManager
    {
        private SQLiteAsyncConnection _connection;


        //if database is created then return true else return false
        public async Task<bool> InitDatabase()
        {
            try
            {
                // Specify the path for the database file
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ProServ.db");

                // Create the directory for the database file, if it doesn't already exist
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath));


                //Debug the db path
                Debug.WriteLine("DB Path: " + dbPath);

                //if there is no file this should automatically create it
                this._connection = new SQLiteAsyncConnection(dbPath);


                //Create all tables
                await _connection.CreateTableAsync<Employee>();
                await _connection.CreateTableAsync<LoginCredentials>();

                return true;
            }
            catch (Exception e) {
                Debug.WriteLine("There was an error in creating your database");
                Debug.WriteLine(e);
                return false;
            }

           
        }





        //Login functions
        public async Task<int> CheckUsername(string username)
        {
            var user = await _connection.Table<Employee>().Where(n => n.userName.Equals(username)).FirstOrDefaultAsync();
            if(user != null)
            {
                return user.Id;
            }
            else
            {
                //negative 1 is the status for no username found
                return -1;
            }

        }

        public async Task<bool> CheckPasswordMatch(string password, int userID)
        {
            //get credentials for user with matching ID
            var credentials = await this._connection.Table<LoginCredentials>().Where(n => n.userId ==  userID).FirstOrDefaultAsync();
            if(credentials != null) {
                if (credentials.password.Equals(password))
                {
                    //if passwords match then return true to start login process
                    return true;
                }
            }
            
            return false;

        }


        //Employee functions
        public async Task<bool> UpdateEmployee(Employee employee)
        {
            try
            {
                await _connection.UpdateAsync(employee);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in updating the employee");
                Debug.WriteLine(e);
                return false;
            }
        }
        public async Task<bool> InsertEmployee(Employee employee, LoginCredentials credentials)
        {
            try
            {
                await _connection.InsertAsync(employee);

                var emp = await GetEmployees();

                //Linq statement to filter through users and find the matching one. Then select userID and apply it to the credentials for proper referential integrity
                credentials.userId = emp.Where(n => n.firstName.Equals(employee.firstName) && n.lastName.Equals(employee.lastName)).Select(p => p.Id).FirstOrDefault();

                if(credentials.userId != null)
                {

                    //if employee insert is successful there needs to be a login for this employee inserted as well
                    bool credentialsUploaded = await InsertLoginCredentials(credentials);
                    if (!credentialsUploaded)
                    {
                        Debug.WriteLine("There was an error in uploading the employees credentials. Please contact system administrator");
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in inserting the employee");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await this._connection.Table<Employee>().ToListAsync();
        }

        public async Task<bool> DeleteEmployee(Employee employee)
        {
            try
            {
                await _connection.DeleteAsync(employee);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in deleting the employee");
                Debug.WriteLine(e);
                return false;
            }
        }

        //Get employee by employee id
        public async Task<Employee> GetEmployeeByID(int empID)
        {
            return await this._connection.Table<Employee>().Where(n => n.Id == empID).FirstOrDefaultAsync();
        }


        //login credential functions

        public async Task<bool> UpdateLoginCredentials(LoginCredentials loginCredentials)
        {
            try
            {
                await _connection.UpdateAsync(loginCredentials);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in updating the login credentials");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> InsertLoginCredentials(LoginCredentials loginCredentials)
        {
            try
            {
                await _connection.InsertAsync(loginCredentials);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in inserting the login credentials");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> DeleteLoginCredentials(LoginCredentials loginCredentials)
        {
            try
            {
                await _connection.DeleteAsync(loginCredentials);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in deleting the login credentials");
                Debug.WriteLine(e);
                return false;
            }
        }

       

    }
}
