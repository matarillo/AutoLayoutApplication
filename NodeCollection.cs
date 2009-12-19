using System.Collections.Generic;

namespace AutoLayoutApplication
{
    public class NodeCollection : Dictionary<string, Node>
    {
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

    }
}
