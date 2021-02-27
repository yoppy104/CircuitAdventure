using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map{
    public class MapCommon
    {
        // マップオブジェクトのスケール
        public const float TILE_SIZE_X = 0.5f;
        public const float TILE_SIZE_Y = 1f;
        public const float TILE_SIZE_Z = 0.5f;

        public static readonly Vector3 TILE_SCALE = new Vector3(TILE_SIZE_X, TILE_SIZE_Y, TILE_SIZE_Z);

        ///<summary> マップ座標系をワールド座標系に変換する。 </summary>
        public static Vector3 MapScale2WorldScale(int x, int y){
            return new Vector3(x * TILE_SIZE_X * 10, 0f, y * TILE_SIZE_Z * 10);
        }

        ///<summary> マップ座標系をワールド座標系に変換する。 </summary>
        public static Vector3 MapScale2WorldScale(Vector2Int pos){
            return MapScale2WorldScale(pos.x, pos.y);
        }
    }
}
