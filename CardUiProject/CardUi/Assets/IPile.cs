using System.Collections.ObjectModel;

public interface IPile
{
    public bool ReceiveItem(IPileItem item);
    public void RemoveItem(IPileItem item);
    ObservableCollection<IPileItem> Items { get; }
}