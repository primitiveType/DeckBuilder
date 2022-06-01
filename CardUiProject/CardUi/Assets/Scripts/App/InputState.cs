using System.Collections;
using System.Collections.Generic;
using Stateless.Reflection;
using UnityEngine;


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
