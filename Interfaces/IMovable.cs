using System.Drawing;

namespace PlantsVsZombies
{
    public interface IMovable
    {
        // Velocity of the object
        PointF Velocity { get; set; }
    }
}