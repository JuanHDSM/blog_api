using System.ComponentModel.DataAnnotations;

namespace blog_api.ViewModels.Accounts
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatórip")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "O E-mail é obrigatórip")]
        [EmailAddress(ErrorMessage = "O E-mail é inválido")]
        public string Email { get; set; } = string.Empty;
    }
}