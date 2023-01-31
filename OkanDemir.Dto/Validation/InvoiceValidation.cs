using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class InvoiceValidation : AbstractValidator<InvoiceDto>
    {
        public InvoiceValidation()
        {
            RuleFor(x => x.InvoiceTypeId > 0)
                .NotEmpty().WithMessage("Fatura Tipi Seçilmesi Zorunludur");
            RuleFor(x => x.Price > 0)
                .NotEmpty().WithMessage("Tutar 0 veya Boş Olamaz");
        }
    }
}
