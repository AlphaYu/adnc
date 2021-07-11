﻿using System;
using System.Collections.Generic;
using System.Text;

namespace System.Threading
{
    public class ThreadPoolSettings
    {
        public int MinThreads { get; set; } = 300;
        public int MinCompletionPortThreads { get; set; } = 300;
        public int MaxThreads { get; set; } = 32767;
        public int MaxCompletionPortThreads { get; set; } = 1000;
    }
}
