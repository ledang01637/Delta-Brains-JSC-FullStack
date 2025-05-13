using DeltaBrainsJSCAppFE.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using DeltaBrainsJSCAppFE.Models.Response;
using System.Net.Http;
using static System.Net.WebRequestMethods;
using System.Net.Http.Json;
using System.ComponentModel;

namespace DeltaBrainsJSCAppFE.ViewModels
{
    public class ManagerViewModel : BaseViewModel
    {
        private static readonly HttpClient _httpClient = new();

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private List<TaskRes> _listTask;

        public List<TaskRes> ListTask
        {
            get => _listTask;
            set
            {
                _listTask = value;
                OnPropertyChanged(nameof(ListTask));
            }
        }

        public ICommand ManagerCommand { get; set; }


        public ManagerViewModel()
        {
            _ = GetTasks();
        }

        public async Task GetTasks()
        {
            try
            {
                IsLoading = true;

                string url = "https://localhost:7089/api/Task/get-list";

                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<TaskRes>>>(url);

                if (response != null && response.Code == 200)
                {
                    ListTask = response.Data;
                }
                else
                {
                    MessageBox.Show("Lỗi: " + response?.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách công việc: " + ex.Message);
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
