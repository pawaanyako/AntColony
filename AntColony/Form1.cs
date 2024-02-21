using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace AntColony
{
    public partial class Form1 : Form
    {
        private Pen pVertexes = new Pen(Color.FromArgb(255, 109, 118, 171));
        private Pen pEdges = new Pen(Color.FromArgb(255, 226, 230, 213));
        private SolidBrush bBlack = new SolidBrush(Color.Black);
        private SolidBrush bWhite = new SolidBrush(Color.White);
        private Font f = new Font("Arial", 16);
        private StringFormat stringFormat = new StringFormat();

        private Graph graph = new Graph();

        private static double defaultPheromone;
        private static double minPheromone;
        private static double evaporateCoeff;
        private static int numberOfAnts;
        private static int Q;
        private static float alpha;
        private static float beta;
        private static bool isSymmetrical;

        private void DrawVertexes(Graphics g)
        {
            float x, y, radius = 20;
            int id;
            pVertexes.Width = 2.2f;
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            for (int i = 0; i < graph.Vertices.Count(); i++)
            {
                x = graph.Vertices[i].X;
                y = graph.Vertices[i].Y;
                id = graph.Vertices[i].Id;
                g.FillEllipse(bWhite, x - radius, y - radius, radius * 2, radius * 2);
                g.DrawEllipse(pVertexes, x - radius, y - radius, radius * 2, radius * 2);
                g.DrawString(id.ToString(), f, bBlack, x + 1, y + 1, stringFormat);
            }
        }

        private void DrawEdges(Graphics g)
        {
            float x1, x2, y1, y2;
            pEdges.Width = 4f;
            foreach (var edge in graph.Edges)
            {
                pEdges.Width = 15f * (float)(edge.Pheromone);
                if (pEdges.Width > 22.5f) pEdges.Width = 22.5f;
                x1 = edge.Start.X;
                x2 = edge.End.X;
                y1 = edge.Start.Y;
                y2 = edge.End.Y;
                g.DrawLine(pEdges, x1, y1, x2, y2);
            }
        }

        private void PaintGraph(PictureBox pictureBox)
        {
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics g = Graphics.FromImage(pictureBox.Image);            
            pictureBox.BackColor = Color.White;
            DrawEdges(g);
            DrawVertexes(g);            
        }
        private void ResetRichTextBox()
        {
            isSymmetrical = checkBox1.Checked;
            richTextBox1.Text = "";
            if (graph.Edges.Count > 0) 
            {
                richTextBox1.Text = "Матрица смежности:\r\n";
                double[,] matrix = new double[graph.Vertices.Count, graph.Vertices.Count];
                for (int k = 0; k < graph.Vertices.Count; k++)
                {
                    for (int l = 0; l < graph.Vertices.Count; l++)
                    {
                        matrix[k, l] = 0;
                    }
                }
                switch (isSymmetrical)
                {
                    case true:
                        for (int i = 0; i < graph.Vertices.Count; i++)
                        {
                            for (int j = 0; j < graph.Vertices.Count; j++)
                            {
                                foreach (var edge in graph.Edges)
                                {
                                    if (edge.Start.Id == i && edge.End.Id == j)
                                    {
                                        matrix[i, j] = edge.Length;                                        
                                    }                                    
                                }
                            }                                                            
                        }
                        break;
                    case false:
                        for (int i = 0; i < graph.Vertices.Count; i++)
                        {
                            for (int j = i + 1; j < graph.Vertices.Count; j++)
                            {
                                foreach (var edge in graph.Edges)
                                {
                                    if (edge.Start.Id == i && edge.End.Id == j)
                                    {
                                        matrix[i, j] = edge.Length;
                                    }
                                }
                            }
                        }
                        break;
                }
                for (int k = 0; k < graph.Vertices.Count; k++)
                {
                    for (int l = 0; l < graph.Vertices.Count; l++)
                    {
                        if (isSymmetrical) richTextBox1.Text += matrix[k, l] + "\t";
                        else if (l < k) richTextBox1.Text += " \t";
                        else richTextBox1.Text += matrix[k, l] + "\t";
                    }
                    richTextBox1.Text += "\r\n";
                }
                richTextBox1.Text += "\r\n";
            }
        }
        private void UpdateRichTextBox(string _str)
        {
            richTextBox1.Text += _str + "\r\n";
        }

        public Form1()
        {
            InitializeComponent();            
            PaintGraph(pictureBox1);
            numericUpDown1.DecimalPlaces = 2;
            numericUpDown1.Increment = 0.05M;
            numericUpDown1.Value = 0.2M;
            numericUpDown1.Minimum = 0.05M;
            numericUpDown1.Maximum = 1.0M;

            numericUpDown2.DecimalPlaces = 2;
            numericUpDown2.Increment = 0.01M;
            numericUpDown2.Value = 0.05M;
            numericUpDown2.Minimum = 0.05M;
            numericUpDown2.Maximum = 5.0M;

            numericUpDown3.DecimalPlaces = 2;
            numericUpDown3.Increment = 0.05M;
            numericUpDown3.Value = 0.6M;
            numericUpDown3.Minimum = 0.1M;
            numericUpDown3.Maximum = 1.0M;

            numericUpDown4.Value = 5;

            numericUpDown5.Increment = 25;
            numericUpDown5.Value = 150;

            numericUpDown6.DecimalPlaces = 1;
            numericUpDown6.Increment = 0.5M;
            numericUpDown6.Value = 1;

            numericUpDown7.DecimalPlaces = 1;
            numericUpDown7.Increment = 0.5M;
            numericUpDown7.Value = 1;

            button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            graph.ClearGraph();
            PaintGraph(pictureBox1);
            ResetRichTextBox();
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            numericUpDown3.Enabled = true;
            numericUpDown4.Enabled = true;
            numericUpDown5.Enabled = true;
            numericUpDown6.Enabled = true;
            numericUpDown7.Enabled = true;
            pictureBox1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {            
            if (graph.Vertices.Count != 10)
            {
                int x = e.X; int y = e.Y;
                graph.Vertices.Add(new Vertex(x, y));
                PaintGraph(pictureBox1);
            }
            else MessageBox.Show("Слишком много вершин", "Ошибка");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (graph.Vertices.Count > 2) 
            {
                defaultPheromone = (double)numericUpDown1.Value;
                minPheromone = (double)numericUpDown2.Value;
                evaporateCoeff = (double)numericUpDown3.Value;
                numberOfAnts = (int)numericUpDown4.Value;
                Q = (int)numericUpDown5.Value;
                alpha = (float)numericUpDown6.Value;
                beta = (float)numericUpDown7.Value;

                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
                numericUpDown3.Enabled = false;
                numericUpDown4.Enabled = false;
                numericUpDown5.Enabled = false;
                numericUpDown6.Enabled = false;
                numericUpDown7.Enabled = false;
                pictureBox1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = true;
                checkBox1.Enabled = false;

                graph.CreateEdges();
                graph.ResetPheromones(defaultPheromone, minPheromone);
                ResetRichTextBox();

                AntColony antColony = new AntColony(graph, numberOfAnts, evaporateCoeff, Q, alpha, beta);
                int i = 0;
                while (i < 100)
                {
                    UpdateRichTextBox((i + 1).ToString() + antColony.Search());
                    i++;
                }
                PaintGraph(pictureBox1);                
            }
            else MessageBox.Show("Создайте больше двух вершин", "Ошибка");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            graph.ClearEdges();
            PaintGraph(pictureBox1);
            ResetRichTextBox();
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            numericUpDown3.Enabled = true;
            numericUpDown4.Enabled = true;
            numericUpDown5.Enabled = true;
            numericUpDown6.Enabled = true;
            numericUpDown7.Enabled = true;
            pictureBox1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;
            checkBox1.Enabled = true;
        }
    }
}
