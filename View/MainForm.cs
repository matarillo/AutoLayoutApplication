using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AutoLayoutApplication
{
    public partial class MainForm : Form
    {
        private NodeCollection nodes;
        private TimerSwitch timerSwitch;

        public MainForm(NodeCollection nodes, TimerSwitch timerSwitch, Size clientSize)
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.nodes = nodes;
            this.timerSwitch = timerSwitch;
            this.ClientSize = clientSize;
            nodes.PropertyChanged += nodes_PropertyChanged;
            timerSwitch.PropertyChanged += timerSwitch_PropertyChanged;
        }

        public void SetControllers(NodeController nodeController, TimerController timerController)
        {
            this.button1.Click += timerController.ButtonClick;
            this.timer1.Tick += nodeController.TimerTick;
            this.MouseDown += nodeController.MouseDown;
            this.MouseMove += nodeController.MouseMove;
            this.MouseUp += nodeController.MouseUp;
        }

        private void timerSwitch_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.timer1.Enabled = timerSwitch.Enabled;
            this.button1.Text = timerSwitch.Enabled ? "Stop" : "Start";
        }

        private void nodes_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Refresh();
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            this.Capture = true;
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            this.Capture = false;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            const int r = 15;
            e.Graphics.Clear(SystemColors.Control);
            foreach (Node n in this.nodes.Values)
            {
                // ロックされているノードは赤くする
                Brush br = this.nodes.IsLocked(n)
                    ? Brushes.Red
                    : Brushes.Green;
                // ノードは、n.R を中心とする半径 r の円で表す
                e.Graphics.FillEllipse(br,
                    ToScreenX(n.R.X) - r, ToScreenY(n.R.Y) - r,
                    2 * r, 2 * r);
                // 円の中にノード名を描く
                e.Graphics.DrawString(n.Name,
                    SystemFonts.DefaultFont, Brushes.White,
                    ToScreenX(n.R.X) - r / 2, ToScreenY(n.R.Y) - r / 2);
                foreach (Node nn in n.Neighbors)
                {
                    // エッジは、ノードの中心をつなぐ直線で表す
                    e.Graphics.DrawLine(Pens.Black,
                        ToScreenX(n.R.X), ToScreenY(n.R.Y),
                        ToScreenX(nn.R.X), ToScreenY(nn.R.Y));
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

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            this.nodes.PropertyChanged -= this.nodes_PropertyChanged;
            this.timerSwitch.PropertyChanged -= this.timerSwitch_PropertyChanged;
        }
    }
}
