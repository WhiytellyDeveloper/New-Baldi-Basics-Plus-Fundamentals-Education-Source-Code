using System;
using System.Collections.Generic;
using System.Text;

namespace nbppfe.BasicClasses.Functions
{
    public class CopyCastTexturesFunction : RoomFunction
    {
        public override void Build(LevelBuilder builder, Random rng)
        {
            base.Build(builder, rng);

            foreach (RoomController _room in room.ec.rooms)
            {
                if (_room.category == category)
                {
                    foreach (Cell cell in room.cells)
                        cell.Tile.MeshRenderer.material.SetTexture("_MainTex", _room.textureAtlas);
                }
            }
        }

        public RoomCategory category;
    }
}
