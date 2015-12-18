namespace Battlestations
{
    using System.Collections.Generic;
    using System.Linq;
    using Gamelogic.Grids;
    using UnityEngine;

    public class ShipGrid : GridBehaviour<RectPoint>
    {
        public Vector2 gridSize = new Vector2(10, 10);
        public Vector2 cellSize = new Vector2(1, 1);

        public ShipCell prefab;

        // The grid data structure that contains all cell.
        private RectGrid<ShipCell> grid;

        // The map (that converts between world and grid coordinates).
        private IMap3D<RectPoint> map;

        private void Start()
        {
            //BuildGrid();
            //grid = GetComponent<RectTileGridBuilder>().Grid;

            RectTileGridBuilder builder = GetComponent<RectTileGridBuilder>();
            start = builder.Grid.First();
            goal = builder.Grid.Last();
        }

        private void BuildGrid()
        {
            //ShipCell defaultCell = CreateDefaultShipCell();

            // Creates a grid in a rectangular shape.
            grid = RectGrid<ShipCell>.Rectangle((int)gridSize.x, (int)gridSize.y);

            // Creates a map...
            map = new RectMap(cellSize).To3DXY();

            foreach (RectPoint point in grid) //Iterates over all points (coordinates) contained in the grid
            {
                ShipCell cell = Instantiate(prefab);

                Vector3 worldPoint = map[point]; //Calculate the world point of the current grid point

                cell.transform.parent = transform; //Parent the cell to the root
                cell.transform.localScale = Vector3.one; //Readjust the scale - the re-parenting above may have changed it.
                cell.transform.localPosition = worldPoint; //Set the localPosition of the cell.

                cell.name = point.ToString(); // Makes it easier to identify cells in the editor.
                grid[point] = cell; // Finally, put the cell in the grid.
            }
        }

        private ShipCell CreateDefaultShipCell()
        {
            GameObject cellObject = new GameObject("ShipCell");
            cellObject.AddComponent<SpriteRenderer>();
            return cellObject.AddComponent<ShipCell>();
        }

        private IEnumerable<RectPoint> GetGridPath()
        {
            RectTileGridBuilder builder = GetComponent<RectTileGridBuilder>();

            var path = Algorithms.AStar(builder.Grid, start, goal,
                (p, q) => p.DistanceFrom(q),
                c => ((ShipCell)c).IsAccessible,
                CalculateCost);

            return path;
        }
        
        private float CalculateCost(RectPoint p, RectPoint q)
        {
            RectTileGridBuilder builder = GetComponent<RectTileGridBuilder>();

            var pCell = (ShipCell)builder.Grid[p];
            var qCell = (ShipCell)builder.Grid[q];

            float cost = 1;

            if (p + RectPoint.North == q)
            {
                // q
                // p
                if (pCell.HasTopWall || qCell.HasBottomWall)
                {
                    cost = float.MaxValue;
                }
            }
            else if (p + RectPoint.South == q)
            {
                // p
                // q
                if (pCell.HasBottomWall || qCell.HasTopWall)
                {
                    cost = float.MaxValue;
                }
            }
            else if (p + RectPoint.East == q)
            {
                // p q
                if (pCell.HasRightWall || qCell.HasLeftWall)
                {
                    cost = float.MaxValue;
                }
            }
            else if (p + RectPoint.West == q)
            {
                // q p
                if (pCell.HasLeftWall || qCell.HasRightWall)
                {
                    cost = float.MaxValue;
                }
            }
            else
            {
                Debug.LogWarningFormat("Not neighbors!: {0} and {1}", p, q);
            }

            return cost;
        }

        private RectPoint start;
        private RectPoint goal;
        private bool selectStart = true; //otherwise, select goal

        private void SetStartOrGoal(RectPoint clickedPoint)
        {
            if (selectStart && clickedPoint != goal)
            {
                start = clickedPoint;
                selectStart = false;
            }
            else if (clickedPoint != start)
            {
                goal = clickedPoint;
                selectStart = true;
            }
        }

        public void OnRightClick(RectPoint clickedPoint)
        {
            Debug.LogFormat("Clicked {0}", clickedPoint);

            SetStartOrGoal(clickedPoint);
            ClearPath();
            UpdatePath();
        }

        private void UpdatePath()
        {
            RectTileGridBuilder builder = GetComponent<RectTileGridBuilder>();

            IEnumerable<RectPoint> path = GetGridPath();

            if (path == null)
            {
                return; //then there is no path between the start and goal.
            }

            Color color = new Color(1.0f, 1.0f, 0.0f, 0.5f);

            foreach (var point in path)
            {
                //grid[point].Color = Color.black;
                builder.Grid[point].transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = color;
            }
        }

        void OnMouseDown()
        {
            Debug.Log("Clicked...");
        }

        void ClearPath()
        {
            RectTileGridBuilder builder = GetComponent<RectTileGridBuilder>();
            Color color = new Color(0, 0, 0, 0);

            foreach (var point in builder.Grid)
            {
                builder.Grid[point].transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}
