﻿namespace WinFormsAtBitmarck.Core.Services;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}