﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel.DataAnnotations;

namespace TheService.Extension.ConfigFactory
{
    public class ReferenceElement
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Type { get; set; }

        public string Assembly { get; set; }
    }
}
