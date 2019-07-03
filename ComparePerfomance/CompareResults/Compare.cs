using System.IO;
using System.Linq;
using Common;
using Dto.Tests;
using Json.Tests;
using Xunit;

namespace CompareResults
{
    public class Compare
    {
        private class CompareKey
        {
            public CompareKey(string className, string propertyName)
            {
                ClassName = className;
                PropertyName = propertyName;
            }
            public string ClassName { get; }
            public string PropertyName { get; }

            public override int GetHashCode()
            {
                return ClassName.GetHashCode() + PropertyName.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var key = (CompareKey)obj;

                return ClassName == key.ClassName && PropertyName == key.PropertyName;
            }
        }
        private class CompareData
        {
            public int Min { get; set; }
            public int Max { get; set; }
            public double Avg { get; set; }
        }

        private double CalculateDifference(int a, int b)
        {
            return (a - b) / (double)b * 100;
        }

        private double CalculateDifference(double a, double b)
        {
            return (a - b) / b * 100;
        }

        [Fact]
        public void CompareReadDtoFromMemoryStreamWithReadJObjectFromMemoryStream()
        {
            var firstFileName = nameof(ReadDtoFromMemoryStream) + ".txt";
            var secondFileName = nameof(ReadJObjectFromMemoryStream) + ".txt";

            var firstFileLines = File.ReadAllLines(Path.Combine(Helper.Path, firstFileName));
            var secondFileLines = File.ReadAllLines(Path.Combine(Helper.Path, secondFileName));

            var firstFileDictionary = firstFileLines.Select(i => i.Split(" ")).ToDictionary(i => i[2], i => new CompareData()
            {
                Min = int.Parse(i[10]),
                Max = int.Parse(i[12]),
                Avg = double.Parse(i[16]),
            });

            var secondFileDictionary = secondFileLines.Select(i => i.Split(" ")).ToDictionary(i => i[2], i => new CompareData()
            {
                Min = int.Parse(i[10]),
                Max = int.Parse(i[12]),
                Avg = double.Parse(i[16]),
            });

            foreach (var keyValuePair in firstFileDictionary)
            {
                if (!secondFileDictionary.TryGetValue(keyValuePair.Key, out var secondFileDate))
                {
                    continue;
                }

                var min = CalculateDifference(keyValuePair.Value.Min, secondFileDate.Min);
                var max = CalculateDifference(keyValuePair.Value.Max, secondFileDate.Max);
                var avg = CalculateDifference(keyValuePair.Value.Avg, secondFileDate.Avg);

                Helper.SaveLog($"{nameof(Compare)}_ {nameof(ReadDtoFromMemoryStream)}_{nameof(ReadJObjectFromMemoryStream)}",
                    $"{keyValuePair.Key} Min: {min} Max: {max} Avg:{avg}");
            }
        }

        [Fact]
        public void CompareWriteDtoToMemoryStreamWithWriteJObjectToMemoryStream()
        {
            var firstFileName = nameof(WriteDtoToMemoryStream) + ".txt";
            var secondFileName = nameof(WriteJObjectToMemoryStream) + ".txt";

            var firstFileLines = File.ReadAllLines(Path.Combine(Helper.Path, firstFileName));
            var secondFileLines = File.ReadAllLines(Path.Combine(Helper.Path, secondFileName));

            var firstFileDictionary = firstFileLines.Select(i => i.Split(" ")).ToDictionary(i => i[2], i => new CompareData()
            {
                Min = int.Parse(i[10]),
                Max = int.Parse(i[12]),
                Avg = double.Parse(i[16]),
            });

            var secondFileDictionary = secondFileLines.Select(i => i.Split(" ")).ToDictionary(i => i[2], i => new CompareData()
            {
                Min = int.Parse(i[10]),
                Max = int.Parse(i[12]),
                Avg = double.Parse(i[16]),
            });

            foreach (var keyValuePair in firstFileDictionary)
            {
                if (!secondFileDictionary.TryGetValue(keyValuePair.Key, out var secondFileDate))
                {
                    continue;
                }

                var min = CalculateDifference(keyValuePair.Value.Min, secondFileDate.Min);
                var max = CalculateDifference(keyValuePair.Value.Max, secondFileDate.Max);
                var avg = CalculateDifference(keyValuePair.Value.Avg, secondFileDate.Avg);

                Helper.SaveLog($"{nameof(Compare)}_ {nameof(WriteDtoToMemoryStream)}_{nameof(WriteJObjectToMemoryStream)}",
                    $"{keyValuePair.Key} Min: {min} Max: {max} Avg:{avg}");
            }
        }

