using UnityEngine;
using UnityEngine.UI;

namespace App
{
    public class ToggleGameObjectButton : MonoBehaviour
    {
        [SerializeField] private GameObject ToToggle;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => { ToToggle.SetActive(!ToToggle.activeSelf); });
        }
    }
}
