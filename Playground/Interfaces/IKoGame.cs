namespace Playground.Interfaces
{
    public interface IKoGame
    {
        bool Playing { get; }
        bool Activating { get; }
        bool Deactivating { get; }

        string Name { get; }
        void Stop();
        void Begin();
        void Create();
        void Destroy();
    }
}