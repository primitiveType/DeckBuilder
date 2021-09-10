using UnityEngine.EventSystems;
using UnityEngine;

public class SelectableComponent : GameEntityComponent, IPointerDownHandler
{
    private InputManager m_InputManager;

    private InputManager InputManager => m_InputManager ?? (m_InputManager = GameObject.Find("InputManager").GetComponent<InputManager>());

    public void OnPointerDown(PointerEventData eventData)
    {
        InputManager.GameEntitySelected(GameEntity);
    }
}