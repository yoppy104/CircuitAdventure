using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyInput;
using System;
using UnityEngine.EventSystems;

namespace EditCircuit{
    ///<summary> 回路編集画面のUIを管理する。 </summary>
    public class EditCircuitUI : MonoBehaviour
    {
        // チップを配置できる上限値
        public readonly static Dictionary<string, float> LIMIT_UI_SPACE = new Dictionary<string, float>(){
            {"max x", 8.6f},
            {"min x", -10.6f},
            {"max y", 4.8f},
            {"min y", -4.8f}
        };

        // チップを並べるときの距離
        public const float DISTANCE_CHIP = 1.6f;

        // CPUチップの基本位置
        public readonly static Vector3 CPU_STANDARD_POSITION = new Vector3(-1, 0, 0);

        [SerializeField] EventTrigger up_chip_button = null;
        [SerializeField] EventTrigger right_chip_button = null;
        [SerializeField] EventTrigger left_chip_button = null;
        [SerializeField] EventTrigger down_chip_button = null;

        public ChipUIFactory factory {
            get; set;
        } = null;

        // 現在ドラッグしているUI
        private ChipUI now_drag = null;

        ///<summary> ChipUIオブジェクトをドラッグ状態にする。 </summary>
        private void SetDrag(GameObject obj){
            // コンポーネントのnullチェック
            var src = obj.GetComponent<ChipUI>();
            if (src == null) return;

            // ドラッグモードに変更
            src.ActivateChase();
            now_drag = src;
            src.SetScale(ChipUI.STANDARD_DRAG_SCALE);
        }

        
        ///<summary> 操作範囲を超えているかどうか判定する。 </summary>
        private bool CheckLimitOver(float x, float y){
            float x_min = LIMIT_UI_SPACE["min x"];
            float y_min = LIMIT_UI_SPACE["min y"];
            float x_max = LIMIT_UI_SPACE["max x"];
            float y_max = LIMIT_UI_SPACE["max y"];
            return (x_min > x || x > x_max) || (y_min > y || y > y_max);
        }

        ///<summary> チップを離した時の処理 </summary>
        private void DropChip(Vector3 pos){

            // UI範囲を超えているなら、処理を終了。
            if (CheckLimitOver(pos.x, pos.y)) {
                // 画面外なので非アクティブ
                now_drag.gameObject.SetActive(false);
                now_drag.FreezePos();

                // なんであれドラッグ状態は解除する。
                now_drag = null;
                return;
            }

            // 位置とサイズを合わせて、座標を固定する。
            now_drag.SetPosition3D(pos);
            now_drag.SetScale(ChipUI.STANDARD_SCALE);
            now_drag.FreezePos();

            now_drag = null;
        }

        ///<summary> ボタンを押したときの処理を追加する </summary>
        private void SetClickDown(EventTrigger trigger, UnityEngine.Events.UnityAction<BaseEventData> onclick){

            EventTrigger.Entry press = new EventTrigger.Entry();
            press.eventID = EventTriggerType.PointerDown;
            press.callback.AddListener(onclick);
            
            trigger.triggers.Add(press);
        }

        // Start is called before the first frame update
        void Start()
        {
            // ChipUIを生成するボタンの登録
            SetClickDown(up_chip_button, (data) => {
                // 左クリックで押していないなら何もしない
                if (InputManager.CheckMouseLeftDown().x == -1) return;
               var temp = factory.GetObject(EditCircuitManager.NAME_UP_CHIP, InputManager.MousePosOnWorld(-3), transform);
                SetDrag(temp);
            } );
            SetClickDown(right_chip_button, (data) => {
                // 左クリックで押していないなら何もしない
                if (InputManager.CheckMouseLeftDown().x == -1) return;
                var temp = factory.GetObject(EditCircuitManager.NAME_RIGHT_CHIP, InputManager.MousePosOnWorld(-3), transform);
                SetDrag(temp);
            } );
            SetClickDown(left_chip_button, (data) => {
                // 左クリックで押していないなら何もしない
                if (InputManager.CheckMouseLeftDown().x == -1) return;
                var temp = factory.GetObject(EditCircuitManager.NAME_LEFT_CHIP, InputManager.MousePosOnWorld(-3), transform);
                SetDrag(temp);
            } );
            SetClickDown(down_chip_button, (data) => {
                // 左クリックで押していないなら何もしない
                if (InputManager.CheckMouseLeftDown().x == -1) return;
                var temp = factory.GetObject(EditCircuitManager.NAME_DOWN_CHIP, InputManager.MousePosOnWorld(-3), transform);
                SetDrag(temp);
            } );

            // CPUChipをセンター配置
            var temp = factory.GetObject(EditCircuitManager.NAME_CPU_CHIP, CPU_STANDARD_POSITION, transform);
        }

        // Update is called once per frame
        void Update()
        {
            if (now_drag != null){
                // 左クリックを離したら、その場所で固定する。
                Vector3 pos = InputManager.CheckMouseLeftUp();
                if (pos.x != -1){
                    DropChip(pos);
                }else{
                    now_drag.SetPosition3D(InputManager.MousePosOnWorld(-3));
                }

            }
        }
    }
}
