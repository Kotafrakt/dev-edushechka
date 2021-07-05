﻿using System.ComponentModel.DataAnnotations;

namespace DevEdu.API.Models.InputModels
{
    public class CommentAddtInputModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Text { get; set; }
    }
}