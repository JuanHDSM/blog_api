using System.ComponentModel.DataAnnotations;

namespace blog_api.ViewModels.Categories
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "O slug é obrigatório")]
        public string Slug { get; set; } = string.Empty;
    }
}