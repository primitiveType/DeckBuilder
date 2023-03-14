using System.ComponentModel;

namespace Api
{
    public interface IAmount : INotifyPropertyChanged
    {
        int Amount { get; set; }
    }
}
