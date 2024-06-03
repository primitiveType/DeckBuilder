using Api;

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
    }
}