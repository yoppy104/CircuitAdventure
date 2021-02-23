using UnityEngine;

namespace Map{
    // マップタイルの種類
    public enum MapType{
        NORMAL,
        DROP,
        COLOR,
        SOUND
    }

    public class MapTile{
        
        public const float TILE_SIZE_X = 0.5f;
        public const float TILE_SIZE_Y = 0.5f;

        // タイルを配置する座標
        public Vector2Int Position{
            get;
            set;
        } = Vector2Int.zero;

        // 移動可能かどうか(初期値はtrue)
        public bool isMoveable{
            get;
            private set;
        } = true;

        public MapType Type{
            get;
            private set;
        } = MapType.NORMAL;

        public UnityEngine.Object prefab{
            get;
            set;
        } = null;

        public GameObject obj{
            get;
            set;
        } = null;

        ///<summary> プレハブのインスタンス化 </summary>
        public void Generate(Transform parent){
            if (prefab == null) return;

            // インスタンス化
            obj = MonoBehaviour.Instantiate(prefab) as GameObject;

            // ローカル座標の初期化
            obj.transform.position = new Vector3(Position.x * TILE_SIZE_X * 10, 0f, Position.y * TILE_SIZE_Y * 10);
            obj.transform.localScale = new Vector3(TILE_SIZE_X, 1f, TILE_SIZE_Y);

            obj.transform.parent = parent;
        }

        ///<summary> マップの1マスを表す </summary>
        public MapTile(int x, int y, MapType type){
            Position = new Vector2Int(x, y);
            Type = type;
            if (type == MapType.DROP){
                isMoveable = false;
            }
        }
    }
}