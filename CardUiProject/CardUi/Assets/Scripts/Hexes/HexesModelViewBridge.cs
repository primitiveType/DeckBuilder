using App;
using Newtonsoft.Json;
using UnityEngine;
using Component = Api.Component;

public class HexesModelViewBridge : Component, IGameObject
{
    private GameObject m_GameObject;

    [JsonIgnore]
    public GameObject gameObject
    {
        get
        {
            if (m_GameObject == null)
            {
                gameObject = HexesManager.Instance.CreateGameObjectForModel(Entity);
            }

            return m_GameObject;
        }
        set => m_GameObject = value;
    }
}
