using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace guestbook.Models
{
    public class guest
    {

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Emails must be in the form 'xyz@xyz.com'")]
        public string email { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string date { get; set; }

        [Required]
        public string comment { get; set; }        
        
        [Required]
        public string language { get; set; }
        public string image { get; set; }

    }
}
