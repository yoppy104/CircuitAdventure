using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lobot;

namespace Common{

    ///<summary> シーン間で共有するデータ </summary>
    public class SharedData
    {
        private static SharedData instance = null;


        private SharedData(){

        }

        public static SharedData Instance{
            get {
                if (instance == null){
                    instance = new SharedData();
                }

                return instance;
            }
        }


        // EditシーンからGameシーンへ共有
        public Circuit shared_circuit = null;

        // ステージをクリアしたかどうか
        public bool is_clear = true;

        public Vector2Int start_pos = new Vector2Int(1,0);
        public Vector2Int goal_pos = Vector2Int.zero;
    }
}
