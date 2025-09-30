using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.Dto
{
    public class AddWalkRequestDto
    {
       
        [Required]
        [MaxLength(255, ErrorMessage = "Maximum characters should not exceed 255")]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Maximum characters should not exceed 1000")]
        public string Description { get; set; }

        [Required]
        [Range(0, 50)]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        [Required]
        public Guid RegionId { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }
    }
}
