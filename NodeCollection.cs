using System.Collections.Generic;

namespace AutoLayoutApplication
{
    public class NodeCollection : Dictionary<string, Node>
    {
        private Node lockedNode;

        public NodeCollection Add(params string[] names)
        {
            foreach (string name in names)
            {
                this[name] = new Node(name);
            }
            return this;
        }

        public NodeCollection Link(params string[] links)
        {
            foreach (string link in links)
            {
                string[] n = link.Split(',');
                this[n[0]].Neighbors.Add(this[n[1]]);
                this[n[1]].Neighbors.Add(this[n[0]]);
            }
            return this;
        }

        public void MoveAll()
        {
            const double dt = 0.1d;
            foreach (Node n in this.Values)
            {
                if (n == this.lockedNode)
                {
                    continue;
                }
                Vector f = new Vector();
                foreach (Node nn in n.Neighbors)
                {
                    f += n.GetSpringForce(nn);
                }
                foreach (Node nn in this.Values)
                {
                    if (n != nn)
                    {
                        f += n.GetReplusiveForce(nn);
                    }
                }
                f += n.GetFrictionalForce();
                n.MoveEular(dt, f);
            }
        }


        public void Lock(double x, double y)
        {
            // 指定した座標に一番近いノードをロックする
            Node nearlestNode = null;
            double nearlestD2 = double.PositiveInfinity;
            foreach (Node n in this.Values)
            {
                double d2 = (n.R.X - x) * (n.R.X - x) + (n.R.Y - y) * (n.R.Y - y);
                if (d2 < nearlestD2)
                {
                    nearlestNode = n;
                    nearlestD2 = d2;
                }
            }
            this.lockedNode = nearlestNode;
            nearlestNode.R = new Vector
            {
                X = x,
                Y = y
            };
            nearlestNode.V = new Vector();
        }

        public void Drag(double x, double y)
        {
            if (this.lockedNode == null)
            {
                return;
            }
            this.lockedNode.R = new Vector
            {
                X = x,
                Y = y
            };
        }

        public void Unlock()
        {
            this.lockedNode = null;
        }

        public bool IsLocked(Node n)
        {
            if (n == null) return false;
            return (n == this.lockedNode);
        }
    }
}
