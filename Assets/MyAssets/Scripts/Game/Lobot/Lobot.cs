using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainSystem;
using Map;

namespace Lobot{
    public enum ActionType{
        FORWARD_MOVE    = 0,
        BACKWARD_MOVE   = 1,
        RIGHT_MOVE      = 2,
        LEFT_MOVE       = 3,
        GAIN            = 4
    }

    public enum LobotState{
        WAIT,
        MOVE
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

        public const float DELTA_MOVE_WORLD_POS_X = 2.84f;
        public const float DELTA_MOVE_WORLD_POS_Y = -2.21f;

        private LobotState state = LobotState.WAIT;

        ///<summary> 移動 </summary>
        public void Move(int dx, int dy){
            move_delta = new Vector2Int(dx, dy);
            state = LobotState.MOVE;
        }

        private const int FRAME_FOR_MOVE = 50;
        private int frame_count_when_move = 0;
        private Vector2Int move_delta = Vector2Int.zero;

        ///<summary> 表示座標を動かす </summary>
        private void _Move(){
            var temp = transform.position;
            temp.x += DELTA_MOVE_WORLD_POS_X * move_delta.x / FRAME_FOR_MOVE;
            temp.y += DELTA_MOVE_WORLD_POS_Y * move_delta.y / FRAME_FOR_MOVE;
            transform.position = temp;
        }

        ///<summary> 移動処理終了 </summary>
        public void FinishMove(){
            // マップ座標を更新
            positionOnMap += new Vector2Int(move_delta.x, move_delta.y);

            move_delta = Vector2Int.zero;
            frame_count_when_move = 0;
            state = LobotState.WAIT;
        }


        ///<summary> 基盤の実行 </summary>
        public void Execute(){
            if (!is_start) return;

            var color = MapManager.Instance.GetColorType(positionOnMap.x, positionOnMap.y);
            var sound = MapManager.Instance.GetSoundType(positionOnMap.x, positionOnMap.y);
            int result = circuit.Execute(sound, color);

            if (result == -1) return;

            action_stack.Add(result);
        }

        ///<summary> 基盤に従ったアクションを実行 </summary>
        public void Action(int result){
            switch(result){
                case (int)ActionType.FORWARD_MOVE:
                    Move(0, -1);
                    break;
                case (int)ActionType.BACKWARD_MOVE:
                    Move(0, 1);
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

        private int frame_count = 0;

        // Update is called once per frame
        public override void onUpdate()
        {
            if (!is_start) return;

            switch(state){
                case LobotState.WAIT:
                    Execute();
                    break;
                case LobotState.MOVE:
                    _Move();
                    frame_count_when_move++;
                    if (frame_count_when_move == FRAME_FOR_MOVE){
                        FinishMove();
                    }
                    break;
            }
        }
    }
}
