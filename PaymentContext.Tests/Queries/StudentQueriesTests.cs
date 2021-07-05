using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Queries;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests.Queries
{
    [TestClass]
    public class StudentQueriesTests
    {
        private List<Student> _students;

        public StudentQueriesTests()
        {
            _students = new List<Student>();
            for (var i = 0; i <= 10; i++)
            {
                _students.Add(new Student(
                    new Name("Aluno", i.ToString()),
                    new Document("8888888888" + i.ToString(), EDocumentType.CPF),
                    new Email(i.ToString() + "@teste.com"),
                    new Address("rua" + i.ToString(), "numero" + i.ToString(), "bairro" + i.ToString(), "cidade" + i.ToString(), "estado" + i.ToString(), "país" + i.ToString(), "cep" + i.ToString())
                ));
            }
        }

        [TestMethod]
        public void ShouldReturnNullWhenDocumentIsNotExists()
        {
            var exp = StudentQueries.GetStudentInfo("88888888877");
            var student = _students.AsQueryable().Where(exp).FirstOrDefault();
            Assert.IsNull(student);
        }

        [TestMethod]
        public void ShouldReturnStudentWhenDocumentExists()
        {
            var exp = StudentQueries.GetStudentInfo("88888888888");
            var studn = _students.AsQueryable().Where(exp).FirstOrDefault();

            Assert.AreNotEqual(null, studn);
        }
    }
}