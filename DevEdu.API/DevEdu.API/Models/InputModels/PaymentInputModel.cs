﻿using System;
using System.ComponentModel.DataAnnotations;


namespace DevEdu.API.Models.InputModels
{
    public class PaymentInputModel
    {
        [Required]

        public DateTime Date { get; set; }
        public int Summ { get; set; }
        public int User { get; set; }
        public int IsPaid { get; set; }

    }
}
