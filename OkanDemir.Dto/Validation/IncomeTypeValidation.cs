using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class IncomeTypeValidation : AbstractValidator<IncomeTypeDto>
    {
        public IncomeTypeValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Adı Boş Olamaz");
            RuleFor(x => x.Period)
                .NotEmpty().WithMessage("Ödeme Periyodu Boş Olamaz");
            RuleFor(x => x.TotalPrice > 0)
                .NotEmpty().WithMessage("Ödenecek Tutar Boş Olamaz");
        }
    }
}
