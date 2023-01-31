using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class ExpenseValidation : AbstractValidator<ExpenseDto>
    {
        public ExpenseValidation()
        {
            RuleFor(x => x.Price > 0)
                .NotEmpty().WithMessage("Tutar Boş Olamaz");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık Boş Olamaz");
        }
    }
}
