using System.Collections.Generic;

namespace SWBF2
{
    public class Camera
    {
        public string Name;
        public Quaternion Rotation;
        public Vector3 Position;
        public float FieldOfView;
        public float NearPlane;
        public float FarPlane;
        public float ZoomFactor;
        public IList<Bookmark> Bookmarks;

        public Camera(string name)
        {
            Name = name;
        }
    }
}