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
        private readonly ITestOutputHelper _testOutput;

        public ReadDtoFromMemoryStream(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { typeof(ClassWith256Ints), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith128Ints), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith64Ints), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith32Ints), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith16Ints), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith8Ints), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith4Ints), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith2Ints), 6, TimeSpan.FromSeconds(10) },

                new object[] { typeof(ClassWith256Strings), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith128Strings), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith64Strings), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith32Strings), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith16Strings), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith8Strings), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith4Strings), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith2Strings), 6, TimeSpan.FromSeconds(10) },

                new object[] { typeof(ClassWith256Lists<int>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith128Lists<int>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith64Lists<int>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith32Lists<int>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith16Lists<int>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith8Lists<int>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith4Lists<int>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith2Lists<int>), 6, TimeSpan.FromSeconds(10) },

                new object[] { typeof(ClassWith256Lists<string>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith128Lists<string>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith64Lists<string>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith32Lists<string>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith16Lists<string>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith8Lists<string>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith4Lists<string>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith2Lists<string>), 6, TimeSpan.FromSeconds(10) },

                new object[] { typeof(ClassWith256Arrays<int>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith128Arrays<int>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith64Arrays<int>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith32Arrays<int>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith16Arrays<int>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith8Arrays<int>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith4Arrays<int>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith2Arrays<int>), 6, TimeSpan.FromSeconds(10) },

                new object[] { typeof(ClassWith256Arrays<string>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith128Arrays<string>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith64Arrays<string>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith32Arrays<string>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith16Arrays<string>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith8Arrays<string>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith4Arrays<string>), 6, TimeSpan.FromSeconds(10) },
                new object[] { typeof(ClassWith2Arrays<string>), 6, TimeSpan.FromSeconds(10) },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Test(Type type, int repeatTimes, TimeSpan duration)
        {
            var memoryStream = Helper.CreateFilledMemoryStream(type);

            //HeatUp
            HeatUp(memoryStream, type);

            var counters = new int[repeatTimes];
            for (var i = 0; i < repeatTimes; i++)
            {
                var counter = 0;
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                while (stopWatch.Elapsed < duration)
                {
                    var instance = ReadInstanceFromMemoryStream(memoryStream, type);
                    Assert.NotNull(instance);
                    counter++;
                }

                stopWatch.Stop();
                counters[i] = counter;
            }

            var message = $"Test for {type} repeted {repeatTimes} times, each took {duration}. Min: {counters.Min()} Max: {counters.Max()} Avg: {counters.Average()}";
            _testOutput.WriteLine(message);
            Helper.SaveLog($"{nameof(ReadDtoFromMemoryStream)}", message);
        }

        private void HeatUp(Stream memoryStream, Type type)
        {
            var instance = ReadInstanceFromMemoryStream(memoryStream, type);
            Assert.NotNull(instance);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var time = stopWatch.Elapsed;
            Assert.True(time.Ticks > -1);
            stopWatch.Stop();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }

        private object ReadInstanceFromMemoryStream(Stream memoryStream, Type type)
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
