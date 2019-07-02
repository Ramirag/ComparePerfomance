using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common;
using Common.Dtos.Classes.Integers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Json.Tests
{
    public class JObjectTests
    {
        public static readonly IEnumerable<object[]> ArgumentsForReading = new List<object[]>
        {
            new object[] {typeof(ClassWith2Ints), nameof(ClassWith256Ints.Property1), 5, 1_000_000_000},
            new object[] {typeof(ClassWith2Ints), nameof(ClassWith256Ints.Property2), 5, 1_000_000_000}
        };

        public static readonly IEnumerable<object[]> ArgumentsForValidationOnNull = new List<object[]>
        {
            new object[] {typeof(ClassWith256Ints), nameof(ClassWith256Ints.Property1), 5, TimeSpan.FromSeconds(2)},
            new object[] {typeof(ClassWith256Ints), nameof(ClassWith256Ints.Property128), 5, TimeSpan.FromSeconds(2)},
            new object[] {typeof(ClassWith256Ints), nameof(ClassWith256Ints.Property256), 5, TimeSpan.FromSeconds(2)},
            new object[] {typeof(ClassWith2Ints), nameof(ClassWith256Ints.Property1), 5, TimeSpan.FromSeconds(2)},
            new object[] {typeof(ClassWith2Ints), nameof(ClassWith256Ints.Property2), 5, TimeSpan.FromSeconds(2)}
        };

        public JObjectTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Theory]
        [MemberData(nameof(ArgumentsForReading))]
        public void TestPerfomanceOnMultipleReadingToJObject(Type type, string parameterName, int repeatTimes, int objectsCount)
        {
            var jObjects = Enumerable.Range(0, objectsCount).Select(j => CreateJObjectForDto(type)).ToArray();
            HeatUpForReading(jObjects, parameterName);

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
            Helper.SaveLog($"{nameof(JObjectTests)}_{nameof(TestPerfomanceOnSingleReadingToJObject)}", message);
        }

        [Theory]
        [MemberData(nameof(ArgumentsForReading))]
        public void TestPerfomanceOnSingleReadingToJObject(Type type, string parameterName, int repeatTimes, int objectsCount)
        {
            var durations = new TimeSpan[repeatTimes];
            for (var i = 0; i < repeatTimes; i++)
            {
                var jObjects = Enumerable.Range(0, objectsCount).Select(j => CreateJObjectForDto(type)).ToArray();
                HeatUp();
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
            Helper.SaveLog($"{nameof(JObjectTests)}_{nameof(TestPerfomanceOnSingleReadingToJObject)}", message);
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

        private void HeatUp()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var time = stopWatch.Elapsed;
            Assert.True(time.Ticks > -1);
            stopWatch.Stop();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }

        private void HeatUpForReading(IEnumerable<JObject> jObjects, string parameterName)
        {
            jObjects.All(jObject =>
            {
                var value = jObject[parameterName].Value<string>();
                return value != null;
            });
            HeatUp();
        }

        private void HeatUpForValidationOnNull(JObject jObject, string parameterName)
        {
            var value = jObject[parameterName].Value<string>();
            Assert.NotNull(value);
            HeatUp();
        }
    }
}