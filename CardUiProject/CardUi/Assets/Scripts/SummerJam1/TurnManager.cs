using Api;
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
            Logging.Log("Getting card.");

            Game.WaitForCard();
        }

        public void EndTurn()
        {
            if (!InputStateManager.Instance.StateMachine.CanFire(InputAction.EndTurn))
            {
                return;
            }

            InputStateManager.Instance.StateMachine.Fire(InputAction.EndTurn);
            Logging.Log("Ending turn.");

            Game.EndTurn();
            Logging.Log("Queueing turn start.");
            AnimationQueue.Instance.Enqueue(() =>
            {
                Logging.Log("Starting next turn.");
                InputStateManager.Instance.StateMachine.Fire(InputAction.BeginTurn);
            });
        }
        
    }
}
