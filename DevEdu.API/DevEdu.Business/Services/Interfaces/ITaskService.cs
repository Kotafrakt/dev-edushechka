﻿using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface ITaskService
    {
        public TaskDto GetTaskById(int taskid, int userId);
        TaskDto GetTaskWithCoursesById(int taskid, int userId);
        public TaskDto GetTaskWithAnswersById(int taskid, int userId);
        public List<TaskDto> GetTasks(int userId);
        public TaskDto AddTaskByMethodist(TaskDto taskDto, List<int> coursesIds, List<int> tagsIds);
        public TaskDto AddTaskByTeacher(TaskDto taskDto, GroupTaskDto groupTask, int groupId, List<int> tagsIds);
        public TaskDto UpdateTask(TaskDto taskDto, int taskId, int userId);
        public void DeleteTask(int taskId, int userId);
        public int AddTagToTask(int taskId, int tagId);
        public void DeleteTagFromTask(int taskId, int tagId);
        public List<GroupTaskDto> GetGroupTasksByTaskId(int taskId);
        public TaskDto GetTaskWithGroupsById(int taskid, int userId);
    }
}