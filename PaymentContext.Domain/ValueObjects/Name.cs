using Flunt.Validations;
using PaymentContext.Shared.ValueObjects;

namespace PaymentContext.Domain.ValueObjects
{
    public class Name : ValueObject
    {
        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            AddNotifications(new Contract()
                .Requires()
                .HasMinLen(FirstName, 3, "Name.FirstName", "Nome de conter pelo menos 3 caracteres")
                .HasMinLen(LastName, 3, "Name.LastName", "Sobrenome de conter pelo menos 3 caracteres")
                .HasMaxLen(FirstName, 40, "Name.FirstName", "Sobrenome de conter pelo menos 40 caracteres")
            );
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}