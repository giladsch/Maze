using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

//https://www.codeproject.com/Articles/9880/Very-simple-A-algorithm-implementation

namespace WindowsFormsApplication35
{

    public partial class Form1 : Form
    {

        int height, width, size = 30, numOfBlocks, offset = 10;
        Graphics g;
        Cell[,] grid;
        static bool isShowed = false;
        Cell end;
        Cell start;
        Cell currPos;
        static bool clearRec = false;
        public Form1()
        {
            InitializeComponent();
            height = this.Height - 40;
            width = this.Width - 30;
            g = this.CreateGraphics();
            numOfBlocks = (((Math.Min(height, width)) / size) - 1);
            grid = new Cell[numOfBlocks, numOfBlocks];
            //grid = new Cell[height/size, width/size];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitArray();
        }
        public void InitArray()
        {

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new Cell(i, j, g, size, offset);
                }
            }
        }
        public void ShowGrid()
        {

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j].show();
                }
            }
        }

        public void HideGrid()
        {

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j].hide();
                }
            }
        }

        public void ResetGrid()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new Cell(i, j, g, size, offset);
                }
            }
        }

        public void clearGrid()
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j].resetBackColor();
                }
            }
            currPos.BackColor(0, 0);
        }
        public int getState(Cell curr, Cell other)
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

        public void getNeighbors(Cell curr)
        {
            int x = curr.x;
            int y = curr.y;

            if (y - 1 > 0 && !grid[x, y - 1].isViseted)
            {
                curr.neighbors.Add(grid[x, y - 1]);
            }
            if (y + 1 < numOfBlocks && !grid[x, y + 1].isViseted)
            {
                curr.neighbors.Add(grid[x, y + 1]);
            }
            if (x - 1 > 0 && !grid[x - 1, y].isViseted)
            {
                curr.neighbors.Add(grid[x - 1, y]);
            }
            if (x + 1 < numOfBlocks && !grid[x + 1, y].isViseted)
            {
                curr.neighbors.Add(grid[x + 1, y]);
            }

        }


        public void knockDownWall(Cell curr, Cell other, int state)
        {
            int x = curr.x;
            int y = curr.y;

            int oX = other.x;
            int oY = other.y;


            if (state == 1)
            {
                curr.walls[0] = false;
                other.walls[2] = false;

                grid[x, y].walls[0] = false;
                grid[oX, oY].walls[2] = false;
            }
            else if (state == 2)
            {
                curr.walls[1] = false;
                other.walls[3] = false;

                grid[x, y].walls[1] = false;
                grid[oX, oY].walls[3] = false;
            }
            else if (state == 3)
            {
                curr.walls[2] = false;
                other.walls[0] = false;

                grid[x, y].walls[2] = false;
                grid[oX, oY].walls[0] = false;
            }
            else if (state == 4)
            {
                curr.walls[3] = false;
                other.walls[1] = false;

                grid[x, y].walls[3] = false;
                grid[oX, oY].walls[1] = false;
            }

        }

        public void Generate(Cell start)
        {

            Random a = new Random();

            Cell curr = start;

            curr.setViseted();

            Stack<Cell> cells = new Stack<Cell>();

            cells.Push(curr);

            while (cells.Count > 0)
            {

                if (curr.neighbors.Count > 0)
                {
                    curr.resetNeighbors();
                }

                getNeighbors(curr);

                if (curr.neighbors.Count > 0)
                {
                    Cell temp = curr.neighbors[a.Next(0, curr.neighbors.Count)];
                    int state = getState(curr, temp);
                    knockDownWall(curr, temp, state);

                    cells.Push(curr);
                    curr = temp;
                    curr.setViseted();
                }
                else
                {
                    curr = cells.Pop();
                }
            }
        }


        public void aStar()
        {
            Map a = new Map(grid, numOfBlocks);

            ArrayList SolutionPathList = new ArrayList();

            //Create a node containing the goal state node_goal
            Node node_goal = new Node(null, null, 1, numOfBlocks - 1, numOfBlocks - 1);

            //Create a node containing the start state node_start
            Node node_start = new Node(null, node_goal, 1, 0, 0);


            //Create OPEN and CLOSED list
            SortedCostNodeList OPEN = new SortedCostNodeList();
            SortedCostNodeList CLOSED = new SortedCostNodeList();


            //Put node_start on the OPEN list
            OPEN.push(node_start);

            //while the OPEN list is not empty
            while (OPEN.Count > 0)
            {
                //Get the node off the open list 
                //with the lowest f and call it node_current
                Node node_current = OPEN.pop();

                //if node_current is the same state as node_goal we have found the solution;
                //break from the while loop;
                if (node_current.isMatch(node_goal))
                {
                    node_goal.parentNode = node_current.parentNode;
                    break;
                }

                //Generate each state node_successor that can come after node_current
                ArrayList successors = node_current.GetSuccessors();

                //for each node_successor or node_current
                foreach (Node node_successor in successors)
                {
                    grid[node_successor.x, node_successor.y].BackColor(node_successor.x, node_successor.y, Color.Blue);
                    //Thread.Sleep(50);
                    //Set the cost of node_successor to be the cost of node_current plus
                    //the cost to get to node_successor from node_current
                    //--> already set while we are getting successors

                    //find node_successor on the OPEN list
                    int oFound = OPEN.IndexOf(node_successor);

                    //if node_successor is on the OPEN list but the existing one is as good
                    //or better then discard this successor and continue
                    if (oFound > 0)
                    {
                        Node existing_node = OPEN.NodeAt(oFound);
                        if (existing_node.CompareTo(node_current) <= 0)
                            continue;
                    }


                    //find node_successor on the CLOSED list
                    int cFound = CLOSED.IndexOf(node_successor);

                    //if node_successor is on the CLOSED list but the existing one is as good
                    //or better then discard this successor and continue;
                    if (cFound > 0)
                    {
                        Node existing_node = CLOSED.NodeAt(cFound);
                        if (existing_node.CompareTo(node_current) <= 0)
                            continue;
                    }

                    //Remove occurences of node_successor from OPEN and CLOSED
                    if (oFound != -1)
                        OPEN.RemoveAt(oFound);
                    if (cFound != -1)
                        CLOSED.RemoveAt(cFound);

                    //Set the parent of node_successor to node_current;
                    //--> already set while we are getting successors

                    //Set h to be the estimated distance to node_goal (Using heuristic function)
                    //--> already set while we are getting successors

                    //Add node_successor to the OPEN list
                    OPEN.push(node_successor);

                }
                //Add node_current to the CLOSED list
                CLOSED.push(node_current);
            }


            //follow the parentNode from goal to start node
            //to find solution
            Node p = node_goal;
            // grid[p.x, p.y].BackColor(p.x, p.y);
            while (p != null)
            {
                grid[p.x, p.y].BackColor(p.x, p.y);
                SolutionPathList.Insert(0, p);
                p = p.parentNode;
            }

            //display solution
            /*
            Map.PrintSolution(SolutionPathList);
            Console.ReadLine();
             * */
        }
        //private void Form1_MouseClick(object sender, MouseEventArgs e)
        //{

        //    if (!isShowed)
        //    {
        //        isShowed = true;
        //        // ShowGrid();
        //    }


        //}

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(e.X.ToString() + " , " + e.Y.ToString());

            start = grid[0, 0];
            end = grid[numOfBlocks - 1, numOfBlocks - 1];
            currPos = start;       

            HideGrid();

            if (clearRec == true)
            {
                clearGrid();
            }

            ResetGrid();

            Generate(start);

            ShowGrid();

            currPos.BackColor(0, 0);



            /*
            start.removeFirst();
            */
            end.removeLast();

        }

        private bool solveMazeRecursion(int xPos, int yPos, bool[,] alreadySearched)
        {
            bool correctPath = false;
            //should the computer check this tile
            bool shouldCheck = true;
            //Check for out of boundaries
            if (xPos > numOfBlocks-1 || xPos < 0 || yPos > numOfBlocks-1 || yPos < 0)
                shouldCheck = false;
            else
            {
                //Check if at finish, not (0,0 and colored light blue)
                if (grid[xPos, yPos].CompareTo(end) && (xPos != 0 && yPos != 0))
                {
                    correctPath = true;
                    shouldCheck = false;
                }
                /*
                //Check for a wall
                if (grid[xPos, yPos].BackColor == Color.Black)
                    shouldCheck = false;
                 * */
                //Check if previously searched
                if (alreadySearched[xPos, yPos])
                    shouldCheck = false;
            }
            //Search the Tile
            if (shouldCheck)
            {

                //mark tile as searched
                alreadySearched[xPos, yPos] = true;
                //check up tile
                if (!grid[xPos, yPos].walls[0])
                {
                    grid[xPos, yPos - 1].BackColor(xPos, yPos - 1, Color.Blue);
                    //Thread.Sleep(50);
                    correctPath = correctPath || solveMazeRecursion(xPos, yPos - 1, alreadySearched);
                }
                //check left tile
                if (!grid[xPos, yPos].walls[1])
                {
                    grid[xPos - 1, yPos].BackColor(xPos - 1, yPos, Color.Blue);
                    //Thread.Sleep(50);
                    correctPath = correctPath || solveMazeRecursion(xPos - 1, yPos, alreadySearched);
                }
                //Check down tile
                if (!grid[xPos, yPos].walls[2])
                {
                    grid[xPos, yPos + 1].BackColor(xPos, yPos + 1, Color.Blue);
                    //Thread.Sleep(50);
                    correctPath = correctPath || solveMazeRecursion(xPos, yPos + 1, alreadySearched);
                }
                //Check right tile
                if (!grid[xPos, yPos].walls[3])
                {
                    grid[xPos + 1, yPos].BackColor(xPos, yPos + 1, Color.Blue);
                    //Thread.Sleep(50);
                    correctPath = correctPath || solveMazeRecursion(xPos + 1, yPos, alreadySearched);
                }


                /*
                alreadySearched[xPos, yPos] = true;
                //Check right tile
                correctPath = correctPath || solveMazeRecursion(xPos + 1, yPos, alreadySearched);
                //Check down tile
                correctPath = correctPath || solveMazeRecursion(xPos, yPos + 1, alreadySearched);
                //check left tile
                correctPath = correctPath || solveMazeRecursion(xPos - 1, yPos, alreadySearched);
                //check up tile
                correctPath = correctPath || solveMazeRecursion(xPos, yPos - 1, alreadySearched);
                 */



            }
            //make correct path gray
            if (correctPath)
                grid[xPos, yPos].BackColor(xPos, yPos);
            return correctPath;
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }

            else if (currPos != null)
            {
                int x = currPos.x;
                int y = currPos.y;

                if (e.KeyCode == Keys.R)
                {
                    //Create a previously searched array
                    bool[,] alreadySearched = new bool[numOfBlocks, numOfBlocks];
                    //Starts the recursive solver at tile (0,0). If false maze can not be solved.
                    if (!solveMazeRecursion(0, 0, alreadySearched))
                        MessageBox.Show("Maze can not be solved.");
                    clearRec = true;
                }
                else if (e.KeyCode == Keys.A)
                {
                    aStar();
                    clearRec = true;
                }
                else if (e.KeyCode == Keys.C)
                {
                    clearGrid();
                    ShowGrid();
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (!currPos.walls[0])
                    {
                        currPos.resetBackColor();
                        currPos.show();
                        currPos.LineVisited();
                        currPos.BackColor(x, --y);
                        currPos = grid[x, y];
                    }
                }
                else if (e.KeyCode == Keys.Left)
                {
                    if (!currPos.walls[1])
                    {
                        currPos.resetBackColor();
                        currPos.show();
                        currPos.LineVisited();
                        currPos.BackColor(--x, y);
                        currPos = grid[x, y];

                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (!currPos.walls[2])
                    {

                        currPos.resetBackColor();
                        currPos.show();
                        currPos.LineVisited();
                        currPos.BackColor(x, ++y);
                        currPos = grid[x, y];

                    }
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (!currPos.walls[3])
                    {

                        currPos.resetBackColor();
                        currPos.show();
                        currPos.LineVisited();
                        currPos.BackColor(++x, y);
                        currPos = grid[x, y];
                    }
                }
            }

        }
    }

}
