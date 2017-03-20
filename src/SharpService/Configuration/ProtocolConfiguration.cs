using System.ComponentModel.DataAnnotations;

namespace SharpService.Configuration
{
    public class ProtocolConfiguration
    {
        [Required]
        public string Protocol { set; get; }

        [Required]
        public string Transmit { set; get; }

        public string LoadBalance { get; set; }

        public string HaStrategy { get; set; } = "failfast";

        public int ReTries { get; set; } = 2;
    }
}
