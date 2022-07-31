using App;
using UnityEngine;

namespace SummerJam1
{
    public class TurnManager : MonoBehaviour 
    {
        private Game Game => GameContext.Instance.Game;


        public void EndTurn()
        {
            if (!InputStateManager.Instance.StateMachine.CanFire(InputAction.EndTurn))
            {
                return;
            }

            InputStateManager.Instance.StateMachine.Fire(InputAction.EndTurn);
            Game.EndTurn();
            AnimationQueue.Instance.Enqueue(() => { InputStateManager.Instance.StateMachine.Fire(InputAction.BeginTurn); });
        }
    }
}
