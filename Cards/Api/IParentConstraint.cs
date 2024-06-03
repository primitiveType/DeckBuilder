namespace Api
{
    public interface IParentConstraint
    {
        bool AcceptsParent(IEntity parent);
        bool AcceptsChild(IEntity child);
    }
}
