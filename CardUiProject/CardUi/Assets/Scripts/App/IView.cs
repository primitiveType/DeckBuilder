using Api;
using Common;

public interface IView<out T> : IView
{
    T Model { get; }
    void SetModel(Entity entity);
}