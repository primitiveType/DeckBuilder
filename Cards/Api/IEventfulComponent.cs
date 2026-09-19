using System.Collections.Generic;

namespace Api
{
    public interface IEventfulComponent : IComponent
    {
        Dictionary<int, int> EventEntrance { get; }
    }
}