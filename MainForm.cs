using System;
using System.Drawing;
using System.Windows.Forms;

namespace AutoLayoutApplication
{
    public partial class MainForm : Form
    {
        private NodeCollection nodes;

        public MainForm()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.nodes = CreateGraph();
            Circularize(this.nodes, 200.0d);
        }

        private static NodeCollection CreateGraph()
        {
            return new NodeCollection()
                .Add("a", "a'", "b", "b'", "c", "c'",
                    "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                    "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
                    "20", "21", "22", "23", "24", "25", "26", "27", "28", "29")
                .Link("0,1", "0,10", "0,21", "1,2", "1,28",
                    "2,3", "2,6", "3,4", "3,b'", "4,5",
                    "4,8", "5,6", "5,7", "6,11", "7,14",
                    "7,c'", "8,9", "8,10", "9,16", "9,a'",
                    "10,29", "11,14", "11,28", "12,13", "12,20",
                    "12,27", "13,14", "13,19", "15,16", "15,18",
                    "15,29", "16,17", "17,18", "17,c", "18,26",
                    "19,22", "19,28", "20,22", "20,25", "21,24",
                    "21,29", "22,23", "23,24", "23,a", "24,25",
                    "25,26", "26,27", "27,b");
        }

        private static void Circularize(NodeCollection nodes, double r)
        {
            int count = nodes.Count;
            double dtheta = 2.0d * Math.PI / (double)count;
            double theta = 0.0d;
            foreach (Node n in nodes.Values)
            {
                n.R = new Vector()
                {
                    X = r * Math.Cos(theta),
                    Y = r * Math.Sin(theta)
                };
                n.V = new Vector()
                {
                    X = 0.0d,
                    Y = 0.0d
                };
                theta += dtheta;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.ClientSize = new Size(600, 600);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = !(this.timer1.Enabled);
            this.button1.Text = (this.timer1.Enabled) ? "Stop" : "Start";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.nodes.MoveAll();
            this.Refresh();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            const int r = 15;
            e.Graphics.Clear(SystemColors.Control);
            foreach (Node n in this.nodes.Values)
            {
                Brush br = Brushes.Green;
                e.Graphics.FillEllipse(br, ToScreenX(n.R.X) - r, ToScreenY(n.R.Y) - r, 2 * r, 2 * r);
                e.Graphics.DrawString(n.Name, SystemFonts.DefaultFont, Brushes.White, ToScreenX(n.R.X) - r / 2, ToScreenY(n.R.Y) - r / 2);
                foreach (Node nn in n.Neighbors)
                {
                    e.Graphics.DrawLine(Pens.Black, ToScreenX(n.R.X), ToScreenY(n.R.Y), ToScreenX(nn.R.X), ToScreenY(nn.R.Y));
                }
            }
        }

        private int ToScreenX(double x)
        {
            return (int)x + this.ClientSize.Width / 2;
        }

        private int ToScreenY(double y)
        {
            return (int)y + this.ClientSize.Height / 2;
        }
    }
}
