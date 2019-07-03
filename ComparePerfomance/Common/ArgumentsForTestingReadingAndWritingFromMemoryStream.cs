using System;
using System.Collections;
using System.Collections.Generic;
using Common.Dtos.Classes.Arrays;
using Common.Dtos.Classes.Integers;
using Common.Dtos.Classes.Lists;
using Common.Dtos.Classes.Strings;

namespace Common
{
    public class ArgumentsForTestingReadingAndWritingFromMemoryStream : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { typeof(ClassWith256Ints), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith128Ints), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith64Ints), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith32Ints), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith16Ints), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith8Ints), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith4Ints), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith2Ints), 5, TimeSpan.FromSeconds(1) };

            yield return new object[] { typeof(ClassWith256Strings), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith128Strings), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith64Strings), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith32Strings), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith16Strings), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith8Strings), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith4Strings), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith2Strings), 5, TimeSpan.FromSeconds(1) };

            yield return new object[] { typeof(ClassWith256Lists<int>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith128Lists<int>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith64Lists<int>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith32Lists<int>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith16Lists<int>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith8Lists<int>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith4Lists<int>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith2Lists<int>), 5, TimeSpan.FromSeconds(1) };

            yield return new object[] { typeof(ClassWith256Lists<string>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith128Lists<string>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith64Lists<string>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith32Lists<string>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith16Lists<string>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith8Lists<string>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith4Lists<string>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith2Lists<string>), 5, TimeSpan.FromSeconds(1) };

            yield return new object[] { typeof(ClassWith256Arrays<int>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith128Arrays<int>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith64Arrays<int>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith32Arrays<int>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith16Arrays<int>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith8Arrays<int>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith4Arrays<int>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith2Arrays<int>), 5, TimeSpan.FromSeconds(1) };

            yield return new object[] { typeof(ClassWith256Arrays<string>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith128Arrays<string>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith64Arrays<string>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith32Arrays<string>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith16Arrays<string>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith8Arrays<string>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith4Arrays<string>), 5, TimeSpan.FromSeconds(1) };
            yield return new object[] { typeof(ClassWith2Arrays<string>), 5, TimeSpan.FromSeconds(1) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
