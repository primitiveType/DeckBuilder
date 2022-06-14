using App;
using Newtonsoft.Json;
using UnityEngine;

namespace SummerJam1
{
    public class
        SummerJam1ModelViewBridge : Api.Component, IGameObject //ViewBridge<SummerJam1Card, SummerJam1CardViewBridge>
    {
        private GameObject m_GameObject;

        [JsonIgnore]
        public GameObject gameObject
        {
            get
            {
                if (m_GameObject == null)
                {
                    gameObject = SummerJam1Context.Instance.CreateGameObjectForModel(this);
                }
                return m_GameObject;
            }
            set => m_GameObject = value;
        }
    }
}