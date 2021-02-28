using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainSystem;
using Map;
using System;

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
        MOVE,
        LOOK,
        CHECK_COLOR,
        CHECK_SOUND,
        GAIN
    }

    public class Lobot : MyBehaviour 
    {
        // Analyze Effect
        [SerializeField] private GameObject sound_effect;
        [SerializeField] private GameObject red_effect;
        [SerializeField] private GameObject blue_effect;
        [SerializeField] private GameObject green_effect;
        [SerializeField] private GameObject yellow_effect;

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
        }

        private MapType check_type = MapType.NORMAL;
        private Action success = null;
        private Action fail = null;
        private bool is_clear = false;
        public void Gain(MapType type, Action onSuccess, Action onFail){
            is_clear = (type == MapType.GOAL);

            success = onSuccess;
            fail = onFail;

            state = LobotState.GAIN;
        }

        public GameObject legendChip{
            get;
            set;
        }

        private int gain_animation_step = 0;
        private const float GAIN_ANIMATION_EACH_TIME = 0.5f;

        private bool is_set_hop_pos = true;
        private Vector3 gain_hop;

        private void _Gain(){
            time_count += Time.deltaTime;
            if (time_count > GAIN_ANIMATION_EACH_TIME){
                time_count = 0;
                gain_animation_step ++;

                if (gain_animation_step == 4){
                    FinishGain();
                }
            }

            switch(gain_animation_step){
                case 0:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, -45f));
                    break;
                case 1:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45f));
                    break;
                case 2:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                case 3:
                    if (is_set_hop_pos){
                        gain_hop = transform.position + new Vector3(0, 1f, 0);
                        is_set_hop_pos = false;
                    }
                    if (is_clear){
                        legendChip.transform.position = transform.position + new Vector3(0, 1f, 0);
                        transform.GetComponent<Renderer>().material.color = Color.yellow;
                        transform.position = gain_hop;
                    }else{
                        transform.GetComponent<Renderer>().material.color = Color.blue;
                    }
                    break;
            }
        }

        private void FinishGain(){
            time_count = 0;
            state = LobotState.WAIT;
            is_set_hop_pos = true;
            
            if (is_clear){
                Debug.Log("success");
                success();
            }else{
                fail();
                Debug.Log("fail");
            }
        }



        ///<summary> 車体の向きを変更する </summary>
        public void Look(int dx, int dy){
            // 斜めはありえない
            if (dx == 0){
                if (dy > 0){
                    drotate = new Vector3(0, 0, 180f) - transform.rotation.eulerAngles;
                }else{
                    drotate = new Vector3(0, 0, 0) - transform.rotation.eulerAngles;
                }
            }else{
                if (dx > 0){
                    drotate = new Vector3(0, 0, 270f) - transform.rotation.eulerAngles;
                }else{
                    drotate = new Vector3(0, 0, 90f) - transform.rotation.eulerAngles;
                }
            }
            
            // deltaが270だったら、90に変換する
            if (drotate.z == 270){
                drotate.z = -90;
            }else if (drotate.z == -270){
                drotate.z = 90;
            }

            // deltaが360だったら、180に変換する
            if (drotate.z == 360){
                drotate.z = -180;
            }else if (drotate.z == -360){
                drotate.z = 180;
            }

            state = LobotState.LOOK;
        }

        private Vector3 drotate = Vector3.zero;

        public void _Look(){
            transform.rotation *= Quaternion.Euler(drotate / FRAME_FOR_LOOK * Time.deltaTime);
        }

        private const float FRAME_FOR_MOVE = 0.7f;  // seconds
        private const float FRAME_FOR_LOOK = 0.5f;  // seconds
        private const float FRAME_FOR_COLOR = 1.7f; // seconds
        private const float FRAME_FOR_SOUND = 1.5f; // seconds

        private int frame_count_when_move = 0;
        private Vector2Int move_delta = Vector2Int.zero;

        ///<summary> 表示座標を動かす </summary>
        private void _Move(){
            var temp = transform.position;
            temp.x += DELTA_MOVE_WORLD_POS_X * move_delta.x / FRAME_FOR_MOVE * Time.deltaTime;
            temp.y += DELTA_MOVE_WORLD_POS_Y * move_delta.y / FRAME_FOR_MOVE * Time.deltaTime;
            transform.position = temp;
        }

        ///<summary> 移動処理終了 </summary>
        public void FinishMove(){
            // マップ座標を更新
            positionOnMap += new Vector2Int(move_delta.x, move_delta.y);
            move_delta = Vector2Int.zero;
            time_count = 0;
            state = LobotState.WAIT;
        }

        private void FinishLook(){
            time_count = 0;
            state = LobotState.MOVE;
            drotate = Vector3.zero;
        }

        private ColorType now_check = ColorType.BRANK;

        public void AnalyzeColor(ColorType type){
            state = LobotState.CHECK_COLOR;

            switch(type){
                case ColorType.RED:
                    red_effect.SetActive(true);
                    break;
                case ColorType.BLUE:
                    blue_effect.SetActive(true);
                    break;
                case ColorType.GREEN:
                    green_effect.SetActive(true);
                    break;
                case ColorType.YELLOW:
                    yellow_effect.SetActive(true);
                    break;
            }
            now_check = type;
            time_count = 0;
        }

        private void FinishAnalyzeColor(){
            state = LobotState.WAIT;
            switch(now_check){
                case ColorType.RED:
                    red_effect.SetActive(false);
                    break;
                case ColorType.BLUE:
                    blue_effect.SetActive(false);
                    break;
                case ColorType.GREEN:
                    green_effect.SetActive(false);
                    break;
                case ColorType.YELLOW:
                    yellow_effect.SetActive(false);
                    break;
            }
            now_check = ColorType.BRANK;
            time_count = 0;
        }


        public void AnalyzeSound(){
            state = LobotState.CHECK_SOUND;
            sound_effect.SetActive(true);
        }

        private void FinishAnalyzeSound(){
            state = LobotState.WAIT;
            sound_effect.SetActive(false);
        }


        ///<summary> 基盤の実行 </summary>
        public void Execute(){
            if (!is_start) return;

            var is_color = circuit.IsColor();
            var is_sound = circuit.IsSound();

            var color = MapManager.Instance.GetColorType(positionOnMap.x, positionOnMap.y);
            var sound = MapManager.Instance.GetSoundType(positionOnMap.x, positionOnMap.y);
            int result = circuit.Execute(sound, color);

            if (is_color != ColorType.BRANK){
                AnalyzeColor(is_color);
            }

            if (is_sound){
                AnalyzeSound();
            }

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

        private float time_count = 0;

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
                    time_count += Time.deltaTime;
                    if (time_count > FRAME_FOR_MOVE){
                        FinishMove();
                    }
                    break;
                case LobotState.LOOK:
                    _Look();
                    time_count += Time.deltaTime;
                    if (time_count > FRAME_FOR_LOOK){
                        FinishLook();
                    }
                    break;
                case LobotState.CHECK_COLOR:
                    time_count += Time.deltaTime;
                    if (time_count > FRAME_FOR_COLOR){
                        FinishAnalyzeColor();
                    }
                    break;
                case LobotState.CHECK_SOUND:
                    time_count += Time.deltaTime;
                    if (time_count > FRAME_FOR_SOUND){
                        FinishAnalyzeSound();
                    }
                    break;
                case LobotState.GAIN:
                    _Gain();
                    break;
            }
        }
    }
}
