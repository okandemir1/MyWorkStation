using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class CodeNoteValidation : AbstractValidator<CodeNoteDto>
    {
        public CodeNoteValidation()
        {
            RuleFor(x => x.CodeCategoryId > 0)
                .NotEmpty().WithMessage("Kategori Seçilmesi Zorunludur.");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık Boş Olamaz");
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Kod Boş Olamaz");
        }
    }
}
