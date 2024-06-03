using System;
using App.Utility;
using UnityEngine;

namespace App
{
    public class MapContainer : MonoBehaviour
    {
        private void Awake()
        {
            Instance = this;
        }

        public static MapContainer Instance { get; set; }
    }
    
}
