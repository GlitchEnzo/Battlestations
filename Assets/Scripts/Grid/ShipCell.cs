namespace Battlestations
{
    using Gamelogic.Grids;
    using UnityEngine;
    using System.Collections;

    public class ShipCell : SpriteCell
    {
        public bool IsAccessible;

        public bool HasTopWall;
        public bool HasBottomWall;
        public bool HasLeftWall;
        public bool HasRightWall;

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Vector3 halfSize = new Vector3(0.5f, 0.5f, 0);
            Vector3 halfsies = new Vector3(-0.5f, 0.5f, 0);

            Vector3 bottomLeft = transform.position - halfSize;
            Vector3 topRight = transform.position + halfSize;
            Vector3 topLeft = transform.position + halfsies;
            Vector3 bottomRight = transform.position - halfsies;

            Gizmos.color = Color.green;
            if (HasTopWall)
                Gizmos.DrawLine(topLeft, topRight);
            if (HasBottomWall)
                Gizmos.DrawLine(bottomLeft, bottomRight);
            if (HasLeftWall)
                Gizmos.DrawLine(bottomLeft, topLeft);
            if (HasRightWall)
                Gizmos.DrawLine(bottomRight, topRight);

            if (!IsAccessible)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(bottomLeft, topRight);
                Gizmos.DrawLine(topLeft, bottomRight);
            }
        }

        void Start()
        {
            Vector3 halfSize = new Vector3(0.5f, 0.5f, 0);
            Vector3 halfsies = new Vector3(-0.5f, 0.5f, 0);

            Vector3 bottomLeft = -halfSize;
            Vector3 topRight = halfSize;
            Vector3 topLeft = halfsies;
            Vector3 bottomRight = -halfsies;
            
            if (HasTopWall)
                AddEdge(topLeft, topRight);
            if (HasBottomWall)
                AddEdge(bottomLeft, bottomRight);
            if (HasLeftWall)
                AddEdge(bottomLeft, topLeft);
            if (HasRightWall)
                AddEdge(bottomRight, topRight);

            if (!IsAccessible)
            {
                var box = gameObject.AddComponent<BoxCollider2D>();
            }
        }

        private void AddEdge(Vector3 start, Vector3 end)
        {
            EdgeCollider2D edge = gameObject.AddComponent<EdgeCollider2D>();
            //edge.points[0] = new Vector2(start.x, start.y);
            //edge.points[1] = new Vector2(end.x, end.y);
            var points = new Vector2[] { new Vector2(start.x, start.y), new Vector2(end.x, end.y) };
            edge.points = points;
        }
    }
}