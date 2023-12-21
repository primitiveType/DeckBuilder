using App;
using UnityEngine;

namespace SummerJam1
{
    public class TurnManager : MonoBehaviour 
    {
        private Game Game => GameContext.Instance.Game;

        public void WaitForCard()
        {
            if (!InputStateManager.Instance.StateMachine.CanFire(InputAction.WaitForCard))
            {
                return;
            }

            InputStateManager.Instance.StateMachine.Fire(InputAction.WaitForCard);
            Debug.Log("Getting card.");

            Game.WaitForCard();
        }

        public void EndTurn()
        {
            if (!InputStateManager.Instance.StateMachine.CanFire(InputAction.EndTurn))
            {
                return;
            }

            InputStateManager.Instance.StateMachine.Fire(InputAction.EndTurn);
            Debug.Log("Ending turn.");

            Game.EndTurn();
            Debug.Log("Queueing turn start.");
            AnimationQueue.Instance.Enqueue(() =>
            {
                Debug.Log("Starting next turn.");
                InputStateManager.Instance.StateMachine.Fire(InputAction.BeginTurn);
            });
        }
        
    }
}
