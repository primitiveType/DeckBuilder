using System;
using DeckbuilderLibrary.Data.GameEntities.Actors;

public static class Tools
{
    public static IPersistentData Data { get; private set; }
    public static PlayerActor Player { get; set; }

    private static bool initialized;

    public static void Initialize(IPersistentData dataAccess)
    {
        if (initialized)
        {
            throw new NotSupportedException();
        }

        initialized = true;
        Data = dataAccess;
    }
}