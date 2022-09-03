using App;
using SummerJam1;

public class DungeonsView : View<DungeonParent>
{
    private void Awake()
    {
        SetModel(GameContext.Instance.Game.Dungeons);
    }

}
