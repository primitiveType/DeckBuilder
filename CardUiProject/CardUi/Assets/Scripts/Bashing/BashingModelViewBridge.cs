using App;
using Newtonsoft.Json;
using UnityEngine;
using Component = Api.Component;

namespace SummerJam1
{
    public class BashingModelViewBridge : Component, IGameObject
    {
        private GameObject m_GameObject;

        [JsonIgnore]
        public GameObject gameObject
        {
            get
            {
                if (m_GameObject == null)
                {
                    gameObject = BashingHelper.Instance.CreateGameObjectForModel(this);
                }

                return m_GameObject;
            }
            set => m_GameObject = value;
        }
    }
}