using System;
using Xunit;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Consumer;
using System.Collections.Generic;

namespace tests
{
    public class ConsumerPactTests : IClassFixture<ConsumerPactClassFixture>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        #region Constructor

        public ConsumerPactTests(ConsumerPactClassFixture fixture)
        { 
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions();
            _mockProviderServiceBaseUri = fixture.MockProviderServiceBaseUri;

        }
        #endregion

       [Fact]
        public void ItCanHandleValidDate()
        {
           //Arrange
           var expectedValidMessage = "validDateTime";
           _mockProviderService
           .Given("There is a date")
           .UponReceiving("A valid GET Request for Date Validation with valid date")
           .With(new ProviderServiceRequest{
               Method = HttpVerb.Get,
               Path = "/api/provider",
               Query="validDateTime=01-01-2012"
           })
           .WillRespondWith(new ProviderServiceResponse{
               Status = 200,
               Headers = new Dictionary<string, object>{
                   {"Content-Type", "application/json; charset=utf-8"}
               },
               Body= new{
                   message= expectedValidMessage
               }
           });

           //Act 
           var result = ConsumerApiClient.ValidateDateTimeUsingProviderApi("01-01-2012",_mockProviderServiceBaseUri).GetAwaiter().GetResult();
           var resultBodyText= result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

           //Assert 
           Assert.Contains(expectedValidMessage,resultBodyText);
        }
          
        #region Tests
        [Fact]
        public void ItCanHandleInvalidDate()
        {
           //Arrange
           var expectedInvalidMessage = "validDateTime is not a date or time";
           _mockProviderService
           .Given("There is a date")
           .UponReceiving("An Invalid GET Request for Date Validation with invalid date")
           .With(new ProviderServiceRequest{
               Method = HttpVerb.Get,
               Path = "/api/provider",
               Query="validDateTime=TechnicalMeeting"
           })
           .WillRespondWith(new ProviderServiceResponse{
               Status = 400,
               Headers = new Dictionary<string, object>{
                   {"Content-Type", "application/json; charset=utf-8"}
               },
               Body= new{
                   message= expectedInvalidMessage
               }
           });

           //Act 
           var result = ConsumerApiClient.ValidateDateTimeUsingProviderApi("TechnicalMeeting",_mockProviderServiceBaseUri)
                .GetAwaiter()
                .GetResult();
           var resultBodyText= result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

           //Assert 
           Assert.Contains(expectedInvalidMessage,resultBodyText);
        }

       
        #endregion
    }
}
