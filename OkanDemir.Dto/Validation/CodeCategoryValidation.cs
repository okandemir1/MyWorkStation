using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class CodeCategoryValidation : AbstractValidator<CodeCategoryDto>
    {
        public CodeCategoryValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Kategori Adı Boş Olamaz");
        }
    }
}
