using Api;
using CardsAndPiles.Components;

namespace CardsAndPiles
{
    public static class EntityExtensions
    {
        public static string GetDebugString(this IEntity entity)
        {
            string components = "";
            foreach (var addedComponent in entity.Components)
            {
                if (components.Length > 0)
                {
                    components += ", ";
                }

                components += addedComponent.GetType().Name;
            }

            return $"Entity {entity.Id} {components}.";
        }

        public static IEntity WithName(this IEntity entity, string name)
        {
            entity.GetOrAddComponent<NameComponent>().Value = name;
            return entity;
        }
        public static string GetName(this IEntity entity)
        {
            if (entity == null)
                return "Null";
            string myName = entity.GetComponent<NameComponent>()?.Value ?? $"Entity ({entity.Id.ToString()})";
            return myName;
        }
        
        public static TComponent WithName<TComponent>(this TComponent component, string name) where TComponent: IComponent
        {
            component.Entity.GetOrAddComponent<NameComponent>().Value = name;
            return component;
        }

    }
}