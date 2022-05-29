using Api;
using UnityEngine;

namespace Common
{
    public class View<T> : MonoBehaviour, IView<T> where T : IComponent
    {
        public Entity Entity { get; private set; }
        public T Model { get; private set; }

        public void SetModel(Entity entity)
        {
            Entity = entity;
            Model = entity.GetComponent<T>();
            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
        
        }
    }

    public interface IView
    {
        Entity Entity { get; }
    }
}