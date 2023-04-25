using ProServ.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
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

        public ICommand SaveChangesCommand { get; set; }
        public ICommand NewEmployeeCommand { get; set; }




        public ManagerView()
        {
            

            _ = ConfigureEmployees();

            
            DataContext = this;
            InitializeComponent();




            SaveChangesCommand = new RelayCommand(SaveChanges);
            NewEmployeeCommand = new RelayCommand(NewEmployee);

            
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

        private async void SaveChanges(object sender, RoutedEventArgs e)
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

