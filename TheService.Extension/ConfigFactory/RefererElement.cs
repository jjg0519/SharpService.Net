using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TheService.Extension.ConfigFactory
{
    public class RefererElement
    {
        [Required]
        public string Id { get; set; }

        public string Interface { get; set; }

        public string Assembly { get; set; }

        [Required]
        public List<Referer> Referers { get; set; }
    }
}
