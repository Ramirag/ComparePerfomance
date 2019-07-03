using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common;
using Common.Dtos.Classes.Integers;
using Common.Dtos.Classes.Strings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Json.Tests
{
    public class JObjectTests
    {
        public static readonly IEnumerable<object[]> ArgumentsForForPossibleCaching = new List<object[]>
        {
            new object[] {typeof(ClassWith2Ints), nameof(ClassWith256Ints.Property1), 5, 1_000_000},
            new object[] {typeof(ClassWith2Ints), nameof(ClassWith256Ints.Property2), 5, 1_000_000}
        };

        public static readonly IEnumerable<object[]> ArgumentsForValidationOnNull = new List<object[]>
        {
            new object[] {typeof(ClassWith256Ints), nameof(ClassWith256Ints.Property1), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith256Ints), nameof(ClassWith256Ints.Property128), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith256Ints), nameof(ClassWith256Ints.Property256), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith128Ints), nameof(ClassWith256Ints.Property1), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith128Ints), nameof(ClassWith256Ints.Property64), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith128Ints), nameof(ClassWith256Ints.Property128), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith2Ints), nameof(ClassWith256Ints.Property1), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith2Ints), nameof(ClassWith256Ints.Property2), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith256Strings), nameof(ClassWith256Strings.Property1), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith256Strings), nameof(ClassWith256Strings.Property128), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith256Strings), nameof(ClassWith256Strings.Property256), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith128Strings), nameof(ClassWith256Strings.Property1), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith128Strings), nameof(ClassWith256Strings.Property64), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith128Strings), nameof(ClassWith256Strings.Property128), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith2Strings), nameof(ClassWith256Strings.Property1), 5, TimeSpan.FromSeconds(1)},
            new object[] {typeof(ClassWith2Strings), nameof(ClassWith256Strings.Property2), 5, TimeSpan.FromSeconds(1)},
        };

        public JObjectTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Theory(Skip = "There is no difference in perfomance between TestPerfomanceOnMultipleReadingToJObject and TestPerfomanceOnSingleReadingToJObject. But it possible can be. Better to read source.")]
        [MemberData(nameof(ArgumentsForForPossibleCaching))]
        public void TestPerfomanceForPossibleCachingOnMultipleReadingToJObject(Type type, string parameterName, int repeatTimes, int objectsCount)
        {
            var jObjects = Enumerable.Range(0, objectsCount).Select(j => CreateJObjectForDto(type)).ToArray();
            HeatUpForPossibleCaching(jObjects, parameterName);

            var durations = new TimeSpan[repeatTimes];
            for (var i = 0; i < repeatTimes; i++)
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                foreach (var jObject in jObjects)
                {
                    var value = jObject[parameterName].Value<string>();
                    Assert.NotNull(value);
                }

                stopWatch.Stop();
                durations[i] = stopWatch.Elapsed;
            }

            var min = durations.Min();
            var max = durations.Max();
            var avg = new TimeSpan((long) durations.Select(i => i.Ticks).Average());
            var diff = (double) (max.Ticks - min.Ticks) / min.Ticks * 100;
            var message = $"Test for {type} parameter {parameterName} repeted {repeatTimes} times, created {objectsCount} instances. Min: {min} Max: {max} Diff: {diff} Avg: {avg}";
            _testOutput.WriteLine(message);
            Helper.SaveLog($"{nameof(JObjectTests)}_{nameof(TestPerfomanceForPossibleCachingOnMultipleReadingToJObject)}", message);
        }

        [Theory(Skip = "There is no difference in perfomance between TestPerfomanceOnMultipleReadingToJObject and TestPerfomanceOnSingleReadingToJObject. But it possible can be. Better to read source.")]
        [MemberData(nameof(ArgumentsForForPossibleCaching))]
        public void TestPerfomanceForPossibleCachingOnSingleReadingToJObject(Type type, string parameterName, int repeatTimes, int objectsCount)
        {
            var durations = new TimeSpan[repeatTimes];
            for (var i = 0; i < repeatTimes; i++)
            {
                var jObjects = Enumerable.Range(0, objectsCount).Select(j => CreateJObjectForDto(type)).ToArray();
                Helper.HeatUp();
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                foreach (var jObject in jObjects)
                {
                    var value = jObject[parameterName].Value<string>();
                    Assert.NotNull(value);
                }

                stopWatch.Stop();
                durations[i] = stopWatch.Elapsed;
            }

            var min = durations.Min();
            var max = durations.Max();
            var avg = new TimeSpan((long) durations.Select(i => i.Ticks).Average());
            var diff = (double) (max.Ticks - min.Ticks) / min.Ticks * 100;
            var message = $"Test for {type} parameter {parameterName} repeted {repeatTimes} times, created {objectsCount} instances. Min: {min} Max: {max} Diff: {diff} Avg: {avg}";
            _testOutput.WriteLine(message);
            Helper.SaveLog($"{nameof(JObjectTests)}_{nameof(TestPerfomanceForPossibleCachingOnSingleReadingToJObject)}", message);
        }

        [Theory]
        [MemberData(nameof(ArgumentsForValidationOnNull))]
        public void TestPerfomanceOnReadingToJObject(Type type, string parameterName, int repeatTimes, TimeSpan duration)
        {
            var jObject = CreateJObjectForDto(type);
            HeatUpForValidationOnNull(jObject, parameterName);

            var counters = new int[repeatTimes];
            for (var i = 0; i < repeatTimes; i++)
            {
                var counter = 0;
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                while (stopWatch.Elapsed < duration)
                {
                    var value = jObject[parameterName].Value<string>();
                    Assert.NotNull(value);
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
            Helper.SaveLog($"{nameof(JObjectTests)}_{nameof(TestPerfomanceOnReadingToJObject)}", message);
        }

        [Theory]
        [MemberData(nameof(ArgumentsForValidationOnNull))]
        public void TestPerfomanceOnValidationTokenOnNullWithAditionalField(Type type, string parameterName, int repeatTimes, TimeSpan duration)
        {
            var jObject = CreateJObjectForDto(type);

            HeatUpForValidationOnNull(jObject, parameterName);

            var counters = new int[repeatTimes];
            for (var i = 0; i < repeatTimes; i++)
            {
                var counter = 0;
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                while (stopWatch.Elapsed < duration)
                {
                    var jToken = jObject[parameterName];
                    if (jToken != null)
                    {
                        var value = jToken.Value<string>();
                        Assert.NotNull(value);
                    }

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
            Helper.SaveLog($"{nameof(JObjectTests)}_{nameof(TestPerfomanceOnValidationTokenOnNullWithAditionalField)}", message);
        }

        [Theory]
        [MemberData(nameof(ArgumentsForValidationOnNull))]
        public void TestPerfomanceOnValidationTokenOnNullWithoutAditionalField(Type type, string parameterName, int repeatTimes, TimeSpan duration)
        {
            var jObject = CreateJObjectForDto(type);

            HeatUpForValidationOnNull(jObject, parameterName);

            var counters = new int[repeatTimes];
            for (var i = 0; i < repeatTimes; i++)
            {
                var counter = 0;
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                while (stopWatch.Elapsed < duration)
                {
                    if (jObject[parameterName] != null)
                    {
                        var value = jObject[parameterName].Value<string>();
                        Assert.NotNull(value);
                    }

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
            Helper.SaveLog($"{nameof(JObjectTests)}_{nameof(TestPerfomanceOnValidationTokenOnNullWithoutAditionalField)}", message);
        }

        private readonly ITestOutputHelper _testOutput;

        private JObject CreateJObjectForDto(Type type)
        {
            var builder = new DtoBuilder();
            var instance = builder.Create(type);
            var json = JsonConvert.SerializeObject(instance);
            return JObject.Parse(json);
        }
        
        private void HeatUpForPossibleCaching(IEnumerable<JObject> jObjects, string parameterName)
        {
            jObjects.All(jObject =>
            {
                var value = jObject[parameterName].Value<string>();
                return value != null;
            });
            Helper.HeatUp();
        }

        private void HeatUpForValidationOnNull(JObject jObject, string parameterName)
        {
            var value = jObject[parameterName].Value<string>();
            Assert.NotNull(value);
            Helper.HeatUp();
        }
    }
}