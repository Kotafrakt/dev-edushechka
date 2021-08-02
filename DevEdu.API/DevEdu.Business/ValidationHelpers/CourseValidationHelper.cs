﻿using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class CourseValidationHelper : ICourseValidationHelper
    {
        private readonly ICourseRepository _courseRepository;

        public CourseValidationHelper(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public void CheckCourseExistence(int courseId)
        {
            var course = _courseRepository.GetCourse(courseId);
            if (course == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(course), courseId));
        }
    }
}