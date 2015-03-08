﻿using System;
using System.Collections.Generic;
using System.Threading;
using BosunReporter.Infrastructure;

namespace BosunReporter.Metrics
{
    public class Counter : BosunMetric, ILongCounter
    {
        public long Value = 0;

        public override string MetricType => "counter";

        protected override IEnumerable<string> GetSerializedMetrics(string unixTimestamp)
        {
            yield return ToJson("", Value, unixTimestamp);
        }

        public Counter()
        {
        }

        public void Increment(long amount = 1)
        {
            if (!IsAttached)
            {
                var ex = new InvalidOperationException("Attempting to record on a gauge which is not attached to a BosunReporter object.");
                try
                {
                    ex.Data["Metric"] = Name;
                    ex.Data["Tags"] = SerializedTags;
                }
                finally
                {
                    throw ex;
                }
            }

            Interlocked.Add(ref Value, amount);
        }
    }
}