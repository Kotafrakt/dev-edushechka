﻿using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.OutputModels
{
    public class StudentAnswerOnTaskInfoOutputModel
    {
        public int Id { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string Status { get; set; }
        public string Answer { get; set; }
    }
}