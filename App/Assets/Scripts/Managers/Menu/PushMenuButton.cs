using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PushMenuButton : GameBehaviour, IPointerClickHandler {

	[SerializeField]
	private MenuPanel PanelToActivate;

	[InitFromParent]
	private MainMenuController MainMenuController;

	public void OnPointerClick(PointerEventData eventData) {
		MainMenuController.PushMenu(PanelToActivate);
	}
}
