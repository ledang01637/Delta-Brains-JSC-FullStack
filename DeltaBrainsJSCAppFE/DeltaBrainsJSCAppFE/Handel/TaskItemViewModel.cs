using DeltaBrainsJSCAppFE.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DeltaBrainsJSCAppFE.Handel
{
    public class TaskItemViewModel
    {
        //Lấy chữ cái đầu tiền của name và đổi màu background
        public TaskItemViewModel(TaskRes task)
        {
            Task = task;

            var random = new Random(Guid.NewGuid().GetHashCode());
            BgColor = new SolidColorBrush(Color.FromRgb(
                (byte)random.Next(100, 256),
                (byte)random.Next(100, 256),
                (byte)random.Next(100, 256)));
        }
        public TaskRes Task { get; }

        public string Character => string.IsNullOrEmpty(Task.AssigneeName)
            ? ""
            : Task.AssigneeName.Substring(0, 1).ToUpper();

        public string AssigneeName => Task.AssigneeName;
        public int Id => Task.Id;
        public string? Title => Task.Title;
        public string? Description => Task.Description;
        public string? Status => Task.Status;

        public SolidColorBrush BgColor { get; }
    }
}
