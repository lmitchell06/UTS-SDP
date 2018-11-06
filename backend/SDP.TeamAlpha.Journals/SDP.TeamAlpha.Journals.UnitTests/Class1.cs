using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SDP.TeamAlpha.Journals.Application.Validators;
using Moq;
using SDP.TeamAlpha.Journals.Application;

namespace SDP.TeamAlpha.Journals.UnitTests
{
    [TestFixture]
    public class ValidatorTests
    {
        private Mock<IUserRepository> _repository;

        [Test]
        public void given_username_or_something()
        {
            _repository = new Mock<IUserRepository>();
            _repository.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            LoginValidator validator = new LoginValidator(_repository.Object);
            ValidatorResult expectedResult = new ValidatorResult
            {
                Field = "Username",
                Message = "Success",
                IsValid = true
            };
            Assert.AreEqual(validator.ValidateUsername(It.IsAny<string>()), expectedResult);
        }
    }
}
