using Api;

namespace App
{
    public interface IView<out T> : IView
    {
        T Model { get; }
    }

    public interface ISetModel
    {
        void SetModel(IEntity entity);
        void SetModel(IComponent component);

    }
}
