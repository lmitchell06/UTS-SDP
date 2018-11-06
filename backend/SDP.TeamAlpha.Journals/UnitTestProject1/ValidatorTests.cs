using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDP.TeamAlpha.Journals.Application.Validators;
using Moq;
using SDP.TeamAlpha.Journals.Application;

namespace UnitTestProject1
{
    [TestClass]
    public class ValidatorTests
    {
        private Mock<IUserRepository> mockRepo = new Mock<IUserRepository>();

        [TestMethod]
        public void given_username_exists_return_success_message()
        {
            var expectedResult = new ValidatorResult
            {
                Field = "Username",
                Message = "Success",
                IsValid = true
            };
            mockRepo.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            LoginValidator validator = new LoginValidator(mockRepo.Object);

            var result = validator.ValidateUsername("asdf");

            Assert.AreEqual(result.Field, expectedResult.Field);
            Assert.AreEqual(result.Message, expectedResult.Message);
            Assert.AreEqual(result.IsValid, expectedResult.IsValid);
        }
    }
}
