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

        [Required]
        public string Interface { get; set; }

        public string Assembly { get; set; }

        [Required]
        public string Binding { get; set; }

        [DefaultValue(0)]
        public int Security { get; set; }

        //[Required]
        public List<string> Addresss { get; set; }

    }
}
