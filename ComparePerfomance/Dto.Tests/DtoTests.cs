using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common;
using Common.Dtos.Classes.Integers;
using Xunit;
using Xunit.Abstractions;

namespace Dto.Tests
{
    public class DtoTests
    {
        public static readonly IEnumerable<object[]> ArgumentsForValidationOnNull = new List<object[]>
        {
            new object[] {5, TimeSpan.FromSeconds(1)}
        };

        public DtoTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Theory]
        [MemberData(nameof(ArgumentsForValidationOnNull))]
        public void TestPerfomanceOnReadingToDto(int repeatTimes, TimeSpan duration)
        {
            var type = typeof(ClassWith256Ints);
            var parameterName = nameof(ClassWith256Ints.Property256);

            var builder = new DtoBuilder();
            var instance = builder.Create<ClassWith256Ints>();
            HeatUp(instance);

            var counters = new int[repeatTimes];
            for (var i = 0; i < repeatTimes; i++)
            {
                var counter = 0;
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                while (stopWatch.Elapsed < duration)
                {
                    var value = instance.Property256;
                    Assert.True(value > -1);
                    counter++;
                }

                stopWatch.Stop();
                counters[i] = counter;
            }

            var min = counters.Min();
            var max = counters.Max();
            var avg = counters.Average();
            var diff = (double) (max - min) / min * 100;
            var message = $"Test for {type} parameter {parameterName} repeted {repeatTimes} times, each took {duration}. Min: {min} Max: {max} Diff: {diff} Avg: {avg}";
            _testOutput.WriteLine(message);
            Helper.SaveLog($"{nameof(DtoTests)}_{nameof(TestPerfomanceOnReadingToDto)}", message);
        }

        private readonly ITestOutputHelper _testOutput;

        private void HeatUp(ClassWith256Ints instance)
        {
            var value = instance.Property256;
            Assert.True(value > -1);
            Helper.HeatUp();
        }
    }
}