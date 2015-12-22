using Gamelogic;
using UnityEditor;

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

        public bool IsBattlestation;

        public TeleportPoint TeleportPoint = TeleportPoint.None;

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Vector3 halfSize = new Vector3(0.5f, 0.5f, 0);
            Vector3 halfsies = new Vector3(-0.5f, 0.5f, 0);

            Vector3 bottomLeft = transform.position + transform.rotation * -halfSize;
            Vector3 topRight = transform.position + transform.rotation * halfSize;
            Vector3 topLeft = transform.position + transform.rotation * halfsies;
            Vector3 bottomRight = transform.position + transform.rotation * -halfsies;

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

            if (IsBattlestation)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, 0.25f);
            }

            if (TeleportPoint != TeleportPoint.None)
            {
                GUIStyle myStyle = new GUIStyle();
                myStyle.fontSize = 10;
                myStyle.fontStyle = FontStyle.Bold;
                Handles.Label(transform.position, TeleportPoint.ToString(), myStyle);
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
                gameObject.AddComponent<BoxCollider2D>();
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

        /// <summary>
        /// Gets a value indicating whether or not this cell has a "top" wall, taking into account the rotation of the cell.
        /// </summary>
        /// <returns>True if it has a "top" wall, otherwise false.</returns>
        public bool GetHasTopWall()
        {
            float rotation = transform.eulerAngles.z;
            while (rotation > 360)
                rotation -= 360;
            while (rotation < 0)
                rotation += 360;

            // counter clockwise is positive
            // ----
            // |\/|
            // |/\|
            // ----
            if (rotation > -45 && rotation <= 45) // 0
                return HasTopWall;
            if (rotation > 45 && rotation <= 135) // 90
                return HasRightWall;
            if (rotation > 135 && rotation <= 225) // 180
                return HasBottomWall;
            if (rotation > 225 && rotation <= 315) // 270
                return HasLeftWall;
            if (rotation > 315 && rotation <= 405) // 0
                return HasTopWall;

            return HasTopWall;
        }

        /// <summary>
        /// Gets a value indicating whether or not this cell has a "bottom" wall, taking into account the rotation of the cell.
        /// </summary>
        /// <returns>True if it has a "bottom" wall, otherwise false.</returns>
        public bool GetHasBottomWall()
        {
            float rotation = transform.eulerAngles.z;
            while (rotation > 360)
                rotation -= 360;
            while (rotation < 0)
                rotation += 360;

            // counter clockwise is positive
            // ----
            // |\/|
            // |/\|
            // ----
            if (rotation > -45 && rotation <= 45) // 0
                return HasBottomWall;
            if (rotation > 45 && rotation <= 135) // 90
                return HasLeftWall;
            if (rotation > 135 && rotation <= 225) // 180
                return HasTopWall;
            if (rotation > 225 && rotation <= 315) // 270
                return HasRightWall;
            if (rotation > 315 && rotation <= 405) // 0
                return HasBottomWall;

            return HasBottomWall;
        }

        /// <summary>
        /// Gets a value indicating whether or not this cell has a "left" wall, taking into account the rotation of the cell.
        /// </summary>
        /// <returns>True if it has a "left" wall, otherwise false.</returns>
        public bool GetHasLeftWall()
        {
            float rotation = transform.eulerAngles.z;
            while (rotation > 360)
                rotation -= 360;
            while (rotation < 0)
                rotation += 360;

            // counter clockwise is positive
            // ----
            // |\/|
            // |/\|
            // ----
            if (rotation > -45 && rotation <= 45) // 0
                return HasLeftWall;
            if (rotation > 45 && rotation <= 135) // 90
                return HasTopWall;
            if (rotation > 135 && rotation <= 225) // 180
                return HasRightWall;
            if (rotation > 225 && rotation <= 315) // 270
                return HasBottomWall;
            if (rotation > 315 && rotation <= 405) // 0
                return HasLeftWall;

            return HasLeftWall;
        }

        /// <summary>
        /// Gets a value indicating whether or not this cell has a "right" wall, taking into account the rotation of the cell.
        /// </summary>
        /// <returns>True if it has a "right" wall, otherwise false.</returns>
        public bool GetHasRightWall()
        {
            float rotation = transform.eulerAngles.z;
            while (rotation > 360)
                rotation -= 360;
            while (rotation < 0)
                rotation += 360;

            // counter clockwise is positive
            // ----
            // |\/|
            // |/\|
            // ----
            if (rotation > -45 && rotation <= 45) // 0
                return HasRightWall;
            if (rotation > 45 && rotation <= 135) // 90
                return HasBottomWall;
            if (rotation > 135 && rotation <= 225) // 180
                return HasLeftWall;
            if (rotation > 225 && rotation <= 315) // 270
                return HasTopWall;
            if (rotation > 315 && rotation <= 405) // 0
                return HasRightWall;

            return HasRightWall;
        }
    }
}