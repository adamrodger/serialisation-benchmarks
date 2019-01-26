using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;

namespace SerialiserBenchmarks
{
    public class SizeBenchmark
    {
        private static  readonly IEnumerable<Type> SerialiserTypes = 
            typeof(ISerialiser).Assembly
                               .GetTypes()
                               .Where(t => t.IsClass && 
                                           typeof(ISerialiser).IsAssignableFrom(t));

        public void Print()
        {
            var fixture = new Fixture();
            var simple = fixture.Create<SimpleObject>();
            var complex = fixture.Create<ComplexObject>();
            var simple10 = fixture.CreateMany<SimpleObject>(10).ToArray();
            var simple10000 = fixture.CreateMany<SimpleObject>(10000).ToArray();
            var complex10 = fixture.CreateMany<ComplexObject>(10).ToArray();
            var complex10000 = fixture.CreateMany<ComplexObject>(10000).ToArray();

            var builder = new StringBuilder();
            builder.AppendLine("| Serialiser                | Simple | Complex | Simple (10) | Complex (10) | Simple (10000) | Complex (10000) |");
            builder.AppendLine("| ------------------------- | ------:| -------:| -----------:| ------------:| --------------:| ---------------:|");

            foreach (Type type in SerialiserTypes)
            {
                ISerialiser serialiser = (ISerialiser)Activator.CreateInstance(type);
                var serialised = new[]
                {
                    serialiser.Serialise(simple),
                    serialiser.Serialise(complex),
                    serialiser.Serialise(simple10),
                    serialiser.Serialise(complex10),
                    serialiser.Serialise(simple10000),
                    serialiser.Serialise(complex10000)
                };

                builder.Append("| ");
                builder.Append(type.Name.PadRight(25));
                builder.Append(" | ");
                builder.Append(string.Join(" | ", serialised.Select(s => s.Length)));
                builder.AppendLine(" |");
            }

            Console.WriteLine(builder.ToString());
        }
    }
}