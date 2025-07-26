using App.Metrics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Dias.Metrics.Tests
{
    public static class Utils
    {
        public static Unit GetUnit(string name)
        {
            FieldInfo field = typeof(Unit).GetField(name, BindingFlags.Static | BindingFlags.Public);
            if (field is null)
            {
                throw new InvalidOperationException($"{nameof(Unit)} does not have field {name}");
            }
            var value = field.GetValue(null);

            if (value is null)
            {
                throw new InvalidOperationException($"The value of {nameof(Unit)}.{name}");
            }

            return (Unit)value;

        }

    }

    public class MetricsUnitName : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            Type unitType = typeof(Unit);
            FieldInfo[] fields = unitType.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (var field in fields)
            {
                yield return new object[] { field.Name };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
