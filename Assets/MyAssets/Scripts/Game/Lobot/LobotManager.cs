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

        [SerializeField] private Vector2Int startPositionOnMap;
        [SerializeField] private Vector3 startPositinOnWorld;

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

        // ロボットのスタート位置
        public readonly static Vector3 START_LOBOT_POS = new Vector3(14.79f, -6.66f, 0f);

        ///<summary> ロボットオブジェクトのインスタンス化 </summary>
        public void InstanciateLobot(){
            var prefab = Resources.Load("Prefab/Lobot/Lobot");

            // インスタンス化
            lobot_obj = Instantiate(prefab, startPositinOnWorld, Quaternion.identity) as GameObject;
            lobot = lobot_obj.GetComponent<Lobot>();

            // 親オブジェクトの設定
            // lobot_obj.transform.parent = this.transform;

            lobot.positionOnMap = startPositionOnMap;

            lobot.SetCircuit(Common.SharedData.Instance.shared_circuit);
            lobot.GameStart();
        }

        public void Clear(){
            Debug.Log("Goal");
        }

        public void Fail(){
            Debug.Log("Fail");
        }

        public override void onUpdate()
        {
            base.onUpdate();

            // ゴールチェック
            if (map.GetMapType(lobot.positionOnMap.x, lobot.positionOnMap.y) == MapType.GOAL){
                Debug.Log("Goal");
            }

            foreach (int action in lobot.action_stack){

                switch (action){
                    case (int)ActionType.FORWARD_MOVE:
                        MoveLobot(0, -1);
                        break;
                    case (int)ActionType.BACKWARD_MOVE:
                        MoveLobot(0, 1);
                        break;
                    case (int)ActionType.RIGHT_MOVE:
                        MoveLobot(1, 0);
                        break;
                    case (int)ActionType.LEFT_MOVE:
                        MoveLobot(-1, 0);
                        break;
                    case (int)ActionType.GAIN:
                        if (map.GetMapType(lobot.positionOnMap.x, lobot.positionOnMap.y) == MapType.GOAL){
                            Clear();
                        }else{
                            Fail();
                        }
                        break;
                }
            }

            lobot.action_stack.Clear();
        }
    }
}
