using System;
using System.Drawing;
using System.Windows.Forms;

namespace AutoLayoutApplication
{
    public class NodeController
    {
        private NodeCollection nodes;
        private Size clientSize;

        public NodeController(NodeCollection nodes, Size clientSize)
        {
            this.nodes = nodes;
            this.clientSize = clientSize;
        }

        public void TimerTick(object sender, EventArgs e)
        {
            this.nodes.MoveAll();
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            this.nodes.Lock(ToNodeX(e.X), ToNodeY(e.Y));
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            this.nodes.Drag(ToNodeX(e.X), ToNodeY(e.Y));
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            this.nodes.Unlock();
        }

        private double ToNodeX(int x)
        {
            return (double)(x - this.clientSize.Width / 2);
        }

        private double ToNodeY(int y)
        {
            return (double)(y - this.clientSize.Height / 2);
        }
    }
}
