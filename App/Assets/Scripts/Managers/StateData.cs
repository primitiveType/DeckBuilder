using DeckbuilderLibrary.Data.GameEntities;

public struct StateData
{
    public readonly IGameEntity Selected;

    public StateData(IGameEntity selected)
    {
        Selected = selected;
    }
}