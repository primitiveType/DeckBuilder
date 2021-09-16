using System;

namespace Scenes.Scripts.StateMachine
{
    public interface IState<TData>
    {
        void Enter(TData input);
        void Exit(TData input);
        void EntitySelected(TData input);
        void EntityHovered(TData input);
        void EntityUnHovered(TData input);
    }
}