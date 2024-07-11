using System.ComponentModel.DataAnnotations;

namespace blog_api.ViewModels.Accounts
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o E-mail")]
        [EmailAddress(ErrorMessage = "Informe um E-mail válido")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Informe uma senha")]
        public string Password { get; set; } = string.Empty;
    }
}