using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Json.Tests
{
    public class ReadJObjectFromMemoryStream
    {
        public ReadJObjectFromMemoryStream(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Theory]
        [ClassData(typeof(ArgumentsForTestingReadingAndWritingFromMemoryStream))]
        public void Test(Type type, int repeatTimes, TimeSpan duration)
        {
            var memoryStream = Helper.CreateFilledMemoryStream(type);

            HeatUp(memoryStream);

            var counters = new int[repeatTimes];
            for (var i = 0; i < repeatTimes; i++)
            {
                var counter = 0;
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                while (stopWatch.Elapsed < duration)
                {
                    var instance = ReadJObject(memoryStream);
                    Assert.NotNull(instance);
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
            Helper.SaveLog($"{nameof(ReadJObjectFromMemoryStream)}", message);
        }

        private readonly ITestOutputHelper _testOutput;

        private void HeatUp(Stream memoryStream)
        {
            var instance = ReadJObject(memoryStream);
            Assert.NotNull(instance);
            Helper.HeatUp();
        }

        private JObject ReadJObject(Stream memoryStream)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            var textReader = new StreamReader(memoryStream, Encoding.UTF8);
            var jsonReader = new JsonTextReader(textReader);
            var instance = JObject.Load(jsonReader);
            return instance;
        }
    }
}