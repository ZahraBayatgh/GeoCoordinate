using FluentValidation;
using Microsoft.eShopOnContainers.Services.Location.API.Application.Behaviors.Commands;
using Microsoft.Extensions.Logging;

namespace Location.API.Application.Behaviors.Validations
{
    public class AddUserLocationCommandValidator : AbstractValidator<AddUserLocationCommand>
    {
        public AddUserLocationCommandValidator(ILogger<AddUserLocationCommandValidator> logger)
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User not found");

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}