using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Common;
using Common.Dtos.Classes.Arrays;
using Common.Dtos.Classes.Integers;
using Common.Dtos.Classes.Lists;
using Common.Dtos.Classes.Strings;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Dto.Tests
{
    public class ReadDtoFromMemoryStream
    {
        public ReadDtoFromMemoryStream(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new List<object[]>
                {
                    new object[] {typeof(ClassWith256Ints), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith128Ints), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith64Ints), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith32Ints), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith16Ints), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith8Ints), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith4Ints), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith2Ints), 5, TimeSpan.FromSeconds(1)},

                    new object[] {typeof(ClassWith256Strings), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith128Strings), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith64Strings), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith32Strings), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith16Strings), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith8Strings), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith4Strings), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith2Strings), 5, TimeSpan.FromSeconds(1)},

                    new object[] {typeof(ClassWith256Lists<int>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith128Lists<int>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith64Lists<int>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith32Lists<int>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith16Lists<int>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith8Lists<int>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith4Lists<int>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith2Lists<int>), 5, TimeSpan.FromSeconds(1)},

                    new object[] {typeof(ClassWith256Lists<string>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith128Lists<string>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith64Lists<string>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith32Lists<string>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith16Lists<string>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith8Lists<string>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith4Lists<string>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith2Lists<string>), 5, TimeSpan.FromSeconds(1)},

                    new object[] {typeof(ClassWith256Arrays<int>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith128Arrays<int>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith64Arrays<int>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith32Arrays<int>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith16Arrays<int>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith8Arrays<int>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith4Arrays<int>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith2Arrays<int>), 5, TimeSpan.FromSeconds(1)},

                    new object[] {typeof(ClassWith256Arrays<string>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith128Arrays<string>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith64Arrays<string>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith32Arrays<string>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith16Arrays<string>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith8Arrays<string>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith4Arrays<string>), 5, TimeSpan.FromSeconds(1)},
                    new object[] {typeof(ClassWith2Arrays<string>), 5, TimeSpan.FromSeconds(1)}
                };
            }
        }

        [Theory]
        [ClassData(typeof(ArgumentsForTestingReadingAndWritingFromMemoryStream))]
        public void Test(Type type, int repeatTimes, TimeSpan duration)
        {
            var memoryStream = Helper.CreateFilledMemoryStream(type);

            HeatUp(memoryStream, type);

            var counters = new int[repeatTimes];
            for (var i = 0; i < repeatTimes; i++)
            {
                var counter = 0;
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                while (stopWatch.Elapsed < duration)
                {
                    var instance = ReadDto(memoryStream, type);
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
            Helper.SaveLog($"{nameof(ReadDtoFromMemoryStream)}", message);
        }

        private readonly ITestOutputHelper _testOutput;

        private void HeatUp(Stream memoryStream, Type type)
        {
            var instance = ReadDto(memoryStream, type);
            Assert.NotNull(instance);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var time = stopWatch.Elapsed;
            Assert.True(time.Ticks > -1);
            stopWatch.Stop();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }

        private object ReadDto(Stream memoryStream, Type type)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            var bytes = new byte[memoryStream.Length];
            memoryStream.Read(bytes, 0, bytes.Length);
            var json = Encoding.UTF8.GetString(bytes);
            var instance = JsonConvert.DeserializeObject(json, type);
            return instance;
        }
    }
}