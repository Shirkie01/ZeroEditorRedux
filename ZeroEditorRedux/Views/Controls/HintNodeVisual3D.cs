using HelixToolkit.Wpf;
using SWBF2;
using System.Windows;
using System.Windows.Media.Media3D;
using Quaternion = System.Windows.Media.Media3D.Quaternion;

namespace ZeroEditorRedux.Controls
{
    internal class HintNodeVisual3D : MeshElement3D
    {
        private static readonly DependencyProperty NameProperty = DependencyProperty.Register(nameof(Name), typeof(string), typeof(HintNodeVisual3D));
        private static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(Point3D), typeof(HintNodeVisual3D));
        private static readonly DependencyProperty RotationProperty = DependencyProperty.Register(nameof(Rotation), typeof(Quaternion), typeof(HintNodeVisual3D));
        private static readonly DependencyProperty HintTypeProperty = DependencyProperty.Register(nameof(HintType), typeof(HintType), typeof(HintNodeVisual3D), new UIPropertyMetadata(HintType.Access));
        private static readonly DependencyProperty PrimaryStanceProperty = DependencyProperty.Register(nameof(PrimaryStance), typeof(PrimaryStance), typeof(HintNodeVisual3D), new UIPropertyMetadata(PrimaryStance.None));
        private static readonly DependencyProperty SecondaryStanceProperty = DependencyProperty.Register(nameof(SecondaryStance), typeof(SecondaryStance), typeof(HintNodeVisual3D), new UIPropertyMetadata(SecondaryStance.None));
        private static readonly DependencyProperty HintModeProperty = DependencyProperty.Register(nameof(HintMode), typeof(HintMode), typeof(HintNodeVisual3D), new UIPropertyMetadata(HintMode.None));
        private static readonly DependencyProperty CommandPostProperty = DependencyProperty.Register(nameof(CommandPost), typeof(string), typeof(HintNodeVisual3D));
        private static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(nameof(Radius), typeof(double), typeof(HintNodeVisual3D));

        public string Name
        {
            get
            {
                return (string)GetValue(NameProperty);
            }
            set
            {
                SetValue(NameProperty, value);
            }
        }

        public Point3D Position
        {
            get
            {
                return (Point3D)GetValue(PositionProperty);
            }
            set
            {
                SetValue(PositionProperty, value);
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return (Quaternion)GetValue(RotationProperty);
            }
            set
            {
                SetValue(RotationProperty, value);
            }
        }

        public HintType HintType
        {
            get
            {
                return (HintType)GetValue(HintTypeProperty);
            }
            set
            {
                SetValue(HintTypeProperty, value);
            }
        }

        public PrimaryStance PrimaryStance
        {
            get
            {
                return (PrimaryStance)GetValue(PrimaryStanceProperty);
            }

            set
            {
                SetValue(PrimaryStanceProperty, value);
            }
        }

        public SecondaryStance SecondaryStance
        {
            get
            {
                return (SecondaryStance)GetValue(SecondaryStanceProperty);
            }

            set
            {
                SetValue(SecondaryStanceProperty, value);
            }
        }

        public HintMode HintMode
        {
            get
            {
                return (HintMode)GetValue(HintModeProperty);
            }
            set
            {
                SetValue(HintModeProperty, value);
            }
        }

        public string CommandPost
        {
            get
            {
                return (string)GetValue(CommandPostProperty);
            }

            set
            {
                SetValue(CommandPostProperty, value);
            }
        }

        public double Radius
        {
            get
            {
                return (double)GetValue(RadiusProperty);
            }

            set
            {
                if (value > 0)
                {
                    SetValue(RadiusProperty, value);
                }
            }
        }

        protected override MeshGeometry3D Tessellate()
        {
            var builder = new MeshBuilder(false, true);
            builder.AddSphere(Position, Radius);
            Point3D endPoint = Position + (Rotation.Axis * Rotation.Angle);
            builder.AddArrow(Position, endPoint, 1);
            return builder.ToMesh();
        }
    }
}