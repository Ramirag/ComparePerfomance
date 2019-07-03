using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Common;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Dto.Tests
{
    public class WriteDtoToMemoryStream
    {
        public WriteDtoToMemoryStream(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Theory]
        [ClassData(typeof(ArgumentsForTestingReadingAndWritingFromMemoryStream))]
        public void Test(Type type, int repeatTimes, TimeSpan duration)
        {
            var builder = new DtoBuilder();
            var ins = builder.Create(type);

            HeatUp(ins);

            var counters = new int[repeatTimes];
            for (var i = 0; i < repeatTimes; i++)
            {
                var counter = 0;
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                while (stopWatch.Elapsed < duration)
                {
                    var memoryStream = WriteDto(ins);
                    Assert.True(memoryStream.Length > 0);
                    counter++;
                }

                stopWatch.Stop();
                counters[i] = counter;
            }

            var min = counters.Min();
            var max = counters.Max();
            var avg = counters.Average();
            var diff = (double) (max - min) / min * 100;
            var message = $"Test for {type} repeted {repeatTimes} times, each took {duration}. Min: {min} Max: {max} Diff: {diff} Avg: {avg}";
            _testOutput.WriteLine(message);
            Helper.SaveLog($"{nameof(WriteDtoToMemoryStream)}", message);
        }

        private readonly ITestOutputHelper _testOutput;

        private void HeatUp(object ins)
        {
            var memoryStream = WriteDto(ins);
            Assert.NotNull(memoryStream);
            Helper.HeatUp();
        }

        private MemoryStream WriteDto(object ins)
        {
            var json = JsonConvert.SerializeObject(ins, Formatting.None);
            var bytes = Encoding.UTF8.GetBytes(json);
            var memoryStream = new MemoryStream();
            memoryStream.Write(bytes, 0, bytes.Length);
            memoryStream.Flush();
            return memoryStream;
        }
    }
}