namespace DeckbuilderLibrary.Data.GameEntities.Resources
{
    public interface IArithmetic<T>
    {
        void Add(T amount);
        void Subtract(T amount);
    }
}