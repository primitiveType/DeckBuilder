//Singleton that provides access to services.
public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public GlobalApi Api { get; private set; }
    protected override void SingletonAwakened()
    {
        base.SingletonAwakened();
        Api = new GlobalApi();
    }
}