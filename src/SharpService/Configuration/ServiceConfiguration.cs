using System.ComponentModel.DataAnnotations;

namespace SharpService.Configuration
{
    public class ServiceConfiguration
    {
        [Required]
        public string Ref { get; set; }

        [Required]
        public string Interface { get; set; }

        [Required]
        public string Assembly { get; set; }

        [Required]
        public string Binding { get; set; } = "nettcp";

        public int Security { get; set; } = 0;

        [Required]
        public string Export { get; set; }

        public string Version { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        public int Port { get; set; }

        [Required]
        public string Address { get; set; }

        public bool Enable { get; set; } = true;
    }
}
