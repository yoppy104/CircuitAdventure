using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainSystem;

namespace Lobot{
    public enum ActionType{
        FORWARD_MOVE    = 0,
        BACKWARD_MOVE   = 1,
        RIGHT_MOVE      = 2,
        LEFT_MOVE       = 3,
        GAIN            = 4
    }

    public class Lobot : MyBehaviour 
    {
        // 基盤
        private Circuit circuit;

        private bool is_start = false;

        public void GameStart(){
            is_start = true;
        }

        public void GameStop(){
            is_start = false;
        }

        ///<summary> 基盤の設定 </summary>
        public void SetCircuit(Circuit set){
            if (set == null) return;
            circuit = set;
        }

        public List<int> action_stack{
            get;
            private set;
        } = new List<int>();

        // マップ上での座標
        public Vector2Int positionOnMap {
            get;
            set;
        } = Vector2Int.zero;


        ///<summary> 移動 </summary>
        public void Move(int dx, int dy){
            positionOnMap += new Vector2Int(dx, dy);

            transform.position = MapCommon.MapScale2WorldScale(positionOnMap);
        }


        ///<summary> 基盤の実行 </summary>
        public void Execute(){
            if (!is_start) return;

            int result = circuit.Execute();

            if (result == -1) return;

            action_stack.Add(result);
        }

        ///<summary> 基盤に従ったアクションを実行 </summary>
        public void Action(int result){
            switch(result){
                case (int)ActionType.FORWARD_MOVE:
                    Move(0, 1);
                    break;
                case (int)ActionType.BACKWARD_MOVE:
                    Move(0, -1);
                    break;
                case (int)ActionType.RIGHT_MOVE:
                    Move(1, 0);
                    break;
                case (int)ActionType.LEFT_MOVE:
                    Move(-1, 0);
                    break;
            }
        }

        // Start is called before the first frame update
        public override void onStart()
        {
            
        }

        // Update is called once per frame
        public override void onUpdate()
        {
            Execute();
        }
    }
}
