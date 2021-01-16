using System;
using Xunit;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace tests
{
    public class ConsumerPactClassFixture : IDisposable
    {
        //This class is used for setting up a shared mock server
        //for pact used by all the tests
        public IPactBuilder PactBuilder {get; private set;}
        public IMockProviderService MockProviderService {get; private set;}

        public int MockServerPort  => 9220;
        public string MockProviderServiceBaseUri => String.Format("Http://localhost:{0}", MockServerPort);

        #region Constructor

        public ConsumerPactClassFixture()
        {
            //Create Pact config
          var pactConfig = new PactConfig{
             SpecificationVersion = "2.0.0",
             PactDir = @"..\..\..\..\..\pacts",
             LogDir = @".\pacts_logs"
          };
          
          
          PactBuilder = new PactBuilder(pactConfig);

          PactBuilder.ServiceConsumer("Consumer").HasPactWith("Provider");

          //Create an instance of the MockService using the config
          MockProviderService = PactBuilder.MockService(MockServerPort);
            
        }

        #endregion
       
       

       #region Dispose Support

       private bool disposedValue = false;

       protected virtual void Dispose(bool disposing)
       {
           if (!disposedValue)
            {
                if (disposing)
                {
                    // This teardown the Mock server and generate the pact file.
                    PactBuilder.Build();
                }

                disposedValue = true;
            }
        }
        #endregion
        public void Dispose()
        {
            Dispose(true);
        }
      
    }

}