using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class InvitationReqModelValidation : AbstractValidator<UserDto>
    {
        public InvitationReqModelValidation()
        {
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon numarası boş olamaz.")
                .Must(Helpers.CheckMobilePhone).WithMessage("Hatalı Telefon Formatı (Örn :5391111111).");
            RuleFor(x => x.Fullname)
                .NotEmpty().WithMessage("Ad boş olamaz.");
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Yetenekler boş bırakılamaz");
        }
    }
}
