using FluentValidation;

namespace OkanDemir.Dto.Validation
{
    public class NoteValidation : AbstractValidator<NoteDto>
    {
        public NoteValidation()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Not Boş Olamaz");
        }
    }
}
