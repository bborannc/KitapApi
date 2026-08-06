using FluentValidation;
using KitapApi.Dtos;

namespace KitapApi.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Kullanıcı adı boş olamaz.");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");
            RuleFor(x => x.Role).Must(role => role == "Admin" || role == "Member")
                .WithMessage("Rol sadece 'Admin' veya 'Member' olabilir.");
        }
    }
}
