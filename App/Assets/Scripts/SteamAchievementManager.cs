using DeckbuilderLibrary.Data.Achievements;
using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SteamAchievementManager : IAchievementManager
{

	public void Init() {
		Steamworks.SteamClient.Init(480 /* steam app id for "Spacewar", the testing app for steam. To get our own achievements the game will have to be approved on steam */, true);
	}

	public void TriggerAchievement(string key) {
		new Achievement(key).Trigger();
	}
}


/*
	To clear all achievements:

		Steamworks.SteamUserStats.Achievements.ToList().ForEach(Achievement => Achievement.Clear());

	

 */