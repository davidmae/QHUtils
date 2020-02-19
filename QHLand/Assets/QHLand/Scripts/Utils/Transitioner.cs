using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QHLand
{

    public class Transitioner
    {
        private TransitionPath transition;

        private static Transitioner instance;
        public bool firstTransition = false;

        private Transitioner()
        {
        }

        public static Transitioner Instance
        {
            get
            {
                if (instance == null)
                    instance = new Transitioner();
                return instance;
            }
        }

        public void DrawTransitions()
        {
            WorldManager worldInstance = WorldManager.worldInstance;

            transition = new TransitionPath(worldInstance);
            transition.Calculate(0, 0);

            Transition[,] path = transition.transitionGrid;
            Chunk[,] chunks = worldInstance._chunks;

            path[0, 0].targetBy.RemoveAt(0);


            for (int i = 0; i < path.GetLength(0); ++i)
            {
                for (int j = 0; j < path.GetLength(1); ++j)
                {
                    Transition point = path[i, j];
                    Material matOrigin = chunks[point.origin[0], point.origin[1]].chunkTerrain.GetComponent<TerrainManager>().terrain.materialTemplate;

                    foreach (int[] target in point.target)
                    {
                        if (point.origin[0] < target[0]) chunks[target[0], target[1]].SetMaterialTransition(matOrigin, 1, 3);
                        if (point.origin[0] > target[0]) chunks[target[0], target[1]].SetMaterialTransition(matOrigin, 3, 3);
                        if (point.origin[1] < target[1]) chunks[target[0], target[1]].SetMaterialTransition(matOrigin, 4, 3);
                        if (point.origin[1] > target[1]) chunks[target[0], target[1]].SetMaterialTransition(matOrigin, 2, 3);
                    }

                }
            }

            firstTransition = true;
        }

    }



    public class TransitionPath
    {
        //private WorldManager worldInstance;
        public int actualDir; //=0 top-bottom ::: =1 right-left 
        public int[] actualPos;
        public int[] limits;
        public Transition lastTransition;
        public Transition[,] transitionGrid;

        public TransitionPath(WorldManager worldInstance)
        {
            //this.worldInstance = worldInstance;
            actualDir = 0;
            actualPos = new int[2] { 0, 0 };
            limits = new int[2] { worldInstance.chunksX - 1, worldInstance.chunksY - 1 };
            transitionGrid = new Transition[limits[0] + 1, limits[1] + 1];
            lastTransition = new Transition(0, 0, 0, 0);
        }

        public void Calculate(int posx, int posy)
        {
            if (posx < 0 || posx > limits[0] || posy < 0 || posy > limits[1] ||
                (transitionGrid[posx, posy] != null && ((transitionGrid[posx, posy].targetBy.Count == 2) || (transitionGrid[posx, posy].Contains(lastTransition.origin)))))
            {
                transitionGrid[lastTransition.origin[0], lastTransition.origin[1]].Clear();
                return;
            }

            if (transitionGrid[posx, posy] == null)
                transitionGrid[posx, posy] = new Transition();

            transitionGrid[posx, posy].targetBy.Add(lastTransition.origin);

            if (transitionGrid[posx, posy].transited == 1)
                return;

            transitionGrid[posx, posy].transited = 1;

            List<int[]> movsList = new List<int[]>();
            int max_movs = GetMovs(posx, posy, movsList);

            Utils.RandomizeArray(ref movsList);

            for (int i = 0; i < max_movs; ++i)
            {
                int nextDir = movsList[i][0];
                int toDir = movsList[i][1];

                int vx = 0, vy = 0;

                if (nextDir == 0) //top or bottom
                {
                    if (toDir == 0)
                    { vx = 0; vy = 1; }
                    else
                    { vx = 0; vy = -1; }
                }

                if (nextDir == 1) //right or left
                {
                    if (toDir == 0) //right
                    { vx = 1; vy = 0; }
                    else
                    { vx = -1; vy = 0; }
                }


                lastTransition = new Transition(posx, posy, posx + vx, posy + vy);
                transitionGrid[posx, posy].CreateTransition(posx, posy, posx + vx, posy + vy);
                Calculate(posx + vx, posy + vy);
            }

        }

        private int GetMovs(int x, int y, List<int[]> movsList)
        {
            if (x == limits[0])
            {
                movsList.Add(new int[2] { 1, 1 }); //left

                if (y == limits[1])
                {
                    movsList.Add(new int[2] { 0, 1 }); //bottom    
                }
                else if (y == 0)
                {
                    movsList.Add(new int[2] { 0, 0 }); //up
                }
                else
                {
                    movsList.Add(new int[2] { 0, 1 }); //bottom
                    movsList.Add(new int[2] { 0, 0 }); //up
                }
            }
            else if (x == 0)
            {
                movsList.Add(new int[2] { 1, 0 }); //right

                if (y == limits[1])
                {
                    movsList.Add(new int[2] { 0, 1 }); //bottom    
                }
                else if (y == 0)
                {
                    movsList.Add(new int[2] { 0, 0 }); //up
                }
                else
                {
                    movsList.Add(new int[2] { 0, 1 }); //bottom
                    movsList.Add(new int[2] { 0, 0 }); //up
                }
            }
            else
            {
                movsList.Add(new int[2] { 1, 1 }); //left
                movsList.Add(new int[2] { 1, 0 }); //right

                if (y == limits[1])
                {
                    movsList.Add(new int[2] { 0, 1 }); //bottom
                }
                else if (y == 0)
                {
                    movsList.Add(new int[2] { 0, 0 }); //up
                }
                else
                {
                    movsList.Add(new int[2] { 0, 1 }); //bottom
                    movsList.Add(new int[2] { 0, 0 }); //up
                }
            }

            return movsList.Count;
        }

    }

    public class Transition
    {
        public int[] origin;
        public int transited;
        public List<int[]> target = new List<int[]>();
        public List<int[]> targetBy = new List<int[]>();

        public Transition()
        { }

        public Transition(int oi, int oe, int ti, int te)
        {
            origin = new int[2] { oi, oe };
            target.Add(new int[2] { ti, te });
        }

        public void CreateTransition(int oi, int oe, int ti, int te)
        {
            origin = new int[2] { oi, oe };
            target.Add(new int[2] { ti, te });
        }

        public void Clear()
        {
            target.RemoveAt(target.Count - 1);
            //targetBy.RemoveAt(targetBy.Count - 1);
        }

        public bool Contains(int[] t)
        {
            foreach (var tar in target)
            {
                if (tar[0] == t[0] && tar[1] == t[1])
                    return true;
            }
            return false;
        }
    }
}