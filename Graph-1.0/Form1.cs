using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;


namespace Graph_1._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //test
        public class Vertex
        {
            public bool wasVisited;
            public string label;

            public Vertex(string label)
            {
                this.label = label;
                wasVisited = false;
            }
        }
        public class Graph
        {
            private const int NUM_VERTICES = 15;
            private Vertex[] vertices;
            private int[,] adjMatrix;
            private int numVerts;

            public Graph()
            {
                vertices = new Vertex[NUM_VERTICES];
                adjMatrix = new int[NUM_VERTICES, NUM_VERTICES];
                numVerts = 0;
                for (int j = 0; j < NUM_VERTICES; j++)
                {
                    for (int k = 0; k < NUM_VERTICES; k++)
                    {
                        adjMatrix[j, k] = 0;
                    }
                }
            }

            public bool AddVertex(string label)
            {
                try
                {
                    vertices[numVerts] = new Vertex(label);
                    numVerts++;
                    return true;
                }
                catch
                {
                    MessageBox.Show("ERR01");
                    return false;
                }
            }

            public void AddEdge(int start, int eend)
            {
                try
                {
                    adjMatrix[start, eend] = 1;
                }
                catch
                {
                    MessageBox.Show("Error");
                }
            }

            public bool HasEdge(int start, int end)
            {
                if (adjMatrix[start, end] == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public string ShowVertex(int v)
            {
                return (vertices[v].label);
            }

            public string ShowMatrix()
            {
                if (numVerts == 0)
                {
                    return "";
                }

                string s = "   |";
                for (int i = 0; i < numVerts; i++)
                {
                    s += string.Format(" {0} |", ShowVertex(i));
                }
                s += "\n";

                for (int i = 0; i <= numVerts; i++)
                {
                    s += "===|";
                }
                s += "\n";

                for (int j = 0; j < numVerts; j++)
                {
                    s += string.Format(" {0} |", ShowVertex(j));
                    for (int k = 0; k < numVerts; k++)
                    {
                        if (HasEdge(j, k))
                        {
                            s += " 1 |";
                        }
                        else
                        {
                            s += " 0 |";
                        }
                    }
                    s += "\n";
                }
                return s;
            }

      public void ClearGraph()
            {
                for (int i = 0; i < NUM_VERTICES; i++)
                {
                    vertices[i] = null;
                }

                numVerts = 0;
                for (int j = 0; j < NUM_VERTICES; j++)
                {
                    for (int k = 0; k < NUM_VERTICES; k++)
                    {
                        adjMatrix[j, k] = 0;
                    }
                }
            }

            public void DelEdge(int start, int eend)
            {
                try
                {
                    adjMatrix[start, eend] = 0;
                }
                catch
                {
                    MessageBox.Show("Error");
                }
            }

            private void MoveRow(int row, int length)
            {
                for (int col = 0; col < length; col++)
                {
                    adjMatrix[row, col] = adjMatrix[row + 1, col];
                }
            }

            private void MoveCol(int col, int length)
            {
                for (int row = 0; row < length; row++)
                {
                    adjMatrix[row, col] = adjMatrix[row, col + 1];
                }
            }

            public bool DelVertex(int vert)
            {
                try
                {
                    if (numVerts == 0)
                    {
                        return true;
                    }

                    if (vert != numVerts - 1)
                    {
                        for (int j = vert; j < numVerts - 1; j++)
                        {
                            vertices[j] = vertices[j + 1];
                        }

                        for (int row = vert; row < numVerts - 1; row++)
                        {
                            MoveRow(row, numVerts);
                        }

                        for (int col = vert; col < numVerts - 1; col++)
                        {
                            MoveCol(col, numVerts);
                        }
                    }
                    numVerts--;
                    return true;
                }
                catch
                {
                    MessageBox.Show("Error");
                    return false;
                }
            }

            public int Count()
            {
                return numVerts;
            }

            //Търсене в дълбочина
            private int GetUnvisitedVertex(int v)
            {
                for (int j = 0; j < numVerts; j++)
                {
                    if ((adjMatrix[v, j] == 1) && (vertices[j].wasVisited == false))
                    {
                        return j;
                    }
                }
                return -1;
            }

            public string DFS()
            {
                vertices[0].wasVisited = true;
                string s = ShowVertex(0) + " -> ";
                Stack gStack = new Stack();
                gStack.Push(0);
                int v;
                while (gStack.Count > 0)
                {
                    v = GetUnvisitedVertex((int)gStack.Peek());
                    if (v == -1)
                    {
                        gStack.Pop();
                    }
                    else
                    {
                        vertices[v].wasVisited = true;
                        s += ShowVertex(v) + " -> ";
                        gStack.Push(v);
                    }
                }
                for (int j = 0; j < numVerts; j++)
                {
                    vertices[j].wasVisited = false;
                }

                return s;
            }

            //Търсене в ширина
            public string BFS()
            {
                Queue gQueue = new Queue();
                vertices[0].wasVisited = true;
                string s = ShowVertex(0) + " -> ";
                gQueue.Enqueue(0);
                int vParent, vChild;
                while (gQueue.Count > 0)
                {
                    vParent = (int)gQueue.Dequeue();
                    vChild = GetUnvisitedVertex(vParent);
                    while (vChild != -1)
                    {
                        vertices[vChild].wasVisited = true;
                        s += ShowVertex(vChild) + " -> ";
                        gQueue.Enqueue(vChild);
                        vChild = GetUnvisitedVertex(vParent);
                    }
                }
                for (int i = 0; i < numVerts; i++)
                {
                    vertices[i].wasVisited = false;
                }

                return s;
            }

           

            public int NoSuccessors()
            {
                bool isEdge;
                for (int row = 0; row < numVerts; row++)
                {
                    isEdge = false;
                    for (int col = 0; col < numVerts; col++)
                    {
                        if (adjMatrix[row, col] > 0)
                        {
                            isEdge = true;
                            break;
                        }
                    }
                    if (!(isEdge))
                    {
                        return row;
                    }
                }
                return -1;
            }

        }

        Graph g = new Graph();

        private void CreateGraph(string[] items)
        {
            g.ClearGraph();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(items);
            listBox1.SelectedIndex = 0;
            listBox2.Items.Clear();
            listBox2.Items.AddRange(items);
            listBox2.SelectedIndex = 0;
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(items);
            comboBox1.SelectedIndex = 0;
            foreach (string element in items)
            {
                g.AddVertex(element);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btn1_Click(object sender, EventArgs e)
        {
            string[] items = new string[] { "A", "B", "C", "D", "E", "F" };
            CreateGraph(items);
            g.AddEdge(0, 1);
            g.AddEdge(0, 2);
            g.AddEdge(1, 3);
            g.AddEdge(3, 4);
            g.AddEdge(4, 5);
            g.AddEdge(1, 5);

            richTextBox1.Text = g.ShowMatrix();
        }

        private void btnDFS_Click(object sender, EventArgs e)
        {
            MessageBox.Show("DFS: \n" + g.DFS());
        }

        private void btnBFS_Click(object sender, EventArgs e)
        {
            MessageBox.Show("BFS: \n" + g.BFS());
        }

        private void btnAddNode_Click(object sender, EventArgs e)
        {
            string labelVertex = textBox1.Text;
            if (labelVertex.Length > 0)
            {
                if (g.AddVertex(labelVertex))
                {
                    listBox1.Items.Add(labelVertex);
                    listBox2.Items.Add(labelVertex);
                    comboBox1.Items.Add(labelVertex);

                    textBox1.Text = "";
                    richTextBox1.Text = g.ShowMatrix();
                }
            }
            else
            {
                MessageBox.Show("No label");
            }
        }

        private void btnRemoveNode_Click(object sender, EventArgs e)
        {
            if (g.Count() == 0)
            {
                return;
            }

            int row = int.Parse(comboBox1.SelectedIndex.ToString());

            if (row > -1 && g.DelVertex(row))
            {
                comboBox1.Items.Remove(comboBox1.SelectedIndex.ToString());
                int count = comboBox1.Items.Count;
                string[] items = new string[count];
                for (int i = 0; i < count; i++)
                {
                    items[i] = comboBox1.Items[i].ToString();
                }

                listBox1.Items.Clear();
                listBox1.Items.AddRange(items);

                listBox2.Items.Clear();
                listBox2.Items.AddRange(items);

                comboBox1.Text = "";

                richTextBox1.Text = g.ShowMatrix();
                if (g.Count() > 0)
                {
                    listBox1.SelectedIndex = 0;
                    listBox2.SelectedIndex = 0;
                    comboBox1.SelectedIndex = 0;
                }
            }
        }

        private void btnAddArc_Click(object sender, EventArgs e)
        {
            int row = int.Parse(listBox1.SelectedIndex.ToString());
            int col = int.Parse(listBox2.SelectedIndex.ToString());
            g.AddEdge(row, col);
            richTextBox1.Text = g.ShowMatrix();
        }

        private void btnRemoveArc_Click(object sender, EventArgs e)
        {
            int row = int.Parse(listBox1.SelectedIndex.ToString());
            int col = int.Parse(listBox2.SelectedIndex.ToString());
            g.DelEdge(row, col);
            richTextBox1.Text = g.ShowMatrix();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();

        }
    }
}
