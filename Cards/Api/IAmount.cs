using System.ComponentModel;

namespace Api
{
    public interface IAmount : INotifyPropertyChanged, IComponent
    {
        int Amount { get; set; }
    }
}
