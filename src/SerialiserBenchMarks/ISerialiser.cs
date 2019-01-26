namespace SerialiserBenchmarks
{
    public interface ISerialiser
    {
        byte[] Serialise<T>(T instance);
        T Deserialise<T>(byte[] bytes);
    }
}