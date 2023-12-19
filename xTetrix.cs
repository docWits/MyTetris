using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyTetris
{
    public class xTetrix:Control
    {

        int size = 25;

        public  Shape curShape;
        public static int[,] map = new int[16, 8];
        public int linesRemoved;
        public int score;
        public int Interval;
        private EventHandler m_OnCreateFigure;

        public event EventHandler OnCreateFigure
        {
            add { m_OnCreateFigure += value; }
            remove { m_OnCreateFigure -= value; }
        }

        protected void CreateFigure()
        {
            if (m_OnCreateFigure != null)
                m_OnCreateFigure(this, EventArgs.Empty);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.W:
                    ResetArea();
                    curShape.RotateShape();
                    break;
                case Keys.Space:
                    break;
                case Keys.D:
                    if (!CollideHor(1))
                    {
                        ResetArea();
                        curShape.MoveRight();
                        Merge();
                        Invalidate();
                    }
                    break;
                case Keys.A:
                    if (!CollideHor(-1))
                    {
                        ResetArea();
                        curShape.MoveLeft();
                        Merge();
                        Invalidate();
                    }
                    break;
            }
            CreateFigure();
            Invalidate();
        }

        public  void ShowNextShape(Graphics e)
        {
            for (int i = 0; i < curShape.sizeNextMatrix; i++)
            {
                for (int j = 0; j < curShape.sizeNextMatrix; j++)
                {
                    if (curShape.nextMatrix[i, j] == 1)
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(300 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (curShape.nextMatrix[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.Yellow, new Rectangle(300 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (curShape.nextMatrix[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.Green, new Rectangle(300 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (curShape.nextMatrix[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.Blue, new Rectangle(300 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (curShape.nextMatrix[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.Purple, new Rectangle(300 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                }
            }
        }

        public  void DrawMap(Graphics e)
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (map[i, j] == 1)
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.Yellow, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.Green, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.Blue, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                    if (map[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.Purple, new Rectangle(50 + j * (size) + 1, 50 + i * (size) + 1, size - 1, size - 1));
                    }
                }
            }
        }

        public  void Merge()
        {
            for (int i = curShape.y; i < curShape.y + curShape.sizeMatrix; i++)
            {
                for (int j = curShape.x; j < curShape.x + curShape.sizeMatrix; j++)
                {
                    if (curShape.matrix[i - curShape.y, j - curShape.x] != 0)
                        map[i, j] = curShape.matrix[i - curShape.y, j - curShape.x];
                }
            }
        }


        public void ResetArea()
        {
            for (int i = curShape.y; i < curShape.y + curShape.sizeMatrix; i++)
            {
                for (int j = curShape.x; j < curShape.x + curShape.sizeMatrix; j++)
                {
                    if (i >= 0 && j >= 0 && i < 16 && j < 8)
                    {
                        if (curShape.matrix[i - curShape.y, j - curShape.x] != 0)
                        {
                            map[i, j] = 0;
                        }
                    }
                }
            }
        }

        public bool Collide()
        {
            for (int i = curShape.y + curShape.sizeMatrix - 1; i >= curShape.y; i--)
            {
                for (int j = curShape.x; j < curShape.x + curShape.sizeMatrix; j++)
                {
                    if (curShape.matrix[i - curShape.y, j - curShape.x] != 0)
                    {
                        if (i + 1 == 16)
                            return true;
                        if (map[i + 1, j] != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public  bool CollideHor(int dir)
        {
            for (int i = curShape.y; i < curShape.y + curShape.sizeMatrix; i++)
            {
                for (int j = curShape.x; j < curShape.x + curShape.sizeMatrix; j++)
                {
                    if (curShape.matrix[i - curShape.y, j - curShape.x] != 0)
                    {
                        if (j + 1 * dir > 7 || j + 1 * dir < 0)
                            return true;

                        if (map[i, j + 1 * dir] != 0)
                        {
                            if (j - curShape.x + 1 * dir >= curShape.sizeMatrix || j - curShape.x + 1 * dir < 0)
                            {
                                return true;
                            }
                            if (curShape.matrix[i - curShape.y, j - curShape.x + 1 * dir] == 0)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public  void SliceMap(Label label1, Label label2)
        {
            int count = 0;
            int curRemovedLines = 0;
            for (int i = 0; i < 16; i++)
            {
                count = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (map[i, j] != 0)
                        count++;
                }
                if (count == 8)
                {
                    curRemovedLines++;
                    for (int k = i; k >= 1; k--)
                    {
                        for (int o = 0; o < 8; o++)
                        {
                            map[k, o] = map[k - 1, o];
                        }
                    }
                }
            }
            for (int i = 0; i < curRemovedLines; i++)
            {
                score += 10 * (i + 1);
            }
            linesRemoved += curRemovedLines;

            label1.Text = "Score: " + score;
            label2.Text = "Lines: " + linesRemoved;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Black, 1);
            Brush brush = Brushes.LightGray;

            for (int i = 0; i <= 16; i++)
            {
                g.DrawLine(Pens.Black, new Point(50, 50 + i * size), new Point(50 + 8 * size, 50 + i * size));
            }
            for (int i = 0; i <= 8; i++)
            {
                g.DrawLine(Pens.Black, new Point(50 + i * size, 50), new Point(50 + i * size, 50 + 16 * size));
            }

            
            DrawMap(e.Graphics);
            ShowNextShape(e.Graphics);

        }

    }
}
