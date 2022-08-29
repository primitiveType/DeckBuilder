using System.Linq;
using App.Utility;
using Guirao.UltimateTextDamage;
using UnityEngine;

namespace SummerJam1
{
    public class TextPopupManager : MonoBehaviourSingleton<TextPopupManager>
    {
        private UltimateTextDamageManager _manager;
        private UltimateTextDamageManager _worldManager;

        private UltimateTextDamageManager UiManager => _manager != null
            ? _manager
            : _manager = FindObjectsOfType<UltimateTextDamageManager>(true).First(utd => !utd.convertToCamera);

        private UltimateTextDamageManager WorldManager => _worldManager != null
            ? _worldManager
            : _worldManager = FindObjectsOfType<UltimateTextDamageManager>(true).First(utd => utd.convertToCamera);


        public const string DAMAGE_TEXT = "damage";
        public const string HEALING_TEXT = "healing";
        public const string STAT_TEXT = "stat";

        public void Add(string text, GameObject go, string key)
        {
            var canvas = go.GetComponentInParent<Canvas>();
            if (canvas == null || canvas.renderMode == RenderMode.WorldSpace)
            {
                WorldManager.Add(text, go.transform, key);
            }
            else
            {
                UiManager.Add(text, go.transform, key);
            }
        }

        public void CreateDamageText()
        {
            UiManager.Add("TEST", new Vector3(), DAMAGE_TEXT);
        }

        public void CreateHealingText()
        {
            UiManager.Add("TEST", new Vector3(), HEALING_TEXT);
        }

        public void CreateStatText()
        {
            UiManager.Add("TEST", new Vector3(), STAT_TEXT);
        }
    }
}
