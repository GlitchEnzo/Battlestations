namespace Battlestations
{
    using Gamelogic.Grids;
    using UnityEngine;

    public class ShipGrid : MonoBehaviour
    {
        public Vector2 gridSize = new Vector2(10, 10);
        public Vector2 cellSize = new Vector2(1, 1);

        // The grid data structure that contains all cell.
        private RectGrid<ShipCell> grid;

        // The map (that converts between world and grid coordinates).
        private IMap3D<RectPoint> map;

        private void Start()
        {
            BuildGrid();
        }
            
        private void BuildGrid()
        {
            ShipCell defaultCell = CreateDefaultShipCell();

            // Creates a grid in a rectangular shape.
            grid = RectGrid<ShipCell>.Rectangle((int)gridSize.x, (int)gridSize.y);

            // Creates a map...
            map = new RectMap(cellSize).To3DXY();

            foreach (RectPoint point in grid) //Iterates over all points (coordinates) contained in the grid
            {
                ShipCell cell = Instantiate(defaultCell); // Instantiate a cell from the given prefab.

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
    }
}
