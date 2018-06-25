using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication35
{
    public class Node : IComparable
    {

        public int totalCost
        {
            get
            {
                return g + h;
            }
            set
            {
                totalCost = value;
            }
        }
        public int g;
        public int h;



        public int x;
        public int y;


        private Node _goalNode;
        public Node parentNode;
        private int gCost;


        public Node(Node parentNode, Node goalNode, int gCost, int x, int y)
        {

            this.parentNode = parentNode;
            this._goalNode = goalNode;
            this.gCost = gCost;
            this.x = x;
            this.y = y;
            InitNode();
        }

        private void InitNode()
        {
            this.g = (parentNode != null) ? this.parentNode.g + gCost : gCost;
            this.h = (_goalNode != null) ? (int)Euclidean_H() : 0;
        }

        private double Euclidean_H()
        {
            double xd = this.x - this._goalNode.x;
            double yd = this.y - this._goalNode.y;
            return Math.Sqrt((xd * xd) + (yd * yd));
        }

        public int CompareTo(object obj)
        {

            Node n = (Node)obj;
            int cFactor = this.totalCost - n.totalCost;
            return cFactor;
        }

        public bool isMatch(Node n)
        {
            if (n != null)
                return (x == n.x && y == n.y);
            else
                return false;
        }

        public ArrayList GetSuccessors()
        {
            ArrayList successors = new ArrayList();

            for (int xd = -1; xd <= 1; xd++)
            {
                for (int yd = -1; yd <= 1; yd++)
                {
                    if (Map.getMap(x + xd, y + yd) != -1)//לבדוק שלא מחוץ למערך ושיש אפשרות מעבר
                    {

                        int state = getState(Map.grid[x, y], Map.grid[x + xd, y + yd]);
                        bool sw = false;

                        if (state == 1)
                        {

                            if (!Map.grid[x, y].walls[0] && !Map.grid[x + xd, y + yd].walls[2])
                            {
                                sw = true;
                            }

                        }
                        else if (state == 2)
                        {
                            if (!Map.grid[x, y].walls[1] && !Map.grid[x + xd, y + yd].walls[3])
                            {
                                sw = true;
                            }

                        }
                        else if (state == 3)
                        {
                            if (!Map.grid[x, y].walls[2] && !Map.grid[x + xd, y + yd].walls[0])
                            {
                                sw = true;
                            }

                        }
                        else if (state == 4)
                        {
                            if (!Map.grid[x, y].walls[3] && !Map.grid[x + xd, y + yd].walls[1])
                            {
                                sw = true;
                            }
                        }
                        if (sw)
                        {
                            //Node n = new Node(this, this._goalNode, Map.getMap(x + xd, y + yd), x + xd, y + yd);                  
                            Node n = new Node(this, this._goalNode, 1, x + xd, y + yd);
                            if (!n.isMatch(this.parentNode) && !n.isMatch(this))
                                successors.Add(n);
                        }

                    }
                }
            }
            return successors;
        }

        public static int getState(Cell curr, Cell other)
        {
            int x = curr.x;
            int y = curr.y;

            int oX = other.x;
            int oY = other.y;
            /*
            // up 
            other
                 
              curr
              */
            if (x - oX == 0 && y - oY == 1)
            {
                return 1;
            }

            // left other--------<curr
            if (x - oX == 1 && y - oY == 0)
            {
                return 2;
            }

            /*
           // down 
           curr
              
              
             other
             */
            if (x - oX == 0 && y - oY == -1)
            {
                return 3;
            }

            // right curr----->other
            if (x - oX == -1 && y - oY == 0)
            {
                return 4;
            }
            return 0;
        }


    }
}
