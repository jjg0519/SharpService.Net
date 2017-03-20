using System.ComponentModel.DataAnnotations;

namespace SharpService.Configuration
{
    public class RefererConfiguration
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Interface { get; set; }

        [Required]
        public string Assembly { get; set; }   
    }
}
