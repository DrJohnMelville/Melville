using System;

namespace Melville.Linq.Statistics.Tables
{
  [Flags]
  public enum SummaryFunctionSelection
  {
    Cell         = 0b00001,
    Row          = 0b00010,
    Column       = 0b00100,
    LowerRight   = 0b01000,
    AllSummaries = 0b01110,
    All          = 0b01111
  };
}