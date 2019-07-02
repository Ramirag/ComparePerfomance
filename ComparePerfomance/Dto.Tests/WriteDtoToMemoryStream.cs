using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace Dto.Tests
{
    public class WriteDtoToMemoryStream
    {
        private ITestOutputHelper _testOutput;

        public WriteDtoToMemoryStream(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }
    }
}
