using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SharpService.Configuration
{
    public class ServiceElement
    {
        [Required]
        public string Ref { get; set; }

        [Required]
        public string Interface { get; set; }

        public string Assembly { get; set; }

        [Required]
        public string Binding { get; set; }

        [DefaultValue(0)]
        public int Security { get; set; }

        //[Required]
        public string Address { get; set; }

        [DefaultValue(true)]
        public bool Enable { get; set; }
    }
}
