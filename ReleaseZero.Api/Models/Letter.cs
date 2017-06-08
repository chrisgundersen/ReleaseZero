using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ReleaseZero.Api.Models
{
    public class Foo
    {
        [Key]
        [Display(Name = "Id")]
        [JsonProperty("id")]
        [Required(ErrorMessage = "{0} is required")]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [JsonProperty("name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "The value of '{0}' must be between {2} and {1} characters")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("bar")]
        public bool Bar { get; set; }
    }
}
