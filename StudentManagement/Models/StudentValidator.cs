using FluentValidation;

namespace StudentManagement.Models
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters");

            RuleFor(x => x.Age)
                .InclusiveBetween(1, 120).WithMessage("Age must be between 1 and 120");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email is required");

            RuleFor(x => x.Course)
                .NotEmpty().WithMessage("Course is required");

            RuleFor(x => x.Address)
                .NotEmpty().MaximumLength(250).WithMessage("Address must be 250 characters or fewer");
        }
    }
}
