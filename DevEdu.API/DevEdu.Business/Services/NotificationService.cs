﻿using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using DevEdu.DAL.Enums;

namespace DevEdu.Business.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationValidationHelper _notificationValidationHelper;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserValidationHelper _userValidationHelper;
        private readonly IGroupValidationHelper _groupValidationHelper;


        public NotificationService(INotificationRepository notificationRepository,
            INotificationValidationHelper notificationValidationHelper,
            IUserValidationHelper userValidationHelper,
            IGroupValidationHelper groupValidationHelper)
        {
            _notificationRepository = notificationRepository;
            _notificationValidationHelper = notificationValidationHelper;
            _userValidationHelper = userValidationHelper;
            _groupValidationHelper = groupValidationHelper;
        }

        public NotificationDto GetNotification(int id)
        {
            var dto =_notificationValidationHelper.GetNotificationByIdAndThrowIfNotFound(id);
            return dto;
        }

        public List<NotificationDto> GetNotificationsByUserId(int userId)
        {
            _userValidationHelper.GetUserByIdAndThrowIfNotFound(userId);
            var list = _notificationRepository.GetNotificationsByUserId(userId);
            return list;
        }
        public List<NotificationDto> GetNotificationsByGroupId(int groupId)
        {
            _groupValidationHelper.CheckGroupExistence(groupId);
            var list = _notificationRepository.GetNotificationsByGroupId(groupId);
            return list;
        }
        public List<NotificationDto> GetNotificationsByRoleId(int RoleId)
        {
            var list = _notificationRepository.GetNotificationsByRoleId(RoleId);
            return list;
        }

        public NotificationDto AddNotification(NotificationDto dto, UserIdentityInfo userInfo)
        {
            if (userInfo.IsTeacher() && dto.Group == null)
            {

            }
            _notificationValidationHelper.CheckRoleIdUserIdGroupIdIsNotNull(dto);
            var output = _notificationRepository.AddNotification(dto);
            return GetNotification(output);
        }

        public void DeleteNotification(int id, UserIdentityInfo userInfo)
        {
            var checkedDto = _notificationValidationHelper.GetNotificationByIdAndThrowIfNotFound(id);
            if (userInfo.IsTeacher())
            {
                _notificationValidationHelper.CheckNotificationIsForGroup(checkedDto, userInfo.UserId);
                _userValidationHelper.CheckAuthorizationUserToGroup(checkedDto.Group.Id, userInfo.UserId, Role.Teacher);
            }
            _notificationRepository.DeleteNotification(id);
        }

        public NotificationDto UpdateNotification(int id, NotificationDto dto, UserIdentityInfo userInfo)
        {
            var checkedDto = _notificationValidationHelper.GetNotificationByIdAndThrowIfNotFound(id);
            dto.Id = id;
            if (userInfo.IsTeacher())
            {
                _notificationValidationHelper.CheckNotificationIsForGroup(checkedDto, userInfo.UserId);
                _userValidationHelper.CheckAuthorizationUserToGroup(checkedDto.Group.Id, userInfo.UserId, Role.Teacher);
            }
            _notificationRepository.UpdateNotification(dto);
            return _notificationRepository.GetNotification(id);
        }
    }
}
