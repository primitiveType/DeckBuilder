using System.Collections;
using System.Collections.Generic;
using Data;

/// <summary>
/// Not sure if this class is necessary. It's just a list that will fire events, essentially. Events aren't hooked up yet though.
/// Also it probably makes more sense to listen to the deck object so that you always see where the card came from and is going.
/// </summary>
public class Pile : GameEntity, IList<Card>
{
    private readonly List<Card> m_Cards = new List<Card>();

    public IEnumerator<Card> GetEnumerator()
    {
        return m_Cards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable) m_Cards).GetEnumerator();
    }

    public void Add(Card item)
    {
        m_Cards.Add(item);
    }

    public void Clear()
    {
        m_Cards.Clear();
    }

    public bool Contains(Card item)
    {
        return m_Cards.Contains(item);
    }

    public void CopyTo(Card[] array, int arrayIndex)
    {
        m_Cards.CopyTo(array, arrayIndex);
    }

    public bool Remove(Card item)
    {
        return m_Cards.Remove(item);
    }

    public int Count => m_Cards.Count;

    public bool IsReadOnly => ((ICollection<Card>) m_Cards).IsReadOnly;

    public int IndexOf(Card item)
    {
        return m_Cards.IndexOf(item);
    }

    public void Insert(int index, Card item)
    {
        m_Cards.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        m_Cards.RemoveAt(index);
    }

    public Card this[int index]
    {
        get => m_Cards[index];
        set => m_Cards[index] = value;
    }
}