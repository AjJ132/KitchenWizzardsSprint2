using ProServ.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProServ.Views
{
    public partial class ManagerView : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        private ObservableCollection<LoginCredentials> _credentials;
        public ObservableCollection<LoginCredentials> Credentials
        {
            get => _credentials;
            set
            {
                _credentials = value;
                OnPropertyChanged(nameof(Credentials));
            }
        }

        private ObservableCollection<DBLog> _logs;
        public ObservableCollection<DBLog> Logs
        {
            get => _logs;
            set
            {
                _logs = value;
                OnPropertyChanged(nameof(Logs));
            }
        }

        private ObservableCollection<Zone> _zones;
        public ObservableCollection<Zone> Zones
        {
            get => _zones;
            set
            {
                _zones = value;
                OnPropertyChanged(nameof(Zones));
            }
        }

        private ObservableCollection<Table> _tables;
        public ObservableCollection<Table> Tables
        {
            get => _tables;
            set
            {
                _tables = value;
                OnPropertyChanged(nameof(Tables));
            }
        }



        public bool newEmployeeCreated = false;

        public bool newZoneCreated = false;



        private string _employeeActivityText;
        public string EmployeeActivityText
        {
            get => _employeeActivityText;
            set
            {
                _employeeActivityText = value;
                OnPropertyChanged(nameof(EmployeeActivityText));
            }
        }





        public ManagerView()
        {
            

            _ = ConfigureEmployees();
            _ = ConfigureLogs();
            _ = ConfigureZonesAndTables();

            
            DataContext = this;
            InitializeComponent(); 





            
        }

        public async Task<Task> ConfigureEmployees()
        {
            List<Employee> emp = await GlobalAccess.globalAccess.dbManager.GetEmployees();

            this.Employees = new ObservableCollection<Employee>(emp);

            this.Credentials = new ObservableCollection<LoginCredentials>();

            foreach(var i in emp)
            {
                var cred = await GlobalAccess.globalAccess.dbManager.GetLoginCredentialsByUserID(i.Id);
                Credentials.Add(cred);
            }


            return Task.CompletedTask;
        }

        public async Task<Task> ConfigureLogs()
        {

            List<DBLog> l = await GlobalAccess.globalAccess.dbManager.GetLogs();
            this.Logs = new ObservableCollection<DBLog>(l);
            return Task.CompletedTask;
        }

        public async Task<Task> ConfigureZonesAndTables()
        {
            List<Zone> z = await GlobalAccess.globalAccess.dbManager.GetZones();
            this.Zones = new ObservableCollection<Zone>(z);

            List<Table> t = await GlobalAccess.globalAccess.dbManager.GetTables();
            this.Tables = new ObservableCollection<Table>(t);

            

            return Task.CompletedTask;
        }

        private async void SaveChanges(object parameter)
        {
            foreach(var i in Employees)
            {
                await GlobalAccess.globalAccess.dbManager.UpdateEmployee(i);
            }
            foreach(var i in Credentials)
            {
                await GlobalAccess.globalAccess.dbManager.UpdateLoginCredentials(i);
            }
        }

        private void NewEmployee(object parameter)
        {
            // Implement logic to create a new employee
            // Update EmployeeActivityText with the activity log
        }

        private void SaveNewEmployee(Employee newEmployee)
        {
            //Logic to save new employees to database
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox editingTextBox = sender as TextBox;
            if (editingTextBox != null)
            {
                string newPassword = editingTextBox.Text;
                DataGridRow dataGridRow = FindVisualParent<DataGridRow>(editingTextBox);
                if (dataGridRow != null)
                {
                    Employee employee = (Employee)dataGridRow.Item;
                    int employeeId = employee.Id; // Replace 'EmployeeId' with the correct property name in your Employee class

                    // Find the corresponding LoginCredentials object and update the password
                    LoginCredentials matchingCredential = Credentials.FirstOrDefault(x => x.userId == employeeId);
                    if (matchingCredential != null)
                    {
                        matchingCredential.password = newPassword;
                    }
                }
            }
        }


        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindVisualParent<T>(parentObject);
            }
        }

        private async void SaveEmployeeChanges(object sender, RoutedEventArgs e)
        {
            if (newEmployeeCreated)
            {
                Employee emp = this.Employees[Employees.Count() - 1];
                LoginCredentials cred = this.Credentials[Credentials.Count() - 1];

                if (ValidateEmployee(emp, cred))
                {

                    //insert Employee to database
                    MessageBox.Show("New employee created!");
                    this.newEmployeeCreated = false;
                }
            }
            else
            {
                foreach (var i in Employees)
                {
                    await GlobalAccess.globalAccess.dbManager.UpdateEmployee(i);
                }
                foreach (var i in Credentials)
                {
                    await GlobalAccess.globalAccess.dbManager.UpdateLoginCredentials(i);
                }
            }
        }

        private bool ValidateEmployee(Employee emp, LoginCredentials cred)
        {
            if (cred == null || emp == null)
            {
                return false;
            }

            if (emp.firstName.Equals("Default") || emp.firstName.Equals(""))
            {
                MessageBox.Show("Firstname cannot be left as default or empty");
                return false;
            }

            if (emp.lastName.Equals("Default") || emp.lastName.Equals(""))
            {
                MessageBox.Show("Lastname cannot be left as default or empty");
                return false;
            }

            if (!emp.employeeType.Equals("Waiter") && !emp.employeeType.Equals("Chef") && !emp.employeeType.Equals("Busboy") && !emp.employeeType.Equals("Manager"))
            {
                MessageBox.Show("Please pick from these options on Employee Type: Waiter, Chef, Busboy, or Manager");
                return false;
            }

            if (emp.pin == 1234 || emp.pin == null || emp.pin.ToString().Length != 4)
            {
                MessageBox.Show("Pin cannot be 1234, empty, and must be 4 digits.");
                return false;
            }

            if (!IsUniquePin(emp.pin) || !IsUniqueUserName(emp.userName))
            {
                return false;
            }

            if(cred.password.Equals("") || cred.password.Equals("Default") || cred.password == null || cred.password.Length < 6)
            {
                MessageBox.Show("Password cannot be empty, Default, or have a length less than 6 characters");
                return false;
            }

            return true;
        }

        private bool IsUniquePin(int pin)
        {
            foreach (var i in Employees)
            {
                if (i.pin == pin && i.Id != 0)
                {
                    MessageBox.Show("Employees cannot have the same pin number.");
                    return false;
                }
            }
            return true;
        }

        private bool IsUniqueUserName(string userName)
        {
            if (!userName.Equals("") && !userName.Equals("Default") && userName != null)
            {
                foreach (var i in Employees)
                {
                    if (i.userName.Equals(userName) && i.Id != 0)
                    {
                        MessageBox.Show("Employees cannot have the same user name.");
                        return false;
                    }
                }
                return true;
            }
            else
            {
                MessageBox.Show("Please make sure the username isn't empty or equal to Default");
                return false;
            }
        }

        private void NewEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (!newEmployeeCreated)
            {
                Employee newEmployee = new Employee(true);
                LoginCredentials newCredentials = new LoginCredentials(true);

                this.Employees.Add(newEmployee);
                this.Credentials.Add(newCredentials);

                this.newEmployeeCreated = true;
            }
            else
            {
                MessageBox.Show("Please save the new employee before creating another one");
            }
        }

        private void NewZone_Click(object sender, RoutedEventArgs e)
        {
            if(!newZoneCreated)
            {
                Zone newZone = new Zone(true);

                this.Zones.Add(newZone);

                this.newZoneCreated = true;
            }
            else
            {
                MessageBox.Show("Please save the new employee before creating another one");
            }
        }

        private async void SaveZoneChanges(object sender, RoutedEventArgs e)
        {
            if (newZoneCreated)
            {
                Zone zone = this.Zones[Zones.Count() - 1];

                if (ValidateZone(zone))
                {

                    //insert zone to database
                    MessageBox.Show("New zone created!");
                    await GlobalAccess.globalAccess.dbManager.InsertZone(zone);
                    this.newZoneCreated = false;
                }
            }
            else
            {
               
                foreach (var i in Zones)
                {
                    if(ValidateZone(i))
                    {
                        await GlobalAccess.globalAccess.dbManager.UpdateZone(i);
                    }
                }

                var lastZone = await GlobalAccess.globalAccess.dbManager.GetZones();
                this.Zones[Zones.Count() - 1].zoneID = lastZone[lastZone.Count() - 1].zoneID;
            }
        }

        private bool ValidateZone(Zone zone)
        {
            // Add a leading "#" if it is missing
            if (!zone.zoneHexColor.StartsWith("#"))
            {
                zone.zoneHexColor = "#" + zone.zoneHexColor;
            }

            // Validate the string as a hexadecimal color code
            Regex hexColorRegex = new Regex("^#?([a-fA-F0-9]{6}|[a-fA-F0-9]{8})$");
            bool isHex = hexColorRegex.IsMatch(zone.zoneHexColor);

            if(!isHex)
            {
                MessageBox.Show("please enter valid Hex code. Follow the link to a hex code website. https://htmlcolorcodes.com/color-picker/");
                return false; 
            }

            return true;
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}

