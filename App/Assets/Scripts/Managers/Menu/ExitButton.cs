using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitButton : GameBehaviour, IPointerClickHandler {
	public void OnPointerClick(PointerEventData eventData) {
		// ignored in editor i guess
		Application.Quit();
	}
}
