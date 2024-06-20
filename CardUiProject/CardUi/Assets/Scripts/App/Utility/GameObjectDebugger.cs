using System;
using UnityEngine;

namespace App.Utility
{
    public class GameObjectDebugger : MonoBehaviour
    {
        private void Awake()
        {
            // Debug.Log($"Gameobject created with hashcode {GetHashCode()}", gameObject);
        }

        private void OnDestroy()
        {
            // Debug.Log($"Gameobject destroyed with hashcode {GetHashCode()}", gameObject);
        }
    }
}
