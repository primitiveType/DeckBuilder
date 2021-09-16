using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DeckbuilderLibrary.Data.Achievements;

public class AchievementManager : MonoBehaviour
{
    private IAchievementManager Manager { get; set; }

    void Awake()
    {
#if UNITY_STANDALONE // || UNITY_EDITOR // uncomment if you want achievements/steam init to happen in editor, can be annoying.

        Manager = new SteamAchievementManager();
        Manager.Init();

#endif
    }
}
