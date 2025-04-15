using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class TodoProjectValidation : AbstractValidator<TodoProjectDto>
    {
        public TodoProjectValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Proje Adı Boş Olamaz");
        }
    }
}
