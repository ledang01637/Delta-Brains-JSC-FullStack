using DeltaBrainsJSCAppFE.Models.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Net.Http.Json;
using DeltaBrainsJSCAppFE.Handel;
using System.Diagnostics;
using DeltaBrainsJSCAppFE.Views;
using System.Windows.Markup;

namespace DeltaBrainsJSCAppFE.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {
        private static readonly HttpClient _httpClient = new();

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TaskItemViewModel> _listTask;

        public ObservableCollection<TaskItemViewModel> ListTask
        {
            get => _listTask;
            set
            {
                _listTask = value;
                OnPropertyChanged(nameof(ListTask));
            }
        }

        public ICommand EmployeeCommand { get; set; }


        public EmployeeViewModel()
        {
            EmployeeCommand = new RelayCommand<Window>((p) => { return true; }, (p) => Init());
        }

        private async void Init()
        {
            await GetTasks();
        }

        public async Task GetTasks()
        {
            try
            {
                IsLoading = true;

                var authLogin = AuthStorage.LoadToken();

                int userId;

                if (authLogin != null && !string.IsNullOrEmpty(authLogin.Token) && AuthStorage.IsTokenValid(authLogin))
                {
                    var _userId = GetFromToken.GetUserId(authLogin.Token);

                    if(!string.IsNullOrEmpty(_userId))
                    {
                        userId = int.Parse(_userId);
                    }
                    else
                    {
                        MessageBoxHelper.ShowError("Lỗi kiểm tra thông tin người dùng");
                        return;
                    }
                }
                else
                {
                    MessageBoxHelper.ShowError("Lỗi xác thực người dùng vui lòng dăng nhập lại!");

                    var loginWindow = new LoginWindow();
                    loginWindow.Show();

                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window != loginWindow)
                        {
                            window.Close();
                        }
                    }
                    return;
                }

                string url = "https://localhost:7089/api/Task/get-list";

                ListTask = new ObservableCollection<TaskItemViewModel>();

                var response = await _httpClient.PostAsJsonAsync(url, userId);

                if (response.IsSuccessStatusCode)
                {

                    var data = await response.Content.ReadFromJsonAsync<ApiResponse<ObservableCollection<TaskRes>>>();

                    foreach (var item in data.Data)
                    {
                        var vm = new TaskItemViewModel(item);
                        ListTask.Add(vm);
                    }
                }
                else
                {
                    MessageBoxHelper.ShowError("Lỗi server");
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError($"Lỗi không xác định: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
