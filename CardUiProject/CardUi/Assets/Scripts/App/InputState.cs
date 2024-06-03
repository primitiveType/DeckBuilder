namespace App
{
    public enum InputState
    {
        Idle,
        Dragging,
        Hovering,
        ChoosingDiscard,
        EnemyTurn
    }

    public enum InputAction
    {
        Drag,
        EndDrag,
        Hover,
        EndHover,
        ChooseDiscard,
        EndChooseDiscard,
        WaitForCard,
        EndTurn,
        BeginTurn,
        EndDungeonPhase,
        BeginDungeonPhase,
        PlayCard
    }
}
