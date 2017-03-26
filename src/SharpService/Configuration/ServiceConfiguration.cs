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

        public string Version { get; set; }

        [Required]
        public int Port { get; set; }

        public string Protocol { get; set; }
    }
}
