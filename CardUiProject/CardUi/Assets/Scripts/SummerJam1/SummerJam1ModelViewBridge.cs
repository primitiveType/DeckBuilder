using App;
using Newtonsoft.Json;
using UnityEngine;
using Component = Api.Component;

namespace SummerJam1
{
    public class SummerJam1ModelViewBridge : Component, IGameObject
    {
        private GameObject m_GameObject;

        [JsonIgnore]
        public GameObject gameObject
        {
            get
            {
                if (m_GameObject == null)
                {
                    gameObject = ViewFactory.Instance.CreateGameObjectForModel(Entity);
                }

                return m_GameObject;
            }
            set => m_GameObject = value;
        }
    }
}
