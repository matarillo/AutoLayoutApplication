using System;
using System.Collections.Generic;

namespace AutoLayoutApplication
{
    public class Node
    {
        public string Name { get; private set; }
        public ICollection<Node> Neighbors { get; private set; }

        public Vector R { get; set; }
        public Vector V { get; set; }

        public Node(string name)
        {
            this.Name = name;
            this.Neighbors = new List<Node>();
        }

        public Vector GetSpringForce(Node n)
        {
            // ばねの力は自然長からの変位に比例 (比例定数 -k, ばねの長さ l)
            const double k = 0.1d;
            const double l = 60.0d;
            double dx = this.R.X - n.R.X;
            double dy = this.R.Y - n.R.Y;
            double d2 = dx * dx + dy * dy;
            if (d2 < double.Epsilon)
            {
                // 距離0の時は例外として乱数で決定
                Random rand = new Random();
                return new Vector()
                {
                    X = rand.NextDouble() - 0.5d,
                    Y = rand.NextDouble() - 0.5d
                };
            }
            double d = Math.Sqrt(d2);
            double cos = dx / d;
            double sin = dy / d;
            double dl = d - l;
            return new Vector()
            {
                X = -k * dl * cos,
                Y = -k * dl * sin
            };
        }

        public Vector GetReplusiveForce(Node n)
        {
            // 反発は距離の2乗に反比例 (比例定数 g)
            const double g = 500.0d;
            double dx = this.R.X - n.R.X;
            double dy = this.R.Y - n.R.Y;
            double d2 = dx * dx + dy * dy;
            if (d2 < double.Epsilon)
            {
                // 距離0の時は例外として乱数で決定
                Random rand = new Random();
                return new Vector()
                {
                    X = rand.NextDouble() - 0.5d,
                    Y = rand.NextDouble() - 0.5d
                };
            }
            double d = Math.Sqrt(d2);
            double cos = dx / d;
            double sin = dy / d;
            return new Vector()
            {
                X = g / d2 * cos,
                Y = g / d2 * sin
            };
        }

        public Vector GetFrictionalForce()
        {
            // 摩擦力は速度に比例 (比例定数 -m)
            const double m = 0.3d;
            return new Vector()
            {
                X = -m * this.V.X,
                Y = -m * this.V.Y
            };
        }

        public void MoveEular(double dt, Vector f)
        {
            // 質量は1とする
            this.R = new Vector()
            {
                X = this.R.X + dt * this.V.X,
                Y = this.R.Y + dt * this.V.Y
            };
            this.V = new Vector()
            {
                X = this.V.X + dt * f.X,
                Y = this.V.Y + dt * f.Y
            };
        }
    }
}
