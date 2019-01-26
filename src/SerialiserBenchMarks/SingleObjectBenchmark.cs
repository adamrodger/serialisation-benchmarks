using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using BenchmarkDotNet.Attributes;

namespace SerialiserBenchmarks
{
    public class SingleObjectBenchmark
    {
        public static IEnumerable<Type> SerialiserTypes => 
            typeof(ISerialiser).Assembly
                               .GetTypes()
                               .Where(t => t.IsClass && 
                                           typeof(ISerialiser).IsAssignableFrom(t));

        [ParamsSource(nameof(SerialiserTypes))]
        public Type SerialiserType { get; set;}

        private ISerialiser serialiser;
        private SimpleObject simple;
        private ComplexObject complex;
        private byte[] serialisedSimple;
        private byte[] serialisedComplex;

        [GlobalSetup]
        public void Setup()
        {
            this.serialiser = (ISerialiser)Activator.CreateInstance(this.SerialiserType);

            var fixture = new Fixture();
            this.simple = fixture.Create<SimpleObject>();
            this.complex = fixture.Create<ComplexObject>();

            this.serialisedSimple = this.serialiser.Serialise(this.simple);
            this.serialisedComplex = this.serialiser.Serialise(this.complex);
        }

        [Benchmark]
        public byte[] Serialise_Simple()
        {
            return this.serialiser.Serialise(this.simple);
        }

        [Benchmark]
        public SimpleObject Deserialise_Simple()
        {
            return this.serialiser.Deserialise<SimpleObject>(this.serialisedSimple);
        }

        [Benchmark]
        public byte[] Serialise_Complex()
        {
            return this.serialiser.Serialise(this.complex);
        }

        [Benchmark]
        public ComplexObject Deserialise_Complex()
        {
            return this.serialiser.Deserialise<ComplexObject>(this.serialisedComplex);
        }
    }
}