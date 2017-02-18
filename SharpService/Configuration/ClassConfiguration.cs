using System.ComponentModel.DataAnnotations;

namespace SharpService.Configuration
{
    public class ClassConfiguration
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Type { get; set; }

        public string Assembly { get; set; }
    }
}
