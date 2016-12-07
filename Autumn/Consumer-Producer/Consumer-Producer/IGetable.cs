namespace Consumer_Producer
{
    public interface IGetable<T>
    {
        T Get();
        void Stop();
    }
}