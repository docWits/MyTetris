using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyTetris
{
    public partial class Form1 : Form
    {
        xTetrix t = new xTetrix();
        //Shape curShape;
        public Form1()
        {
            InitializeComponent();
            t.Left = 0;
            t.Top = 0;
            t.Width = 809;
            t.Height = 500;
            t.curShape = new Shape(3, 0);
            t.linesRemoved = 0;
            
            label1.Text = "Score: " + t.score;
            label2.Text = "Lines: " + t.linesRemoved;
            
            //timer1.Interval = 100;
            //timer1.Tick += new EventHandler(update);
            t.OnCreateFigure += new EventHandler(update);

            this.Controls.Add(t);
        }

        private void update(object sender, EventArgs e)
        {
            t.ResetArea();
            if (!t.Collide())
            {
                t.curShape.MoveDown();
            }
            else
            {
                t.Merge();
                t.SliceMap(label1, label2);
                t.curShape.ResetShape(3, 0);
            }
            t.Merge();
            Invalidate();
        }
    }
}
