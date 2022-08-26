using Api;
using SummerJam1.Units;

namespace SummerJam1Tests
{
    public class TestUnit : Unit
    {
        protected override bool PlayCard(IEntity target)
        {
            return true;
        }

        public override bool AcceptsParent(IEntity parent)
        {
            return true;
        }
    }
}
