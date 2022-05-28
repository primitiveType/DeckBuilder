using System;
using System.Collections.ObjectModel;
using UnityEngine;

public class Pile : MonoBehaviour, IPile
{
    public ObservableCollection<IPileItem> Items { get; } = new ObservableCollection<IPileItem>();

    public bool ReceiveItem(IPileItem item)
    {
        foreach (var constraint in GetComponentsInChildren<IPileConstraint>())
        {
            if (!constraint.CanReceive(item))
            {
                return false;
            }
        }

        if (Items.Contains(item))
        {
            return false;
        }

        if (item.CanEnterPile(this))
        {
            Items.Add(item);
            return true;
        }

        return false;
    }

    public void RemoveItem(IPileItem item)
    {
        //do nothing.
        Items.Remove(item);
    }
}