using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaBrainsJSCAppFE.Handel
{
    public class AppMemory
    {
        private static AppMemory _instance;
        public static AppMemory Instance => _instance ??= new AppMemory();

        public string? Token { get; set; }
        public string? CurrentUserName { get; set; }

        public ObservableCollection<TaskItemViewModel> CachedTasks { get; set; } = new();

        private AppMemory() { }
    }
}
