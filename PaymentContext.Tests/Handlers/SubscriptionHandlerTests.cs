using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Tests.Moks;

namespace PaymentContext.Tests.Handlers
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExits()
        {
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Carlos";
            command.LastName = "Sperling";
            command.Document = "88888888888";
            command.Email = "teste1@teste1.com";
            command.BarCode = "123456789";
            command.BoletoNumber = "9999999999";
            command.PaymentNumber = "456456";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 46;
            command.TotalPaid = 46;
            command.Payer = "Carlos";
            command.PayerDocument = "01896557841";
            command.PayerDocumentType = EDocumentType.CPF;
            command.PayerEmail = "carlos@carlos.com";
            command.Street = "Rua 1";
            command.Number = "1234";
            command.Neighborhood = "Bairro 1";
            command.City = "cidade 1";
            command.State = "Estado 1";
            command.Country = "País 1";
            command.ZipCode = "94456189";
            var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
            handler.Handle(command);
            Assert.IsTrue(handler.Invalid);
        }

    }
}