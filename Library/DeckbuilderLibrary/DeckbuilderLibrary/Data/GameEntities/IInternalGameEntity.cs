namespace DeckbuilderLibrary.Data.GameEntities
{
    internal interface IInternalGameEntity : IGameEntity, IInternalInitialize
    {
        
    }

    internal interface IInternalInitialize 
    {
        void InternalInitialize();

        void SetContext(IContext context);
    }
}