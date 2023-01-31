using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class ArchiveValidation : AbstractValidator<ArchiveDto>
    {
        public ArchiveValidation()
        {
            RuleFor(x => x.ArchiveCategoryId > 0)
                .NotEmpty().WithMessage("Arşiv kategori seçilmemiş");
            RuleFor(x => x.Domain)
                .NotEmpty().WithMessage("Domain boş bırakılamaz");
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş bırakılamaz");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş bırakılamaz");
            RuleFor(x => x.Fullname)
                .NotEmpty().WithMessage("Ad Soyad boş bırakılamaz");
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon boş bırakılamaz");
            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("Key boş bırakılamaz");
        }
    }
}
