using System.Collections.Generic;

namespace SWBF2
{
    public class Animation
    {
        public string Name;
        public float Runtime;
        public bool Loop;
        public bool LocalTranslation;

        public IList<PositionKey> PositionKeys { get; } = new List<PositionKey>();
        public IList<RotationKey> RotationKeys { get; } = new List<RotationKey>();

        public Animation(string name, float runtime, bool loop, bool localTranslation)
        {
            Name = name;
            Runtime = runtime;
            Loop = loop;
            LocalTranslation = localTranslation;
        }
    }
}