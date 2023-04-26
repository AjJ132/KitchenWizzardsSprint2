using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                await _connection.CreateTableAsync<Table>();
                await _connection.CreateTableAsync<Zone>();
                await _connection.CreateTableAsync<CustomerTab>();
                await _connection.CreateTableAsync<Item>();
                await _connection.CreateTableAsync<ItemCategory>();
                await _connection.CreateTableAsync<ForeignItem>();
                await _connection.CreateTableAsync<QueuedItem>();
                await _connection.CreateTableAsync<DBLog>();
                


                //this is for testing purposes. If the database is empty then insert test data;
                var emp = await GetEmployees();
                if(emp.Count == 0 || emp == null)
                {
                    CreateSQLObjects testObjects = new CreateSQLObjects();
                    testObjects.InsertStandardDatabaseObjects();
                }


                return true;
            }
            catch (Exception e) {
                Debug.WriteLine("There was an error in creating your database");
                Debug.WriteLine(e);
                return false;
            }

           
        }

        public SQLiteAsyncConnection GetConnection()
        {
            return this._connection;
        }

        public async void LogNow(string message)
        {
            string userName;
            try
            {
                

                if(GlobalAccess.globalAccess.currentEmployee == null)
                {
                    userName = "No User";
                }
                else
                { 
                    userName = GlobalAccess.globalAccess.currentEmployee.userName;
                }
            }
            catch(Exception e)
            {
                userName = "No User";
            }

            DBLog log = new DBLog(message, userName);
            await _connection.InsertAsync(log);
        }

        public async void InsertLog(DBLog log)
        {
            await _connection.InsertAsync(log);
        }   

        public async Task<List<DBLog>> GetLogs()
        {
            return await _connection.Table<DBLog>().ToListAsync();
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
                LogNow("Updated employee record");
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

                LogNow("Inserted Employee");
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

        public async Task<Employee> GetEmployeeByPin(int pin)
        {
            return await this._connection.Table<Employee>().Where(n => n.pin == pin).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteEmployee(Employee employee)
        {
            try
            {
                await _connection.DeleteAsync(employee);

                LogNow("Deleted Employee");
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

        public async Task<LoginCredentials> GetLoginCredentialsByUserID(int userID)
        {
            return await this._connection.Table<LoginCredentials>().Where(n => n.userId == userID).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateLoginCredentials(LoginCredentials loginCredentials)
        {
            try
            {
                await _connection.UpdateAsync(loginCredentials);
                LogNow($"Updated login credentials. ID: {loginCredentials.userId}");
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
                LogNow("Inserted Login Credentials");
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
                LogNow("Deleted Login Credentials");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in deleting the login credentials");
                Debug.WriteLine(e);
                return false;
            }
        }

        //Zones

        public async Task<List<Zone>> GetZones()
        {
            return await this._connection.Table<Zone>().ToListAsync();
        }

        public async Task<Zone> GetZoneByID(int zoneID)
        {
            return await this._connection.Table<Zone>().Where(n => n.zoneID == zoneID).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertZone(Zone zone)
        {
            try
            {
                await _connection.InsertAsync(zone);
                LogNow($"Updated zone: {zone.zoneID}");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in inserting the zone");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> UpdateZone(Zone zone)
        {
            try
            {
                await _connection.UpdateAsync(zone);
                LogNow("Updated zone");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in updating the zone");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> DeleteZone(Zone zone)
        {
            try
            {
                await _connection.DeleteAsync(zone);
                LogNow("Deleted Zone");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in deleting the zone");
                Debug.WriteLine(e);
                return false;
            }
        }

        //Tables

        public async Task<List<Table>> GetTables()
        {
            return await this._connection.Table<Table>().ToListAsync();
        }

        public async Task<Table> GetTableByID(int tableID)
        {
            return await this._connection.Table<Table>().Where(n => n.tableId == tableID).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertTable(Table table)
        {
            try
            {
                await _connection.InsertAsync(table);
                LogNow("Inserted Table");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in inserting the table");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> UpdateTable(Table table)
        {
            try
            {
                await _connection.UpdateAsync(table);
                LogNow("Updated Table");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in updating the table");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> DeleteTable(Table table)
        {
            try
            {
                await _connection.DeleteAsync(table);
                LogNow("Deleted Table");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in deleting the table");
                Debug.WriteLine(e);
                return false;
            }
        }


        //Tabs get, insert, update, delete, get by id
        public async Task<List<CustomerTab>> GetCustomerTabs()
        {
            return await this._connection.Table<CustomerTab>().ToListAsync();
        }
        
        public async Task<CustomerTab> GetCustomerTabByID(int tabId)
        {
            return await this._connection.Table<CustomerTab>().Where(n => n.tabId == tabId).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertCustomerTab(CustomerTab tab)
        {
            try
            {
                await _connection.InsertAsync(tab);
                LogNow("Inserted Customer Tab");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in inserting the tab");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> UpdateCustomerTab(CustomerTab tab)
        {
            try
            {
                await _connection.UpdateAsync(tab);
                LogNow("Updated Customer Tab");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in updating the tab");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> DeleteCustomerTab(CustomerTab tab)
        {
            try
            {
                await _connection.DeleteAsync(tab);
                LogNow("Deleted Customer Tab");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in deleting the tab");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<int> GetRunningTabId()
        {
            var tab = await this._connection.Table<CustomerTab>().OrderByDescending(n => n.tabId).FirstOrDefaultAsync();
            if(tab == null)
            {
                return -1;
            }
            return tab.tabId;

        }

        public async Task<CustomerTab> GetOpenTabByTableId(int tableId)
        {
            var tab = await this._connection.Table<CustomerTab>().OrderByDescending(n => n.tabId).Where(n => n.tableId == tableId).FirstOrDefaultAsync();
            if(tab == null)
            {
                return new CustomerTab();
                
            }
            return tab;
        }

        //Items

        public async Task<List<Item>> GetItems()
        {
            return await this._connection.Table<Item>().ToListAsync();
        }

        public async Task<Item> GetItemByID(int itemId)
        {
            return await this._connection.Table<Item>().Where(n => n.itemId == itemId).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertItem(Item item)
        {
            try
            {
                await _connection.InsertAsync(item);

                LogNow("Inserted Food Item");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in inserting the item");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> UpdateItem(Item item)
        {
            try
            {
                await _connection.UpdateAsync(item);
                LogNow("Updated food item");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in updating the item");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> DeleteItem(Item item)
        {
            try
            {
                await _connection.DeleteAsync(item);
                LogNow("Deleted Item");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in deleting the item");
                Debug.WriteLine(e);
                return false;
            }
        }


        //Categories

        public async Task<List<ItemCategory>> GetItemCategories()
        {
            return await this._connection.Table<ItemCategory>().ToListAsync();
        }

        public async Task<ItemCategory> GetItemCategoryByID(int categoryId)
        {
            return await this._connection.Table<ItemCategory>().Where(n => n.categoryId == categoryId).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertItemCategory(ItemCategory category)
        {
            try
            {
                await _connection.InsertAsync(category);
                LogNow("Insert Item Category");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in inserting the category");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> UpdateItemCategory(ItemCategory category)
        {
            try
            {
                await _connection.UpdateAsync(category);
                LogNow("Updated Item Category");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in updating the category");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> DeleteItemCategory(ItemCategory category)
        {
            try
            {
                await _connection.DeleteAsync(category);
                LogNow("Deleted Item Category");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in deleting the category");
                Debug.WriteLine(e);
                return false;
            }
        }

        //ForeginItems

        //this is in charge of inserting an item into the customer tab and writing that record to a
        //datatbase. This is a foreign key relationship

        public async Task<List<ForeignItem>> GetForeignItems()
        {
            return await this._connection.Table<ForeignItem>().ToListAsync();
        }

        public async Task <List<ForeignItem>> GetForeignItemByTabId(int tabId)
        {
            return await this._connection.Table<ForeignItem>().Where(n => n.customerTabId == tabId).ToListAsync();
        }

        public async Task<ForeignItem> GetForeignItemByTabAndItemId(int tabId, int itemdId)
        {
            return await this._connection.Table<ForeignItem>().Where(n => n.customerTabId == tabId).Where(p => p.itemId == itemdId).FirstOrDefaultAsync();
        }

        

        public async Task<bool> InsertForeignItem(ForeignItem foreignItem)
        {
            try
            {
                await _connection.InsertAsync(foreignItem);
                LogNow("Inserted Item To ForeignItem Table");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in inserting the foreign item");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> UpdateForeignItem(ForeignItem foreignItem)
        {
            try
            {
                await _connection.UpdateAsync(foreignItem);
                LogNow("Updated Foreign Item");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in updating the foreign item");
                Debug.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> DeleteForeignItem(ForeignItem foreignItem)
        {
            try
            {
                var variable = await _connection.DeleteAsync(foreignItem);
                LogNow("Deleted Foreign Item");
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("There was an error in deleting the foreign item");
                Debug.WriteLine(e);
                return false;
            }
        }



    }
}
