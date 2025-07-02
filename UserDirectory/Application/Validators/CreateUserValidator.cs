using FluentValidation;
using UserDirectory.Application.Dtos;

namespace UserDirectory.Application.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Company).NotEmpty();

        RuleFor(x => x.Sex)
            .Must(s => s is "M" or "F")
            .WithMessage("Sex must be 'M' or 'F'.");

        RuleFor(x => x.Contact.Phone).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Contact.Address).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Contact.City).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Contact.Country).NotEmpty().MaximumLength(50);

        RuleFor(x => x.RoleId).GreaterThan(0);
    }
}

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator() => Include(new CreateUserValidator());
}
