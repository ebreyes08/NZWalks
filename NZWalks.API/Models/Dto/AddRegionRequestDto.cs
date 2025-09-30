using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.Dto
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Minimum characters should be 3")]
        [MaxLength(3, ErrorMessage = "Maximum characters should not exceed 3")]
        public string Code { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Maximum characters should not exceed 255")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
