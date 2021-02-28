using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainSystem;
using Map;
using UnityEngine.SceneManagement;

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

        [SerializeField] private GameObject legend_chip;

        public Lobot lobot{
            get;
            private set;
        }

        ///<summary> ロボットの移動 </summary>
        public void MoveLobot(int dx, int dy){
            int new_x = lobot.positionOnMap.x + dx;
            int new_y = lobot.positionOnMap.y + dy;

            lobot.Look(dx, dy);

            if (! map.IsMoveable(new_x, new_y)) return;

            lobot.Move(dx, dy);
        }

        // ロボットのスタート位置
        public readonly static Vector3 START_LOBOT_POS = new Vector3(14.79f, -6.66f, 0f);

        public static Vector3 MapPos2WorldPos(Vector2Int map_pos){
            var ret = new Vector3 (0, 0, 0);
            ret.x = Lobot.START_00_POS.x + Lobot.DELTA_MOVE_WORLD_POS_X * map_pos.x;
            ret.y = Lobot.START_00_POS.y + Lobot.DELTA_MOVE_WORLD_POS_Y * map_pos.y;
            return ret;
        }

        public void SetStartPos(){
            startPositionOnMap = Common.SharedData.Instance.start_pos;

            startPositinOnWorld = MapPos2WorldPos(startPositionOnMap);
        }

        ///<summary> ロボットオブジェクトのインスタンス化 </summary>
        public void InstanciateLobot(){
            var prefab = Resources.Load("Prefab/Lobot/Lobot");

            // インスタンス化
            lobot_obj = Instantiate(prefab, startPositinOnWorld, Quaternion.identity) as GameObject;
            lobot = lobot_obj.GetComponent<Lobot>();

            // 親オブジェクトの設定
            // lobot_obj.transform.parent = this.transform;

            lobot.positionOnMap = startPositionOnMap;
            lobot.legendChip = legend_chip;

            lobot.SetCircuit(Common.SharedData.Instance.shared_circuit);
            lobot.GameStart();
        }

        public void Clear(){
            Debug.Log("Goal");
            Common.SharedData.Instance.is_clear = true;

            SceneManager.LoadScene("Result");
        }

        public void Fail(){
            Debug.Log("Fail");
            Common.SharedData.Instance.is_clear = false;

            SceneManager.LoadScene("Result");
        }

        public override void onUpdate()
        {
            base.onUpdate();

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
                        lobot.Gain(
                            map.GetMapType(lobot.positionOnMap.x, lobot.positionOnMap.y),
                            () => {
                                Clear();
                            },
                            () => {
                                Fail();
                            }
                        );
                        break;
                }
            }

            lobot.action_stack.Clear();
        }
    }
}
