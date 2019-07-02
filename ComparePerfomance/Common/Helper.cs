using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace Common
{
    public static class Helper
    {
        public static MemoryStream CreateFilledMemoryStream<T>() where T : new()
        {
            var builder = new DtoBuilder();
            var ins = builder.Create<T>();
            var json = JsonConvert.SerializeObject(ins, Formatting.None);
            var bytes = Encoding.UTF8.GetBytes(json);
            var memoryStream = new MemoryStream();
            memoryStream.Write(bytes, 0, bytes.Length);
            memoryStream.Flush();
            return memoryStream;
        }

        public static MemoryStream CreateFilledMemoryStream(Type type)
        {
            var builder = new DtoBuilder();
            var ins = builder.Create(type);
            var json = JsonConvert.SerializeObject(ins, Formatting.None);
            var bytes = Encoding.UTF8.GetBytes(json);
            var memoryStream = new MemoryStream();
            memoryStream.Write(bytes, 0, bytes.Length);
            memoryStream.Flush();
            return memoryStream;
        }

        public static void HeatUp()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var time = stopWatch.Elapsed;
            Assert.True(time.Ticks > -1);
            stopWatch.Stop();
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }

        public static void SaveLog(string testName, string log)
        {
            if (!_locks.TryGetValue(testName, out var lockObject))
            {
                lockObject = new object();
                if (!_locks.TryAdd(testName, lockObject))
                {
                    lockObject = _locks[testName];
                }
            }

            lock (lockObject)
            {
                var filePath = Path.Combine(_path, testName + ".txt");
                Directory.CreateDirectory(_path);
                File.AppendAllLines(filePath, new[] {log});
            }
        }

        private const string _path = @"C:\PerfomaceTests\";
        private static readonly ConcurrentDictionary<string, object> _locks = new ConcurrentDictionary<string, object>();
    }
}