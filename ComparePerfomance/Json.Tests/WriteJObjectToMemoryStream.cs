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
    public class WriteJObjectToMemoryStream
    {
        public WriteJObjectToMemoryStream(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Theory]
        [ClassData(typeof(ArgumentsForTestingReadingAndWritingFromMemoryStream))]
        public void Test(Type type, int repeatTimes, TimeSpan duration)
        {
            var builder = new DtoBuilder();
            var ins = builder.Create(type);
            var json = JsonConvert.SerializeObject(ins, Formatting.None);
            var jObject = JObject.Parse(json);

            HeatUp(jObject);

            var counters = new int[repeatTimes];
            for (var i = 0; i < repeatTimes; i++)
            {
                var counter = 0;
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                while (stopWatch.Elapsed < duration)
                {
                    var memoryStream = WriteJObject(jObject);
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
            Helper.SaveLog($"{nameof(WriteJObjectToMemoryStream)}", message);
        }

        private readonly ITestOutputHelper _testOutput;

        private void HeatUp(JObject jObject)
        {
            var memoryStream = WriteJObject(jObject);
            Assert.NotNull(memoryStream);
            Helper.HeatUp();
        }

        private MemoryStream WriteJObject(JObject jObject)
        {
            var memoryStream = new MemoryStream();
            var textWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            var jsonWriter = new JsonTextWriter(textWriter);
            jObject.WriteTo(jsonWriter);
            memoryStream.Flush();
            return memoryStream;
        }
    }
}