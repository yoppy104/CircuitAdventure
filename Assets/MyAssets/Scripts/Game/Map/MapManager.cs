using UnityEngine;
using MainSystem;

namespace Map{
    public enum ColorType{
        BRANK   = 1 << 0,
        RED     = 1 << 1,
        BLUE    = 1 << 2,
        YELLOW  = 1 << 3,
        GREEN   = 1 << 4
    }
    public enum SoundType{
        SILENCE = 1 << 10,
        BIRD    = 1 << 11
    }

    public class MapManager : MyBehaviour
    {
        // プレイヤーの現在位置
        private Vector2Int player_locate = Vector2Int.zero;

        public static MapManager Instance {
            get;
            private set;
        }

        [SerializeField] private int limit_x = 0;
        [SerializeField] private int limit_y = 0;
        [SerializeField] private Vector2Int[] exit_pos;

        [SerializeField] private Vector2Int[] color_pos;
        [SerializeField] private ColorType[] color_type;

        [SerializeField] private Vector2Int[] sound_pos;
        [SerializeField] private SoundType[] sound_type;

        [SerializeField] private Vector2Int goal_pos;

        private MapTile[,] tiles;

        ///<summary> プレイヤー位置の設定 </summary>
        public void SetPlayerLocate(Vector2Int pos){
            player_locate = pos;
        }
        ///<summary> プレイヤー位置の設定 </summary>
        public void SetPlayerLocate(int x, int y){
            player_locate = new Vector2Int(x, y);
        }

        ///<summary> 指定座標のマップタイプを返す </summary>
        public MapType GetMapType(int x, int y){
            return tiles[x, y].Type;
        }

        public ColorType GetColorType(int x, int y){
            return tiles[x, y].hasColor;
        }

        public SoundType GetSoundType(int x, int y){
            return tiles[x, y].hasSound;
        }

        ///<summary> 指定座標が移動可能か判定する </summary>
        public bool IsMoveable(int x, int y){
            // タイル配列の範囲内かを判定
            if (x < 0 || y < 0) return false;
            if (x >= limit_x || y >= limit_y) return false;

            // タイルが移動可能かどうかを判定
            return tiles[x, y].isMoveable;
        } 

        public override void onStart()
        {
            base.onStart();

            Instance = this;

            // タイル配列
            tiles = new MapTile[limit_x, limit_y];

            var tiles_obj = new GameObject("Tiles");
            tiles_obj.transform.parent = this.transform;
            
            // タイル配列の初期化
            // ゴール
            var goal = new MapTile(goal_pos.x, goal_pos.y, MapType.GOAL);
            tiles[goal_pos.x, goal_pos.y] = goal;


            // 進入不可タイル
            foreach (Vector2Int pos in exit_pos){
                var temp = new MapTile(pos.x, pos.y, MapType.DROP);
                tiles[pos.x, pos.y] = temp;
            }

            // 色タイル
            if (color_pos.Length == color_type.Length){
                for (int i = 0; i < color_pos.Length; i++){
                    var pos = color_pos[i];
                    var type = color_type[i];

                    var temp = new MapTile(pos.x, pos.y, MapType.COLOR);
                    temp.hasColor = type;

                    tiles[pos.x, pos.y] = temp;
                }
            }

            // 音タイル
            if (sound_pos.Length == sound_type.Length){
                for (int i = 0; i < sound_pos.Length; i++){
                    var pos = sound_pos[i];
                    var type = sound_type[i];

                    var temp = new MapTile(pos.x, pos.y, MapType.COLOR);
                    temp.hasSound = type;

                    tiles[pos.x, pos.y] = temp;
                }
            }

            // 残りを普通タイルでうめる
            for (int x = 0; x < limit_x; x++){
                for (int y = 0; y < limit_y; y++){
                    if (tiles[x, y] != null) continue;

                    var temp = new MapTile(x, y, MapType.NORMAL);
                    // temp.prefab = Resources.Load("Prefab/Map/Plane");
                    // temp.Generate(tiles_obj.transform);
                    tiles[x, y] = temp;
                }
            }
        }

        public override void onUpdate()
        {
            base.onUpdate();
        }
    }
}