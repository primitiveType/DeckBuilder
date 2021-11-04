using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : GameBehaviour {

	[SerializeField]
	private MenuPanel DefaultMenu;

	private Stack<MenuPanel> MenuStack { get; } = new Stack<MenuPanel>();



	public void PushMenu(MenuPanel menuPanel) {
		
		if (menuPanel.DeactivatePredecessor && MenuStack.Count > 0) {
			MenuStack.Peek().gameObject.SetActive(false);
		}

		menuPanel.gameObject.SetActive(true);
		MenuStack.Push(menuPanel);
	}

	public void PopMenu() {
		if(MenuStack.Count > 1) {
			MenuPanel menuPanel = MenuStack.Pop();
			menuPanel.gameObject.SetActive(false);
			MenuStack.Peek().gameObject.SetActive(true);
		}
	}

	protected override void Start() {
		base.Start();

		foreach(MenuPanel panel in GetComponentsInChildren<MenuPanel>()) {
			panel.gameObject.SetActive(false);
		}

		PushMenu(DefaultMenu);
	}

}
