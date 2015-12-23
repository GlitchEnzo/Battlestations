using Gamelogic.Grids;

namespace Battlestations
{
    using UnityEngine;

    public class Module : MonoBehaviour
    {
        private RectGrid<ShipCell> grid = null;

        public RectGrid<ShipCell> Grid
        {
            get
            {
                if (grid == null)
                {
                    RectTileGridBuilder builder = GetComponent<RectTileGridBuilder>();
                    grid = (RectGrid<ShipCell>)builder.Grid.CastValues<ShipCell, RectPoint>();
                }
                
                return grid;
            }   
        }

        public Texture2D Texture
        {
            get
            {
                Texture2D texture = null;

                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    texture = spriteRenderer.sprite.texture;
                }

                return texture;
            }
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }
}