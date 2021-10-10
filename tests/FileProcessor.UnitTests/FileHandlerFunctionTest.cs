using Autofac.Extras.Moq;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FileProcessor.UnitTests
{
   public class FileHandlerFunctionTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public void Test_FraudCheck_ValidInputs()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.MockRepository.DefaultValue = DefaultValue.Mock;
               
               
            }

        }
    }
}
