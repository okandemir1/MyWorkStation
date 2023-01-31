using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class SubscriptionValidation : AbstractValidator<SubscriptionDto>
    {
        public SubscriptionValidation()
        {
            RuleFor(x => x.SubscriptionTypeId > 0)
                .NotEmpty().WithMessage("Abonelik Tipi Seçilmesi Zorunludur");
        }
    }
}
