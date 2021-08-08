public interface IGameEventHandler
{
    event CardMovedEvent CardMoved;
    void InvokeCardMoved(object sender, CardMovedEventArgs args);
}