using System;

namespace AutoLayoutApplication
{
    static class Program
    {
        static void Main()
        {
            NodeCollection nodes = CreateGraph();
            Circularize(nodes, 200.0);

            for (int i = 0; i < 1000; i++)
            {
                nodes.MoveAll();
            }
            foreach (Node n in nodes.Values)
            {
                Console.WriteLine("{0} ({1}, {2})", n.Name, n.R.X, n.R.Y);
            }
        }

        static NodeCollection CreateGraph()
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

        static void Circularize(NodeCollection nodes, double r)
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

    }
}
