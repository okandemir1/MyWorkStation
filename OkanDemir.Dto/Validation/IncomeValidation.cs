using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class IncomeValidation : AbstractValidator<IncomeDto>
    {
        public IncomeValidation()
        {
            RuleFor(x => x.Price > 0)
                .NotEmpty().WithMessage("Tutar Boş Olamaz");
            RuleFor(x => x.IncomeTypeId > 0)
                .NotEmpty().WithMessage("Kategori Boş Olamaz");
        }
    }
}
