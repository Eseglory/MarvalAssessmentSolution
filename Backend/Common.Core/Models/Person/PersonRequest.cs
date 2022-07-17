﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Core.Models.Person
{
    public class PersonRequest
    {
        // this request model is used for both creation and modification of person
        // Identity is auto generated by the system so as to avoid human errors
        //you only enter Identity for update

        //Here we are using Data Annotations for vlidation
        //We can use external libraries like Abstract Validator in FluentValidation 

        //long data type can hold millions of Identity(record) that's why am using long not int
        public long Identity { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(0, 150, ErrorMessage = "The Age should be between 0 and 150 years")] 
        public int Age { get; set; }

        [Required(ErrorMessage = "Sex is required")]
        public string Sex { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Active is required")]
        public string Active { get; set; }
    }
}
