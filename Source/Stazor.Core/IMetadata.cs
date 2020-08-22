﻿using System;
using System.Collections.Generic;

namespace Stazor.Core
{
    public interface IMetadata
    {
        string? Title { get; set; }

        DateTimeOffset Date { get; set; }

        DateTimeOffset? ModifiedDate { get; set; }

        string? Category { get; set; }

        List<string>? Tags { get; set; }
    }
}