using UnityEngine;

namespace nbppfe.BasicClasses.Functions
{
    public class LobbySkyboxRoomFunction : RoomFunction
    {
        public override void Initialize(RoomController room)
        {
            //Placeholder 
            base.Initialize(room);
            float num = room.ec.RealRoomMax(room).x - room.ec.RealRoomMin(room).x;
            float num2 = room.ec.RealRoomMax(room).z - room.ec.RealRoomMin(room).z;
            Vector3 a = new Vector3(room.ec.RealRoomMax(room).x - num / 2f, 5f, room.ec.RealRoomMax(room).z - num2 / 2f);
            skybox.transform.position = a + Vector3.up * 20f;
            Vector3 vector = default;
            vector = room.ec.RealRoomSize(room);
            vector.x /= 10f;
            vector.z /= 10f;
            vector.y = 1f;
            skybox.transform.localScale = vector;
            skybox.InitializeMaterials(room.wallTex, room.size.x, room.size.z);
            room.cells[0].renderers.AddRange(skybox.GetComponent<RendererContainer>().renderers);
        }

        public Skybox skybox;
    }
}
