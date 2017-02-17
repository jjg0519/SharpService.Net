using System.ComponentModel.DataAnnotations;

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
        public string Binding { get; set; } = "nettcp";

        public int Security { get; set; } = 0;

        public string Address { get; set; }

        public string Export { get; set; }

        public string Version { get; set; }

        public bool Enable { get; set; } = true;
    }
}
