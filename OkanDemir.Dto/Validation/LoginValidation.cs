using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class LoginValidation : AbstractValidator<LoginRequestDto>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı Adı Boş Olamaz");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre Boş Olamaz");
            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("Key Boş Olamaz");
        }
    }
}
