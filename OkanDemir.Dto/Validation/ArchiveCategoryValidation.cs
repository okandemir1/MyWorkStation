using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class ArchiveCategoryValidation : AbstractValidator<ArchiveCategoryDto>
    {
        public ArchiveCategoryValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori Adı Boş Olamaz");
        }
    }
}
