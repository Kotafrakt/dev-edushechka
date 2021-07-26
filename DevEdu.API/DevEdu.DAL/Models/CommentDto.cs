﻿using System;
using System.Collections.Generic;

namespace DevEdu.DAL.Models
{
    public class CommentDto : BaseDto
    {
        public string Text { get; set; }
        public UserDto User { get; set; }
        public LessonDto Lesson { get; set; }
        public StudentAnswerOnTaskDto StudentAnswer { get; set; }
        public DateTime Date { get; set; }
    }
}