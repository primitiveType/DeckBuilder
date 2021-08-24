namespace DeckbuilderLibrary.Data.GameEntities
{
    internal interface IInternalGameEntity : IGameEntity
    {
        void InternalInitialize();

        void SetContext(IContext context);
    }
}