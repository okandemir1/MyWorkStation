using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class SubscriptionTypeValidation : AbstractValidator<SubscriptionTypeDto>
    {
        public SubscriptionTypeValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık Boş Olamaz");
        }
    }
}
