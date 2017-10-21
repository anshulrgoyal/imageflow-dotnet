﻿using System;
using System.Collections.Generic;

namespace Imageflow.Fluent
{
    public interface IPreparedFilesystemJob : IDisposable
    {
        string JsonPath { get; }
        IReadOnlyDictionary<int, string> OutputFiles { get; }
    }
}