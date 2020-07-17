using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Dijkstra
{
    public partial class Form1 : Form
    {
        private List<Vertex> Graph = new List<Vertex>();
        private List<Vertex> S;
        private List<Vertex> Q;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DGV.RowCount = DGV.ColumnCount = Convert.ToInt32(Rows.Value);
            DGV[0, 0].Value = @"From \ To";
            DGV[0, 0].ReadOnly = DGV[1, 1].ReadOnly = DGV[2, 2].ReadOnly = true;            
        }

        private void Rows_ValueChanged(object sender, EventArgs e)
        {
            int value = Convert.ToInt32(Rows.Value);
            DGV.RowCount = DGV.ColumnCount = value;
            DGV[value - 1, value - 1].ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int value = Convert.ToInt32(Rows.Value);
            String[] buff = new String[value];
            textBox1.Clear();
            Graph.Clear();

            for (int i = 1; i < value; i++)
            {
                for (int j = 1; j < value; j++)
                {
                    if (i != j)
                        buff[j - 1] = Convert.ToString(DGV[j, i].Value);
                    else
                        buff[j - 1] = "0";
                }

                Graph.Add(new Vertex(buff));
            }

            for (int i = 1; i < value; i++)
                Graph[i - 1].name = Convert.ToString(DGV[i, 0].Value);
                
            
            Vertex u = null;

            Graph[0].d = 0;

            S = new List<Vertex>();
            Q = new List<Vertex>(Graph);

            while (Q.Count > 0)
            {
                u = Q.Min(); // Min() (метод расширения) требует реализацию IComparable<T> от Vertex
                Q.Remove(u);
                S.Add(u);

                for (int i = 0; i < u.edges.Count; i++)
                {

                    if (!S.Contains(Graph[u.edges[i].vertex]))
                    {
                        if (Graph[u.edges[i].vertex].d == null || Graph[u.edges[i].vertex].d > u.d + u.edges[i].w)
                        {
                            Graph[u.edges[i].vertex].d = u.d + u.edges[i].w;
                            Graph[u.edges[i].vertex].p = u;
                        }
                    }

                }
            }

            Stack<Vertex> stack = new Stack<Vertex>();
            Vertex v = Graph[Graph.Count - 1];

            textBox1.Text += "Shortest path length is " + v.d + Environment.NewLine;
            
            
            while (v != null)
            {
                stack.Push(v);
                v = v.p;
            }

            String str = "";
            while (stack.Count > 0)
                str += stack.Pop().name + " ";

            textBox1.Text += "Shortest path: " + str;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Open_D.FileName = "graph.txt";

            if (Open_D.ShowDialog() == DialogResult.OK)
            {
                Regex rgx = new Regex(@"\s+");
                String[] buff;
                textBox1.Clear();

                using (StreamReader sr = new StreamReader(File.Open(Open_D.FileName, FileMode.Open)))
                {
                    buff = rgx.Split(sr.ReadLine());
                    Rows.Value = Convert.ToDecimal(buff.Length) + 1;
                    int value = Convert.ToInt32(Rows.Value);

                    for (int i = 1; i < value; i++)
                    {
                        DGV[i, 0].Value = DGV[0, i].Value = buff[i - 1];
                    }

                    int j = 1; // пробегается по строкам, k - по столбцам

                    while (!sr.EndOfStream)
                    {
                        buff = rgx.Split(sr.ReadLine());

                        for (int k = 0; k + 1 < value; k++)
                        {
                            if (k != j - 1)
                                DGV[k + 1, j].Value = buff[k];
                        }

                        j++;
                    }

                    
                }
            }

            button1_Click(sender, e);
        }


    }
}
