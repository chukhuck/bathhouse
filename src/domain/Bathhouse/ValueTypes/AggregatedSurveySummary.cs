using Bathhouse.Entities;
using Bathhouse.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bathhouse.ValueTypes
{
  public class AggregatedSurveySummary : SimpleSurveySummary
  {
    protected internal AggregatedSurveySummary(Survey survey) : base(survey) { }


    protected override sealed List<string> GetFooters()
    {
      return Headers?.Select((header, index) => Data.AggregateColumn(index, header.Type)).ToList()
        ?? new List<string>();
    }
  }
}