﻿using AutoMapper;
using DeltaBrainsJSCAppBE.DTOs.Request;
using DeltaBrainsJSCAppBE.DTOs.Response;
using Task = DeltaBrainsJSCAppBE.Models.Task;
using TaskStatus = DeltaBrainsJSCAppBE.Enum.TaskStatus;

namespace DeltaBrainsJSCAppBE.AutoMapper
{
    public class TaskMapper : Profile
    {
        public TaskMapper()
        {
            CreateMap<TaskReq, Task>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetEnumStatus("chưa thực hiện")));

            CreateMap<Task, TaskRes>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetStatusString(src.Status)))
                .ForMember(dest => dest.AssigneeName, opt => opt.MapFrom(src => src.Assignee != null ? src.Assignee.Name : string.Empty))
                .ForMember(dest => dest.AssignedBy, opt => opt.MapFrom(src => src.AssignedByUser != null ? src.AssignedByUser.Name : string.Empty));

        }


        private static TaskStatus GetEnumStatus(string status)
        {
            return status.Trim().ToLower() switch
            {
                "chưa thực hiện" => TaskStatus.NotStarted,
                "đang thực hiện" => TaskStatus.InProgress,
                "hoàn thành" => TaskStatus.Completed,
                _ => throw new ArgumentException("Trạng thái không hợp lệ: " + status)
            };
        }

        private static string GetStatusString(TaskStatus status)
        {
            return status switch
            {
                TaskStatus.NotStarted => "Chưa thực hiện",
                TaskStatus.InProgress => "Đang thực hiện",
                TaskStatus.Completed => "Hoàn thành",
                _ => "Không xác định"
            };
        }
    }
}
