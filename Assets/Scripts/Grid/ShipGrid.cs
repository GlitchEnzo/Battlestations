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
                if (pCell.GetHasTopWall() || qCell.GetHasBottomWall())
                {
                    cost = float.MaxValue;
                }
            }
            else if (p + RectPoint.South == q)
            {
                // p
                // q
                if (pCell.GetHasBottomWall() || qCell.GetHasTopWall())
                {
                    cost = float.MaxValue;
                }
            }
            else if (p + RectPoint.East == q)
            {
                // p q
                if (pCell.GetHasRightWall() || qCell.GetHasLeftWall())
                {
                    cost = float.MaxValue;
                }
            }
            else if (p + RectPoint.West == q)
            {
                // q p
                if (pCell.GetHasLeftWall() || qCell.GetHasRightWall())
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
            //Debug.LogFormat("Clicked {0}", clickedPoint);

            SetStartOrGoal(clickedPoint);
            ClearPath();
            UpdatePath();
            //DrawLine();
            //DrawLine2();

            //Debug.LogFormat("In Line of Sight = {0}", IsInLineOfSight(start, goal));
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

            if (!IsInLineOfSight(start, goal))
                color = new Color(1.0f, 0.0f, 0.0f, 0.5f);

            foreach (var point in path)
            {
                //grid[point].Color = Color.black;
                builder.Grid[point].transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = color;
            }
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

        /// <summary>
        /// Draws lines via DDA.  Does NOT support vertical lines..
        /// </summary>
        /// <returns></returns>
        IEnumerable<RectPoint> DrawLine()
        {
            RectTileGridBuilder builder = GetComponent<RectTileGridBuilder>();
            Color color = new Color(1.0f, 1.0f, 0.0f, 0.5f);

            var line = new List<RectPoint>();

            float dydx = (goal.Y - start.Y) / (goal.X - start.X);
            float y = start.Y;
            for (int x = start.X; x <= goal.X; x++)
            {
                var point = new RectPoint(x, Mathf.RoundToInt(y));
                line.Add(point);
                builder.Grid[point].transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = color;

                y = y + dydx;
            }

            return line;
        }

        IEnumerable<RectPoint> DrawLine2()
        {
            RectTileGridBuilder builder = GetComponent<RectTileGridBuilder>();
            Color color = new Color(1.0f, 1.0f, 0.0f, 0.5f);

            var line = new List<RectPoint>();

            int dy = goal.Y - start.Y;
            int dx = goal.X - start.X;
            int stepx, stepy;

            if (dy < 0) { dy = -dy; stepy = -1; } else { stepy = 1; }
            if (dx < 0) { dx = -dx; stepx = -1; } else { stepx = 1; }
            dy <<= 1;        // dy is now 2*dy
            dx <<= 1;        // dx is now 2*dx

            float x = start.X;
            float y = start.Y;

            //drawpixel(x1, y1, color);
            var point = new RectPoint(start.X, start.Y);
            line.Add(point);
            builder.Grid[point].transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = color;

            if (dx > dy)
            {
                int fraction = dy - (dx >> 1);  // same as 2*dy - dx
                while (x != goal.X)
                {
                    if (fraction >= 0)
                    {
                        y += stepy;
                        fraction -= dx;          // same as fraction -= 2*dx
                    }
                    x += stepx;
                    fraction += dy;              // same as fraction -= 2*dy

                    //drawpixel(x1, y1, color);
                    point = new RectPoint(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
                    line.Add(point);
                    builder.Grid[point].transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = color;
                }
            }
            else
            {
                int fraction = dx - (dy >> 1);
                while (y != goal.Y)
                {
                    if (fraction >= 0)
                    {
                        x += stepx;
                        fraction -= dy;
                    }
                    y += stepy;
                    fraction += dx;

                    //drawpixel(x1, y1, color);
                    point = new RectPoint(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
                    line.Add(point);
                    builder.Grid[point].transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = color;
                }
            }

            return line;
        }

        public bool IsInLineOfSight(RectPoint viewer, RectPoint target)
        {
            RectTileGridBuilder builder = GetComponent<RectTileGridBuilder>();
            return IsInLineOfSight(builder.Grid[viewer].transform.position, builder.Grid[target].transform.position);
        }

        public static bool IsInLineOfSight(Vector3 viewer, Vector3 target)
        {
            bool inLineOfSight = false;

            var direction = target - viewer;
            var distance = direction.magnitude;
            direction.Normalize();

            var hit = Physics2D.Raycast(new Vector2(viewer.x, viewer.y), new Vector2(direction.x, direction.y), distance);

            if (hit.collider != null)
            {
                // something was in the way, therefore NOT in line of sight
                Debug.Log(hit.collider.name);
            }
            else
            {
                // the ray didn't hit anything, so it IS in the line of sight
                inLineOfSight = true;
            }

            return inLineOfSight;
        }

        void OnDrawGizmos()
        {
            //if (Application.isPlaying)
            Gizmos.color = Color.blue;
            RectTileGridBuilder builder = GetComponent<RectTileGridBuilder>();
            if (builder != null)
            {
                var origin = builder.Grid[start].transform.position;
                var target = builder.Grid[goal].transform.position;
                Gizmos.DrawRay(origin, target - origin);
            } 
        }
    }
}
