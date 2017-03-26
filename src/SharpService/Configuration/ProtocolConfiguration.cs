using System.ComponentModel.DataAnnotations;

namespace SharpService.Configuration
{
    public class ProtocolConfiguration
    {
        public string Name { set; get; }

        [Required]
        public string Protocol { set; get; }

        [Required]
        public string Transmit { set; get; }

        public bool Defalut { get; set; }

        public string LoadBalance { get; set; }

        public bool CircuitBreak { get; set; } = false;

        public int ExceptionsAllowedBeforeBreaking { get; set; }

        public int DurationOfBreak { get; set; }

        public int ReTries { get; set; } = 2;
    }
}
