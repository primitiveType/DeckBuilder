using System.ComponentModel;
using Component = Api.Component;
using IComponent = Api.IComponent;

namespace CardsAndPiles
{
    public class Draggable : Component, IDraggable
    {
        public bool CanDrag { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public interface IDraggable : INotifyPropertyChanged, IComponent
    {
        bool CanDrag { get; set; }
    }
}