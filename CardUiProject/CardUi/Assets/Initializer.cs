using App.Utility;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviourSingleton<Initializer>
{//guarantees the main scene is always loaded.
    protected override void SingletonAwakened()
    {
        base.SingletonAwakened();
        SceneManager.LoadScene("Main");
    }
}
