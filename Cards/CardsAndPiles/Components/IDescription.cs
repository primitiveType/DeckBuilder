using Api;
using Newtonsoft.Json;

namespace CardsAndPiles.Components
{
    public interface IDescription : IComponent
    {
        [JsonIgnore]
        string Description { get; }
    }
}
