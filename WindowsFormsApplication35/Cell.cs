using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

//https://www.codeproject.com/Articles/9880/Very-simple-A-algorithm-implementation

namespace WindowsFormsApplication35
{

    public class Cell
    {
        public int x;
        public int y;
        public bool[] walls;
        public bool isViseted;
        Graphics g = null;
        Pen p = null;
        public int size;
        public int offset;
        public List<Cell> neighbors;

        public Cell()
        {
            this.isViseted = false;
            this.x = 0;
            this.y = 0;
            this.walls = Enumerable.Repeat(true, 4).ToArray();
            this.size = 0;
            this.offset = 0;
            neighbors = new List<Cell>();

            this.g = null;
            this.p = null;

        }

        public Cell(int x, int y, Graphics gr, int size, int offset)
        {
            this.isViseted = false;
            this.x = x;
            this.y = y;
            this.walls = Enumerable.Repeat(true, 4).ToArray();
            this.size = size;
            this.offset = offset;
            neighbors = new List<Cell>();

            if (this.g == null)
            {
                this.g = gr;
            }

            if (this.p == null)
            {
                this.p = new Pen(Color.Black, 1f);
            }
        }

        public Cell(int x, int y, Graphics gr, int size, int offset, bool[] walls)
        {
            this.isViseted = false;
            this.x = x;
            this.y = y;
            this.walls = walls;
            this.size = size;
            this.offset = offset;
            neighbors = new List<Cell>();

            if (this.g == null)
            {
                this.g = gr;
            }

            if (this.p == null)
            {
                this.p = new Pen(Color.Black, 1f);
            }
        }

        public void setViseted()
        {
            this.isViseted = true;
        }
        public void resetNeighbors()
        {
            this.neighbors = new List<Cell>();
        }

        public void show()
        {

            int startX = ((this.x) * this.size) + offset;
            int startY = ((this.y) * this.size) + offset;

            // UP
            if (this.walls[0])
            {
                g.DrawLine(p, startX, startY, startX + size, startY);
            }

            // LEFT
            if (this.walls[1])
            {
                g.DrawLine(p, startX, startY, startX, startY + size);
            }

            //BOTTOM
            if (this.walls[2])
            {
                g.DrawLine(p, startX, startY + size, startX + size, startY + size);
            }

            //RIGHT
            if (this.walls[3])
            {
                g.DrawLine(p, startX + size, startY + size, startX + size, startY);
            }
        }

        public void hide()
        {
            int startX = ((this.x) * this.size) + offset;
            int startY = ((this.y) * this.size) + offset;

            Pen gr = new Pen(Color.Silver, 1f);

            g.DrawLine(gr, startX, startY, startX + size, startY);

            g.DrawLine(gr, startX, startY, startX, startY + size);

            g.DrawLine(gr, startX, startY + size, startX + size, startY + size);

            g.DrawLine(gr, startX + size, startY + size, startX + size, startY);

        }
        public void resetBackColor()
        {
            int startX = ((this.x) * this.size) + offset;
            int startY = ((this.y) * this.size) + offset;

            Rectangle rect = new Rectangle(startX, startY, size, size);
            g.FillRectangle(Brushes.Silver, rect);
        }
        public void removeFirst()
        {
            int startX = ((this.x) * this.size) + offset;
            int startY = ((this.y) * this.size) + offset;

            Pen gr = new Pen(Color.Silver, 1f);


            g.DrawLine(gr, startX, startY, startX + size, startY);
        }
        public void removeLast()
        {
            int startX = ((this.x) * this.size) + offset;
            int startY = ((this.y) * this.size) + offset;

            Pen gr = new Pen(Color.Silver, 1f);

            g.DrawLine(gr, startX, startY + size, startX + size, startY + size);

        }
        public void BackColor(int x, int y)
        {
            int startX = (x * this.size) + offset;
            int startY = (y * this.size) + offset;

            Rectangle rect = new Rectangle(startX, startY, size, size);

            Brush brush = new SolidBrush(Color.FromArgb(128, 255, 0, 0));

            // g.FillRectangle(Brushes.Red, rect);
            g.FillRectangle(brush, rect);
        }
        public void BackColor(int x, int y, Color color)
        {
            int startX = (x * this.size) + offset;
            int startY = (y * this.size) + offset;

            Rectangle rect = new Rectangle(startX, startY, size, size);

            Brush brush = new SolidBrush(Color.FromArgb(128, color.R, color.G, color.B));
            //Brush brush = new SolidBrush(color);

            // g.FillRectangle(Brushes.Red, rect);
            g.FillRectangle(brush, rect);
        }
        public void LineVisited()
        {
            /*
            int startX = ((this.x) * this.size) + offset;
            int startY = ((this.y) * this.size) + offset;

            Pen gr = new Pen(Color.Black, 2f);


            g.DrawLine(gr, startX + offset, (startY + size + offset) / 2, startX + size + offset, (startY + size + offset) / 2);
              */

        }
        public bool CompareTo(Cell other)
        {
            return (this.x == other.x && this.y == other.y);
        }
    }
}
