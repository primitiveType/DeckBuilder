namespace SummerJam1.Units
{
    public class FirstDungeonExit : ClickToExitBattle<SecondDungeonPile>
    {
    }
    public class SecondDungeonExit : ClickToExitBattle<FirstDungeonPile>
    {
    }

}
