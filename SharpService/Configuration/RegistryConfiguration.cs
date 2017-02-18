using System.ComponentModel.DataAnnotations;

namespace SharpService.Configuration
{
    public class RegistryConfiguration
    {
        [Required]
        public string RegProtocol { set; get; }

        [Required]
        public string Name { set; get; }

        public string Address { set; get; }

        public int ConnectTimeout { set; get; } = 2000;
    }
}
