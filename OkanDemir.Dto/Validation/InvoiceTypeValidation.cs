using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class InvoiceTypeValidation : AbstractValidator<InvoiceTypeDto>
    {
        public InvoiceTypeValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık Boş Olamaz");
            RuleFor(x => x.ContractNumber)
                .NotEmpty().WithMessage("Kontrat Numarası Boş Olamaz");
        }
    }
}
