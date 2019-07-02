using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Dtos.Classes.Arrays;
using Common.Dtos.Classes.Integers;
using Common.Dtos.Classes.Lists;
using Common.Dtos.Classes.Strings;
using Xunit;

namespace Dto.Tests
{
    public class DtoBuilderTests
    {
        [Fact]
        public void Create_ClassWithArrays_InstanceFilled()
        {
            var builder = new DtoBuilder();
            var ins = builder.Create<ClassWith256Arrays<int>>();

            Assert.NotNull(ins);
            Assert.True(IsInstanceWithArrysValid<ClassWith256Arrays<int>, int>(ins, IsIntValid));
        }

        [Fact]
        public void Create_ClassWithInts_InstanceFilled()
        {
            var builder = new DtoBuilder();
            var ins = builder.Create<ClassWith256Ints>();

            Assert.NotNull(ins);
            Assert.True(IsInstanceValid<ClassWith256Ints, int>(ins, IsIntValid));
        }

        [Fact]
        public void Create_ClassWithLists_InstanceFilled()
        {
            var builder = new DtoBuilder();
            var ins = builder.Create<ClassWith256Lists<int>>();

            Assert.NotNull(ins);
            Assert.True(IsInstanceWithArrysValid<ClassWith256Lists<int>, int>(ins, IsIntValid));
        }

        [Fact]
        public void Create_ClassWithStrings_InstanceFilled()
        {
            var builder = new DtoBuilder();
            var ins = builder.Create<ClassWith256Strings>();

            Assert.NotNull(ins);
            Assert.True(IsInstanceValid<ClassWith256Strings, string>(ins, IsStringValid));
        }

        private bool IsInstanceValid<T1, T2>(T1 instance, Func<T2, bool> validator)
        {
            var type = typeof(T1);
            var properties = type.GetProperties();
            return properties.Select(property => (T2) property.GetValue(instance)).All(validator);
        }

        private bool IsInstanceWithArrysValid<T1, T2>(T1 instance, Func<T2, bool> validator)
        {
            var type = typeof(T1);
            var properties = type.GetProperties();
            return properties.SelectMany(property => (IEnumerable<T2>) property.GetValue(instance)).All(validator);
        }

        private bool IsIntValid(int value)
        {
            return value != 0;
        }

        private bool IsStringValid(string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}