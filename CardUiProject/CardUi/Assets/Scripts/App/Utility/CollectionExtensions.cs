using System.Linq;
using Api;
using UnityEngine;

public static class CollectionExtensions
{
    public static T GetRandom<T>(this IChildrenCollection<T> collection)
    {
        int index = Random.Range(0, collection.Count - 1);
        return collection.ElementAt(index);
    }
}