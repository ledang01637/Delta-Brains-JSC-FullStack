using DeltaBrainsJSCAppFE.Handel;
using DeltaBrainsJSCAppFE.Models.Request;
using DeltaBrainsJSCAppFE.Models.Response;
using DeltaBrainsJSCAppFE.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DeltaBrainsJSCAppFE.ViewModels
{
    public class TaskViewModel : BaseViewModel
    {
        private static readonly HttpClient _httpClient = new();

        public event EventHandler TaskSaved;
        public TaskReq Request { get; set; } = new TaskReq();
        public TaskUpdate UpdateRequest { get; set; }
        public object CurrentTask => IsEditMode ? (object)UpdateRequest : Request;

        public bool IsEditMode { get; set; }

        public ObservableCollection<UserRes> Users { get; set; } = new();

        private UserRes _selectedUser;
        public UserRes SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                if (value != null)
                {
                    if (IsEditMode)
                        UpdateRequest.UserId = value.Id;
                    else
                        Request.UserId = value.Id;
                }
                OnPropertyChanged();
            }
        }

        public bool IsLoading { get; set; }

        public AsyncRelayCommand<object> TaskViewCommand { get; }
        public AsyncRelayCommand<object> SaveCommand => new(async (p) => await Save());

        public TaskViewModel()
        {
            TaskViewCommand = new AsyncRelayCommand<object>((p) => Init());
            Request = new TaskReq();
            IsEditMode = false;
        }

        public TaskViewModel(TaskRes existingTask)
        {
            TaskViewCommand = new AsyncRelayCommand<object>((p) => Init());
            IsEditMode = true;



            var assignedBy = GetFromToken.GetUserId();

            if (assignedBy == 0)
            {
                MessageBoxHelper.ShowError("Vui lòng đăng nhập lại");

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

            UpdateRequest = new TaskUpdate()
            {
                Id = existingTask.Id,
                UserId = int.TryParse(existingTask.AssigneeName, out int anId) ? anId : 0,
                Title = existingTask.Title,
                Description = existingTask.Description,
                AssignedBy = assignedBy
            };
            OnPropertyChanged(nameof(UpdateRequest));
        }

        public async Task Init()
        {
            await LoadUser();
        }

        public async Task LoadUser()
        {
            try
            {
                IsLoading = true;

                var url = "https://localhost:7089/api/User/get-list";
                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<UserRes>>>(url);

                if (response?.Code == 200 && response.Data != null)
                {
                    Users = new ObservableCollection<UserRes>(response.Data);
                    OnPropertyChanged(nameof(Users));

                    if (IsEditMode && UpdateRequest != null)
                    {
                        var matchedUser = Users.FirstOrDefault(u => u.Id == UpdateRequest.UserId);
                        if (matchedUser != null)
                            SelectedUser = matchedUser;
                    }
                }
                else
                {
                    MessageBoxHelper.ShowError("Lỗi gọi API: " + (response?.Message ?? "Không xác định"));
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError("Lỗi ngoại lệ khi tải danh sách người dùng: " + ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task Save()
        {
            try
            {
                if (SelectedUser == null)
                {
                    MessageBoxHelper.ShowError("Vui lòng chọn người thực hiện.");
                    return;
                }

                if (IsEditMode)
                {
                    var a = UpdateRequest;
                    var response = await _httpClient.PutAsJsonAsync($"https://localhost:7089/api/Task/update-task/{UpdateRequest.Id}", UpdateRequest);
                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBoxHelper.ShowError("Cập nhật thất bại");
                        return;
                    }
                    MessageBoxHelper.ShowInfo("Cập nhật thành công");
                }
                else
                {

                    var userId = GetFromToken.GetUserId();

                    if (userId == 0)
                    {
                        MessageBoxHelper.ShowError("Vui lòng đăng nhập lại");

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

                    Request.AssignedBy = userId;

                    var response = await _httpClient.PostAsJsonAsync("https://localhost:7089/api/Task/create", Request);
                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBoxHelper.ShowError("Thêm mới thất bại");
                        return;
                    }

                    MessageBoxHelper.ShowInfo("Thêm mới thành công");
                }

                TaskSaved?.Invoke(this, EventArgs.Empty);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Application.Current.Windows.OfType<TaskWindow>().FirstOrDefault()?.Close();
                });
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowError("Lỗi khi lưu: " + ex.Message);
            }
        }
    }

}
