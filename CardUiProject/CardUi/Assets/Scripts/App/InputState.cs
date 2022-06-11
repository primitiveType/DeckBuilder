namespace App
{
    public enum InputState
    {
        Idle,
        Dragging,
        Hovering
    }

    public enum InputAction
    {
        Drag,
        EndDrag,
        Hover,
        EndHover,
    }
}