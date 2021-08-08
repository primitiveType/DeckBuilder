
namespace Data
{
    public abstract class GameEntity
    {
        public int Id { get; private set; }
        protected IGameEventHandler GameEvents
        {
            get
            {
                return Injector.GameEventHandler;
            }
        }
        protected IGlobalApi Api => Injector.GlobalApi;

        protected GameEntity()
        {
            Id = UnityEngine.Random.Range(0 , 100000000);//test code
        }
    }
}