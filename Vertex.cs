using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dijkstra
{

    class Vertex : IComparable<Vertex>
    {
        public decimal? d = null; // вес/метка/кратчайший путь (как в Кормене атрибут d); значение null будет рассматриваться как положительная бесконечность
        public Vertex p = null; // предыдущая вершина (нужна, чтоб определить как раз-таки оптимальный маршрут (по каким конкретно ребрам двигаться))
        public String name = "";
        public List<Edge> edges = new List<Edge>();

        public Vertex(string[] arr)
        {
            decimal? temp = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                if ((temp = Convert.ToDecimal(arr[i])) > 0)
                    edges.Add(new Edge(i, temp));
            }
        }

        public int CompareTo(Vertex v)
        {
            if (this.d == null && v.d == null)
                return 0;

            if (this.d != null && v.d == null)
                return -1;

            if (this.d == null && v.d != null)
                return 1;
            
            return Convert.ToInt32(this.d - v.d);
        }
    }

    class Edge
    {
        public int vertex; // номер вершины (используется в списке Graph)
        public decimal? w; // вес до вершины

        public Edge(int new_vertex, decimal? new_w)
        {
            vertex = new_vertex;
            w = new_w;
        }
    }
}
