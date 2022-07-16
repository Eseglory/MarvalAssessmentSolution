﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Core.Models.Person
{
    public class PersonRequest
    {
        [Required(ErrorMessage = "Identity is required")]
        public long Identity { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Sex is required")]
        public string Sex { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        //use string because some phone numbers are alphanueric
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Active is required")]
        public string Active { get; set; }
    }
}