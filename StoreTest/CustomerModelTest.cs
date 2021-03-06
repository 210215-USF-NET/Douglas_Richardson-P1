using System;
using Xunit;
using StoreModels;
namespace StoreTest
{
    public class CustomerModelTest
    {
        /// <summary>
        /// Customer unit test 
        /// </summary>
        
        private Customer testCustomer = new Customer();
        [Fact]
        public void CustomerEmailShouldBeSet()
        {
            string testEmail = "aRealEmail@google";
            testCustomer.EmailAddress = testEmail;
            Assert.Equal(testEmail,testCustomer.EmailAddress);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("notarealemail")]
        public void CustomerEmailShouldNotBeEmpty(string testEmail){
            Assert.Throws<Exception>(() => testCustomer.EmailAddress = testEmail);
        }
    }
}
