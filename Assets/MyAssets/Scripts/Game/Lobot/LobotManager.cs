using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainSystem;
using Map;

namespace Lobot{
    public class LobotManager : MyBehaviour
    {
        public MapManager map{
            get;
            set;
        }

        private GameObject lobot_obj = null;

        public Lobot lobot{
            get;
            private set;
        }


        ///<summary> ロボットの移動 </summary>
        public void MoveLobot(int dx, int dy){
            int new_x = lobot.positionOnMap.x + dx;
            int new_y = lobot.positionOnMap.y + dy;

            if (! map.IsMoveable(new_x, new_y)) return;

            lobot.Move(dx, dy);
        }


        ///<summary> ロボットオブジェクトのインスタンス化 </summary>
        public void InstanciateLobot(){
            var prefab = Resources.Load("Prefab/Lobot/Lobot");

            // インスタンス化
            lobot_obj = Instantiate(prefab) as GameObject;
            lobot = lobot_obj.GetComponent<Lobot>();

            // 親オブジェクトの設定
            lobot_obj.transform.parent = this.transform;

            lobot_obj.transform.position += new Vector3(0f, 0.6f, 0f);
        }
    }
}
