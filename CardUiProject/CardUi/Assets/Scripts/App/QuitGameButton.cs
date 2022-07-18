using System;
using UnityEngine;
using UnityEngine.UI;

namespace App
{
    public class QuitGameButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(Application.Quit);
        }
    }
}
