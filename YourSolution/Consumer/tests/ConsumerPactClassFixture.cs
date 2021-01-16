namespace tests
{
    public class ConsumerPactClassFixture : IDisposable
    {
        //This class is used for setting up a shared mock server
        //for pact used by all the tests
        public IPactBuilder PactBuilder {get; private set;}
        public IMockServiceProvider MockServiceProvider {get; private set;}

        public int MockServerPort  => 9220;
        public int MockProviderServiceBaseUri => String.Format("Http://localhost:{0}", MockServerPort);

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
          MockServiceProvider = PactBuilder.MockService(MockServerPort);
            
        }

        #endregion
       
       Dispose(true);

       #region Dispose Support

       private bool disposeValue = false;

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

       }

      #endregion

    }
}