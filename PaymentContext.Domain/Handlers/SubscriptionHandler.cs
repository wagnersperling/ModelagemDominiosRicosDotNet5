using System;
using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : Notifiable,
    IHandler<CreateBoletoSubscriptionCommand>,
    IHandler<CreatePayPalSubscriptionCommand>
    {
        private readonly IStudentRepository _studentRepository;

        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository studentRepository, IEmailService emailService)
        {
            _studentRepository = studentRepository;
            _emailService = emailService;
        }

        public ICommandResult Handler(CreateBoletoSubscriptionCommand command)
        {
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            }

            if (_studentRepository.DocumentExists(command.Document))
            {
                AddNotification("Document", "Esse CPF já está em uso");
            }

            if (_studentRepository.EmailExists(command.Email))
            {
                AddNotification("Email", "Esse E-mail já está em uso");
            }

            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            var student = new Student(name, document, email, address);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email,
                command.BarCode,
                command.BoletoNumber
            );

            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            AddNotifications(name, document, email, address, student, subscription, payment);

            if (Invalid)
            {
                return new CommandResult(false, "Não foi possível realizar sua assinatura");
            }

            _studentRepository.CreateSubscription(student);

            _emailService.Send(student.Name.ToString(), student.Email.Address, "bem vindo ao balta.io", "Sua assinatura foi criada");

            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handler(CreatePayPalSubscriptionCommand command)
        {
            if (_studentRepository.DocumentExists(command.Document))
            {
                AddNotification("Document", "Esse CPF já está em uso");
            }

            if (_studentRepository.EmailExists(command.Email))
            {
                AddNotification("Email", "Esse E-mail já está em uso");
            }

            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            var student = new Student(name, document, email, address);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(
                command.TransactionCode,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email
            );

            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            AddNotifications(name, document, email, address, student, subscription, payment);

            _studentRepository.CreateSubscription(student);

            _emailService.Send(student.Name.ToString(), student.Email.Address, "bem vindo ao balta.io", "Sua assinatura foi criada");

            return new CommandResult(true, "Assinatura realizada com sucesso");
        }
    }
}