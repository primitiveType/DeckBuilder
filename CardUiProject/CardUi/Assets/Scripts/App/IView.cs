using Api;
using Common;

public interface IView<out T> : IView
{
    T Model { get; }
}

public interface ISetModel
{
    void SetModel(IEntity entity);
}