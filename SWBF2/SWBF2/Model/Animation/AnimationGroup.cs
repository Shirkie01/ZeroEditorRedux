using System.Collections.Generic;

namespace SWBF2
{
    public class AnimationGroup
    {
        public string Name;
        public IDictionary<Animation, GameObject> AnimationObjectPairs { get; } = new Dictionary<Animation, GameObject>();
        public IList<Animation> Animations { get; } = new List<Animation>();
        public bool PlayOnStart;
        public bool StopWhenObjectIsControlled;
        public bool DisableHierarchies;
    }
}