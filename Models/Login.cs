using System.ComponentModel.DataAnnotations;


namespace AnticaPizza.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Il campo Username/Email è obbligatorio")]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Il campo Password è obbligatorio")]
        public string Password { get; set; }
    }
}