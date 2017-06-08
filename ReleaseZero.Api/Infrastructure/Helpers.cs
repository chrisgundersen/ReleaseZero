using ReleaseZero.Api.Models;

namespace ReleaseZero.Api.Infrastructure
{
    /// <summary>
    /// Helpers.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Gets the letter array.
        /// </summary>
        /// <returns>The letter array.</returns>
        public static Letter[] GetLetterArray()
        {
            var letterArray = new Letter[26] {
                new Letter { Character = 'a', Position = 1, Telephony = "Alfa", MorseCode = "·-" },
                new Letter { Character = 'b', Position = 2, Telephony = "Bravo", MorseCode = "-···" },
                new Letter { Character = 'c', Position = 3, Telephony = "Charlie", MorseCode = "-·-·" },
	            new Letter { Character = 'd', Position = 4, Telephony = "Delta", MorseCode = "-··" },
	            new Letter { Character = 'e', Position = 5, Telephony = "Echo", MorseCode = "·" },
	            new Letter { Character = 'f', Position = 6, Telephony = "Foxtrot", MorseCode = "··-·" },
	            new Letter { Character = 'g', Position = 7, Telephony = "Golf", MorseCode = "--·" },
	            new Letter { Character = 'h', Position = 8, Telephony = "Hotel", MorseCode = "····" },
	            new Letter { Character = 'i', Position = 9, Telephony = "India", MorseCode = "··" },
	            new Letter { Character = 'j', Position = 10, Telephony = "Juliet", MorseCode = "·---" },
	            new Letter { Character = 'k', Position = 11, Telephony = "Kilo", MorseCode = "-·-" },
	            new Letter { Character = 'l', Position = 12, Telephony = "Lima", MorseCode = "·-··" },
	            new Letter { Character = 'm', Position = 13, Telephony = "Mike", MorseCode = "--" },
	            new Letter { Character = 'n', Position = 14, Telephony = "November", MorseCode = "-·" },
	            new Letter { Character = 'o', Position = 15, Telephony = "Oscar", MorseCode = "---" },
	            new Letter { Character = 'p', Position = 16, Telephony = "Papa", MorseCode = "·--·" },
	            new Letter { Character = 'q', Position = 17, Telephony = "Quebec", MorseCode = "--·-" },
	            new Letter { Character = 'r', Position = 18, Telephony = "Romeo", MorseCode = "·-·" },
	            new Letter { Character = 's', Position = 19, Telephony = "Sierra", MorseCode = "···" },
	            new Letter { Character = 't', Position = 20, Telephony = "Tango", MorseCode = "-" },
	            new Letter { Character = 'u', Position = 21, Telephony = "Uniform", MorseCode = "··-" },
	            new Letter { Character = 'v', Position = 22, Telephony = "Victor", MorseCode = "···-" },
	            new Letter { Character = 'w', Position = 23, Telephony = "Whiskey", MorseCode = "·--" },
	            new Letter { Character = 'x', Position = 24, Telephony = "XRay", MorseCode = "-··-" },
	            new Letter { Character = 'y', Position = 25, Telephony = "Yankee", MorseCode = "-·--" },
	            new Letter { Character = 'z', Position = 26, Telephony = "Zulu", MorseCode = "--··" }
            };

            return letterArray;
        }
    }
}
