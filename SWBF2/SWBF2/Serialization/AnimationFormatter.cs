using System.Collections.Generic;
using System.IO;

namespace SWBF2.Serialization
{
    internal class AnimationFormatter : ITypedFormatter<IList<Animation>>
    {
        public IList<Animation> Deserialize(Stream serializationStream)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(Stream serializationStream, IList<Animation> animations)
        {
            using (var writer = new StreamWriter(serializationStream))
            {
                foreach (Animation animation in animations)
                {
                    writer.WriteLine("Animation()");
                    writer.WriteLine("{");

                    foreach (var positionKey in animation.PositionKeys)
                    {
                        writer.WriteLine(string.Format("AddPositionKey({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10});", positionKey.Time, positionKey.Position.x, positionKey.Position.y, positionKey.Position.z, positionKey.Transition, positionKey.SplinePosition.x, positionKey.SplinePosition.y, positionKey.SplinePosition.z));
                    }
                    foreach (var rotationKey in animation.RotationKeys)
                    {
                        var angles = rotationKey.Rotation.ToEulerAngles();
                        writer.WriteLine(string.Format("AddPositionKey({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10});", rotationKey.Time, angles.x, angles.y, angles.z, rotationKey.Transition, rotationKey.SplinePosition.x, rotationKey.SplinePosition.y, rotationKey.SplinePosition.z));
                    }
                    writer.WriteLine();
                }
            }
        }
    }
}