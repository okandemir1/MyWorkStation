using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class RegisterValidation : AbstractValidator<RegisterRequestDto>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.Fullname)
                .NotEmpty().WithMessage("Ad Soyad Boş Olamaz");
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı Adı Boş Olamaz");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre Boş Olamaz");
            RuleFor(x => x.RPassword)
                .NotEmpty().WithMessage("Şifre Tekrar Boş Olamaz");
            RuleFor(x => x.RPassword == x.Password)
                .NotEmpty().WithMessage("Şifreler Uyuşmuyor Boş Olamaz");
            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("Key Boş Olamaz");
            RuleFor(x => x.Phone)
                .Must(Helpers.CheckMobilePhone).WithMessage("Geçersiz telefon numarası")
                .NotEmpty().WithMessage("Telefon Numarası Boş Olamaz");
            RuleFor(x => x.Mail)
                .Must(Helpers.CheckEmail).WithMessage("Geçersiz E-Posta Adresi")
                .NotEmpty().WithMessage("E-Posta Adresi Boş Olamaz");
            RuleFor(x => x.IdNo)
                .Must(Helpers.IdNoValid).WithMessage("Geçersiz TC Kimlik Numarası")
                .NotEmpty().WithMessage("TcNo Boş Olamaz");
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres Boş Olamaz");
        }
    }
}
