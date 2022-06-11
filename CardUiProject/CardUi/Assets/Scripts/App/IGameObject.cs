using UnityEngine;

namespace App
{
    public interface IGameObject
    {
        // ReSharper disable once InconsistentNaming Use Unity naming.
        public GameObject gameObject { get; }
    }
}