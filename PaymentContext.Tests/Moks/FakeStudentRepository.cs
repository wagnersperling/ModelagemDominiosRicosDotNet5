using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Repositories;

namespace PaymentContext.Tests.Moks
{
    public class FakeStudentRepository : IStudentRepository
    {
        public void CreateSubscription(Student student)
        {

        }

        public bool DocumentExists(string document)
        {
            if (document == "88888888888")
                return true;

            return false;
        }

        public bool EmailExists(string email)
        {
            if (email.Equals("teste@teste.com"))
                return true;
            return false;
        }
    }
}