using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class DtoBuilder
    {
        public DtoBuilder()
        {
            int ticks;
            unchecked
            {
                ticks = (int) DateTime.Now.Ticks;
            }

            _random = new Random(ticks);
        }

        public T Create<T>() where T : new()
        {
            var type = typeof(T);

            return (T) Create(type);
        }

        public object Create(Type type)
        {
            var instance = Activator.CreateInstance(type);
            var properties = type.GetProperties();
            foreach (var propertyInfo in properties)
            {
                var propertyType = propertyInfo.PropertyType;
                if (propertyType.IsArray)
                {
                    propertyInfo.SetValue(instance, CreateArray(propertyType));
                    continue;
                }

                switch (propertyType.Name)
                {
                    case "Int32":
                        propertyInfo.SetValue(instance, CreateInt());
                        break;
                    case "String":
                        propertyInfo.SetValue(instance, CreateString());
                        break;
                    case "List`1":
                        propertyInfo.SetValue(instance, CreateList(propertyType));
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported type: {propertyType}");
                }
            }

            return instance;
        }

        private readonly Random _random;

        private object CreateArray(Type propertyType)
        {
            const int min = 16;
            const int max = 32;
            var count = _random.Next(min, max);
            switch (propertyType.Name)
            {
                case "Int32[]":
                    var intList = new int[count];
                    FillList(intList, CreateInt);
                    return intList;
                case "String[]":
                    var stringList = new string[count];
                    FillList(stringList, CreateString);
                    return stringList;
                default:
                    throw new InvalidOperationException($"Unsupported type: {propertyType}");
            }
        }

        private int CreateInt()
        {
            return _random.Next(int.MaxValue / 2, int.MaxValue);
        }

        private object CreateList(Type propertyType)
        {
            const int min = 16;
            const int max = 32;
            var count = _random.Next(min, max);
            var genericType = propertyType.GenericTypeArguments[0];
            switch (genericType.Name)
            {
                case "Int32":
                    var intList = new List<int>(new int[count]);
                    FillList(intList, CreateInt);
                    return intList;
                case "String":
                    var stringList = new List<string>(new string[count]);
                    FillList(stringList, CreateString);
                    return stringList;
                default:
                    throw new InvalidOperationException($"Unsupported type: {propertyType}");
            }
        }

        private string CreateString()
        {
            const int min = 16;
            const int max = 32;
            var length = _random.Next(min, max);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        private void FillList<T>(IList<T> list, Func<T> builder)
        {
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = builder();
            }
        }
    }
}