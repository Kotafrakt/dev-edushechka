﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DevEdu.API.Models;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Extensions
{
    public static class ControllerExtensions
    {
        public static int GetUserId(this ControllerBase controller)
        {
            var userId =  Convert.ToInt32(controller.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            return userId;
        }

        public static List<Role> GetUserRoles(this ControllerBase controller)
        {
            return controller.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => (Role)Enum.Parse(typeof(Role), c.Value)).ToList();
        }

        public static UserDto GetUserIdAndRoles(this Controller controller)
        {
            var userId = Convert.ToInt32(controller.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var roles = controller.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => (Role)Enum.Parse(typeof(Role), c.Value)).ToList();
            return new UserDto
            {
                Id = userId,
                Roles = roles
            };
        }
    }
}
