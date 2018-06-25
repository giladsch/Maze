
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication35
{
    public class Map
    {
        public static Cell[,] grid;
        public static int size;


        public Map(Cell[,] Grid, int SIZE)
        {
            Map.grid = Grid;
            Map.size = SIZE;
        }

        public static int getMap(int x, int y)
        {

            if (x >= 0 && y >= 0 && x < size && y < size)
                return 0;
            else return -1;
        }


        static public void PrintSolution(ArrayList solutionPathList)
        {
            /*
            int yMax = Mapdata.GetUpperBound(0);
            int xMax = Mapdata.GetUpperBound(1);

            for (int j = 0; j <= yMax; j++)
            {
                for (int i = 0; i <= xMax; i++)
                {
                    bool solutionNode = false;
                    foreach (Node n in solutionPathList)
                    {
                        Node tmp = new Node(null, null, 0, i, j);

                        if (n.isMatch(tmp))
                        {
                            solutionNode = true;
                            break;
                        }
                    }
                    if (solutionNode)
                        Console.Write("o "); //solution path
                    else if (Map.getMap(i, j) == -1)
                        Console.Write("# "); //wall
                    else
                        Console.Write(". "); //road
                }
                Console.WriteLine("");
            }
             * */
        }
    }
}