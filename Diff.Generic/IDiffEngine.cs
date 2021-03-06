﻿using System;
using System.Collections.Generic;
using Diff.Generic.Model;

namespace Diff.Generic
{
    /// <summary>
    /// Detects differences between object streams.
    /// </summary>
    public interface IDiffEngine<T>
    {
        DiffResult<T> CreateDiffs(IList<T> oldStream, IList<T> newStream);
    }
}