using Api;

namespace CardsAndPiles.Components
{
    public class NameComponent : CardsAndPilesComponent
    {
        public string Value { get; set; }
    }

    public interface IDescription : IComponent
    {
        string Description { get; }
    }
    
   
    
}

