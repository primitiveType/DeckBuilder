using Api;
using Newtonsoft.Json;

namespace CardsAndPiles.Components
{
    public class DescriptionComponent : Component, IDescription
    {
        [JsonProperty]
        private string _description;

        // ReSharper disable once ConvertToAutoProperty we want this class to be the only exception to the JsonIgnore defined in the interface.
        public string Description
        {
            get => _description;
            set => _description = value;
        }
    }
}
