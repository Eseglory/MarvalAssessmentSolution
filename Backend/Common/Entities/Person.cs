﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Entities
{
    public class Person
    {
        [Key]
        public int Identity { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required]
        //use string because some phone numbers are alphanueric
        public string Mobile { get; set; }

        [Required]
        public string Active { get; set; }
    }
}
