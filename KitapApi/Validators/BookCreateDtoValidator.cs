using FluentValidation;
using KitapApi.Dtos;


namespace KitapApi.Validators
{
    public class BookCreateDtoValidator : AbstractValidator<BookCreateDto>
    {
        public BookCreateDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Kitap başlığı boş geçilemez.");
            RuleFor(x => x.Author).NotEmpty().WithMessage("Yazar adı boş geçilemez.");
        }
    }
}
