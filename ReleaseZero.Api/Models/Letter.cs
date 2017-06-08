using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ReleaseZero.Api.Models
{
    /// <summary>
    /// Letter.
    /// </summary>
    public class Letter
    {
        /// <summary>
        /// Gets or sets the character.
        /// </summary>
        /// <value>The character.</value>
        [Key]
        [Display(Name = "Character")]
        [JsonProperty("character")]
        [RegularExpression(@"^[a-zA-Z]{1}$", ErrorMessage =  "{0} must be a letter")]
        [Required(ErrorMessage = "{0} is required")]
        public char Character { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        [Display(Name = "Position")]
        [JsonProperty("position")]
        [Range(1, 26)]
        [Required(ErrorMessage = "{0} is required")]
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the telephony.
        /// </summary>
        /// <value>The telephony.</value>
        [Display(Name = "Telephony")]
        [JsonProperty("telephony")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "The value of '{0}' must be between {2} and {1} characters")]
        public string Telephony { get; set; }

        /// <summary>
        /// Gets or sets the morse code.
        /// </summary>
        /// <value>The morse code.</value>
        [Display(Name = "Morse Code")]
        [JsonProperty("morseCode")]
        [RegularExpression(@"^[·-]{1,4}$", ErrorMessage = "{0} must consist of one to four '·' and '-' characters")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(4, MinimumLength = 1, ErrorMessage = "{0} must be between {2} and {1} characters")]
        public string MorseCode { get; set; }
    }
}
