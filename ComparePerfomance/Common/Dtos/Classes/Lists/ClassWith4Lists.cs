﻿using System.Collections.Generic;

namespace Common.Dtos.Classes.Lists
{
    public class ClassWith4Lists<T>
    {
        public List<T> Property1 { get; set; }
        public List<T> Property2 { get; set; }
        public List<T> Property3 { get; set; }
        public List<T> Property4 { get; set; }
    }
}
