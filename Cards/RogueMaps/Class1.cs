using System.ComponentModel;
using System.Runtime.CompilerServices;
using RogueMaps.Annotations;
using RogueSharp;
using RogueSharp.MapCreation;

namespace RogueMaps
{
    public class MapComponent : Api.Component
    {
        public CustomMap Map { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Map = RogueSharp.Map.Create(new RandomRoomsMapCreationStrategy<CustomMap, CustomCell>(17, 10, 30, 5, 3));
        }
    }

    public class CustomMap : Map<CustomCell>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CustomCell : Cell, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
