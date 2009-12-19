
namespace AutoLayoutApplication
{
    public struct Vector
    {
        public double X;
        public double Y;

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector()
            {
                X = v1.X + v2.X,
                Y = v1.Y + v2.Y
            };
        }
    }
}
