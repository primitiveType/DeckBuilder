using System;
using UnityEngine;

namespace App.Utility
{
    public class GameObjectDebugger : MonoBehaviour
    {
        private void Awake()
        {
            // Logging.Log($"Gameobject created with hashcode {GetHashCode()}", gameObject);
        }

        private void OnDestroy()
        {
            // Logging.Log($"Gameobject destroyed with hashcode {GetHashCode()}", gameObject);
        }
    }
}
