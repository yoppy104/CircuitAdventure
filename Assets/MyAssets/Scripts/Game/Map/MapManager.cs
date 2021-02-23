using UnityEngine;
using MainSystem;

namespace Map{
    public class MapManager : MyBehaviour
    {
        // プレイヤーの現在位置
        private Vector2Int player_locate = Vector2Int.zero;

        private int limit_x = 0;
        private int limit_y = 0;

        private MapTile[,] tiles;

        ///<summary> プレイヤー位置の設定 </summary>
        public void SetPlayerLocate(Vector2Int pos){
            player_locate = pos;
        }
        ///<summary> プレイヤー位置の設定 </summary>
        public void SetPlayerLocate(int x, int y){
            player_locate = new Vector2Int(x, y);
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

            // マップサイズの上限値
            limit_x = 5;
            limit_y = 5;

            // タイル配列
            tiles = new MapTile[limit_x, limit_y];

            var tiles_obj = new GameObject("Tiles");
            tiles_obj.transform.parent = this.transform;
            
            // タイル配列の初期化
            for (int x = 0; x < limit_x; x++){
                for (int y = 0; y < limit_y; y++){
                    var temp = new MapTile(x, y, MapType.NORMAL);
                    temp.prefab = Resources.Load("Prefab/Map/Plane");
                    temp.Generate(tiles_obj.transform);
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