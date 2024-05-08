using System.ComponentModel.DataAnnotations;

namespace FoodFeedbackForm.Models
{
    public class Feedback
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Invalid name")]
        public string name { get; set; }
        public string serialNumber { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }

        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Invalid phone number")]
        public string mobNo { get; set; }

        public byte[] Image { get; set; }
        public string foodLiked { get; set; }
        public string preferedTimeSlot { get; set; }
        public string visitingStatus { get; set; }

    }
}
