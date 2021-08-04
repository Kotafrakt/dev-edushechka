﻿using System;
using System.Collections.Generic;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public class StudentAnswerOnTaskService : IStudentAnswerOnTaskService
    {
        private readonly IStudentAnswerOnTaskRepository _studentAnswerOnTaskRepository;
        private readonly IStudentAnswerOnTaskValidationHelper _studentAnswerOnTaskValidationHelper;
        private const string _dateFormat = "dd.MM.yyyy HH:mm";
        private readonly IUserValidationHelper _userValidationHelper;
        private readonly ITaskValidationHelper _taskValidationHelper;

        public StudentAnswerOnTaskService(IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository, 
            IStudentAnswerOnTaskValidationHelper studentAnswerOnTaskValidationHelper,
            IUserValidationHelper userValidationHelper,
            ITaskValidationHelper taskValidationHelper)
        {
            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
            _studentAnswerOnTaskValidationHelper = studentAnswerOnTaskValidationHelper;
            _userValidationHelper = userValidationHelper;
            _taskValidationHelper = taskValidationHelper;
        }

        public int AddStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto taskAnswerDto)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(studentId);
            _taskValidationHelper.CheckTaskExistence(taskId);

            taskAnswerDto.Task = new TaskDto { Id = taskId };
            taskAnswerDto.User = new UserDto { Id = studentId };

            var studentAnswerId = _studentAnswerOnTaskRepository.AddStudentAnswerOnTask(taskAnswerDto);

            return studentAnswerId;
        }

        public void DeleteStudentAnswerOnTask(int taskId, int studentId)
        {
            _studentAnswerOnTaskValidationHelper.CheckStudentAnswerOnTaskExistence(taskId, studentId);

            _studentAnswerOnTaskRepository.DeleteStudentAnswerOnTask(taskId, studentId);
        }

        public List<StudentAnswerOnTaskDto> GetAllStudentAnswersOnTask(int taskId)
        {
            _taskValidationHelper.CheckTaskExistence(taskId);

            return _studentAnswerOnTaskRepository.GetAllStudentAnswersOnTask(taskId);
        }

        public StudentAnswerOnTaskDto GetStudentAnswerOnTaskByTaskIdAndStudentId(int taskId, int studentId)
        {
            _studentAnswerOnTaskValidationHelper.CheckStudentAnswerOnTaskExistence(taskId, studentId);

            var answerDto = _studentAnswerOnTaskRepository.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, studentId);
            return answerDto;
        }

        public int ChangeStatusOfStudentAnswerOnTask(int taskId, int studentId, int statusId)
        {
            _studentAnswerOnTaskValidationHelper.CheckStudentAnswerOnTaskExistence(taskId, studentId);

            DateTime completedDate = default;

            if (statusId == (int)TaskStatus.Accepted)
                completedDate = DateTime.Now;

            string stringTime = completedDate.ToString(_dateFormat);
            DateTime time = Convert.ToDateTime(stringTime);

            var status = _studentAnswerOnTaskRepository.ChangeStatusOfStudentAnswerOnTask(taskId, studentId, statusId, time);

            return status;
        }

        public StudentAnswerOnTaskDto UpdateStudentAnswerOnTask(int taskId, int studentId, StudentAnswerOnTaskDto taskAnswerDto)
        {
            _studentAnswerOnTaskValidationHelper.CheckStudentAnswerOnTaskExistence(taskId, studentId);

            taskAnswerDto.Task = new TaskDto { Id = taskId };
            taskAnswerDto.User = new UserDto { Id = studentId };

            _studentAnswerOnTaskRepository.UpdateStudentAnswerOnTask(taskAnswerDto);

            return _studentAnswerOnTaskRepository.GetStudentAnswerOnTaskByTaskIdAndStudentId(taskId, studentId);
        }

        public int AddCommentOnStudentAnswer(int taskStudentId, int commentId) => _studentAnswerOnTaskRepository.AddCommentOnStudentAnswer(taskStudentId, commentId);

        public List<StudentAnswerOnTaskDto> GetAllAnswersByStudentId(int userId)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);

            var dto = _studentAnswerOnTaskRepository.GetAllAnswersByStudentId(userId);
            return dto;
        }
    }
}
