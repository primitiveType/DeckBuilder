public interface IView<out T> : IView
{
    T Model { get; }
}