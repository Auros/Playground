namespace Playground.Interfaces
{
    public interface IKoGame
    {
        string Name { get; }
        void Stop();
        void Begin();
        void Create();
        void Destroy();
    }
}