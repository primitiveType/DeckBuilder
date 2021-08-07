namespace Data
{
    public abstract class GameEntity
    {
        public int Id { get; private set; }

        protected GameEntity()
        {
            Id = UnityEngine.Random.Range(0 , 100000000);//test code
        }
    }
}