using UnityEngine;

public static class Injector
{
    public static IGlobalApi GlobalApi { get; private set; }
    public static IGameEventHandler GameEventHandler { get; set; }

    public static void Initialize()
    {
        GlobalApi = new GlobalApi();
        GameEventHandler = new GameEventHandler();
    }
}