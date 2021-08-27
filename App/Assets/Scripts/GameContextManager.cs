using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities.Actors;

public class GameContextManager : MonoBehaviourSingleton<GameContextManager>
{
    public GameContext Context { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(this);

        Context = new GameContext();
        PlayerActor player = Context.CreateActor<PlayerActor>(100, 0);
        Tools.Player = player;
    }
}