        [Fact]
        public void CompareTestPerfomanceOnValidationTokenOnNullWithAditionalFieldWithTestPerfomanceOnValidationTokenOnNullWithoutAditionalField()
        {
            var firstFileName = $"{nameof(JObjectTests)}_{nameof(JObjectTests.TestPerfomanceOnValidationTokenOnNullWithAditionalField)}" + ".txt";
            var secondFileName = $"{nameof(JObjectTests)}_{nameof(JObjectTests.TestPerfomanceOnValidationTokenOnNullWithoutAditionalField)}" + ".txt";

            var firstFileLines = File.ReadAllLines(Path.Combine(Helper.Path, firstFileName));
            var secondFileLines = File.ReadAllLines(Path.Combine(Helper.Path, secondFileName));

            var firstFileDictionary = firstFileLines.Select(i => i.Split(" ")).ToDictionary(
                i => new CompareKey(i[2], i[4]),
                i => new CompareData
                {
                    Min = int.Parse(i[12]),
                    Max = int.Parse(i[14]),
                    Avg = double.Parse(i[18]),
                });

            var secondFileDictionary = secondFileLines.Select(i => i.Split(" ")).ToDictionary(i => new CompareKey(i[2], i[4]),
                i => new CompareData
                {
                    Min = int.Parse(i[12]),
                    Max = int.Parse(i[14]),
                    Avg = double.Parse(i[18]),
                });

            foreach (var keyValuePair in firstFileDictionary)
            {
                if (!secondFileDictionary.TryGetValue(keyValuePair.Key, out var secondFileData))
                {
                    continue;
                }

                var min = CalculateDifference(keyValuePair.Value.Min, secondFileData.Min);
                var max = CalculateDifference(keyValuePair.Value.Max, secondFileData.Max);
                var avg = CalculateDifference(keyValuePair.Value.Avg, secondFileData.Avg);

                Helper.SaveLog($"{nameof(Compare)}_ {nameof(JObjectTests.TestPerfomanceOnValidationTokenOnNullWithAditionalField)}_{nameof(JObjectTests.TestPerfomanceOnValidationTokenOnNullWithoutAditionalField)}",
                    $"{keyValuePair.Key.ClassName} Property: {keyValuePair.Key.PropertyName} Min: {min} Max: {max} Avg:{avg}");
            }
        }

        [Fact]
        public void CompareTestPerfomanceOnReadingToDtoWithTestPerfomanceOnReadingToJObject()
        {
            var firstFileName = $"{nameof(DtoTests)}_{nameof(DtoTests.TestPerfomanceOnReadingToDto)}" + ".txt";
            var secondFileName = $"{nameof(JObjectTests)}_{nameof(JObjectTests.TestPerfomanceOnReadingToJObject)}" + ".txt";

            var firstFileLines = File.ReadAllLines(Path.Combine(Helper.Path, firstFileName));
            var secondFileLines = File.ReadAllLines(Path.Combine(Helper.Path, secondFileName));

            var firstFileParts = firstFileLines.First().Split(" ");
            var firstFileData = new CompareData
            {
                Min = int.Parse(firstFileParts[12]),
                Max = int.Parse(firstFileParts[14]),
                Avg = double.Parse(firstFileParts[18]),
            };

            var secondFileDictionary = secondFileLines.Select(i => i.Split(" ")).ToDictionary(i => new CompareKey(i[2], i[4]),
                i => new CompareData
                {
                    Min = int.Parse(i[12]),
                    Max = int.Parse(i[14]),
                    Avg = double.Parse(i[18]),
                });

            foreach (var keyValuePair in secondFileDictionary)
            {
                var min = CalculateDifference(firstFileData.Min, keyValuePair.Value.Min);
                var max = CalculateDifference(firstFileData.Max, keyValuePair.Value.Max);
                var avg = CalculateDifference(firstFileData.Avg, keyValuePair.Value.Avg);

                Helper.SaveLog($"{nameof(Compare)}_ {nameof(DtoTests.TestPerfomanceOnReadingToDto)}_{nameof(JObjectTests.TestPerfomanceOnReadingToJObject)}",
                    $"{keyValuePair.Key.ClassName} Property: {keyValuePair.Key.PropertyName} Min: {min} Max: {max} Avg:{avg}");
            }
        }
    }
}
