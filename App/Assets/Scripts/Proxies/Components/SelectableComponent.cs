using UnityEngine.EventSystems;
using UnityEngine;

public class SelectableComponent : GameEntityComponent, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private InputManager m_InputManager;

    private InputManager InputManager => m_InputManager ?? (m_InputManager = GameObject.Find("InputManager").GetComponent<InputManager>());

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InputManager.GameEntitySelected(GameEntity);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InputManager.GameEntityHovered(GameEntity);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InputManager.GameEntityUnHovered(GameEntity);
    }
}