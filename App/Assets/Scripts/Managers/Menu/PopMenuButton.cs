using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopMenuButton : GameBehaviour, IPointerClickHandler {

	[InitFromParent]
	private MainMenuController MainMenuController;

	public void OnPointerClick(PointerEventData eventData) {
		MainMenuController.PopMenu();
	}
}
