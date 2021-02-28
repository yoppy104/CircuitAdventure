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
            {"max x", 9f},
            {"min x", -11f},
            {"max y", 5f},
            {"min y", -5f}
        };

        // チップを並べるときの距離
        public const float DISTANCE_CHIP = 1.6f;

        
        public List<ChipUI> useChips{
            get;
            private set;
        } = new List<ChipUI>();

        // CPUチップの基本位置
        public readonly static Vector3 CPU_STANDARD_POSITION = new Vector3(-1.25f, 0, 0);

        [SerializeField] EventTrigger up_chip_button = null;
        [SerializeField] EventTrigger right_chip_button = null;
        [SerializeField] EventTrigger left_chip_button = null;
        [SerializeField] EventTrigger down_chip_button = null;
        [SerializeField] EventTrigger sound_chip_button = null;
        [SerializeField] EventTrigger color_chip_button = null;
        [SerializeField] EventTrigger gain_chip_button = null;

        [SerializeField] Button map_button = null;
        [SerializeField] GameObject learge_map_image = null;

        [SerializeField] Button game_start_button = null;

        public EventTrigger UpChipButton { get { return up_chip_button; }}
        public EventTrigger RightChipButton { get { return right_chip_button; }}
        public EventTrigger LeftChipButton { get { return left_chip_button; }}
        public EventTrigger DownChipButton { get { return down_chip_button; }}


        [SerializeField] GameObject chip_config = null;

        [SerializeField] GameObject error_dialog = null;

        private Button config_delete = null;
        private Button config_red = null;
        private Button config_blue = null;
        private Button config_yellow = null;
        private Button config_green = null;

        private int num_gain_chip = 0;

        public ChipUIFactory factory {
            get; set;
        } = null;

        // 現在ドラッグしているUI
        private ChipUI now_drag = null;
        private ChipUI now_selected = null;

        private Action<ChipUI> onDrag = null;
        public void SetOnDrag(Action<ChipUI> action){
            onDrag = action;
        }

        ///<summary> ChipUIオブジェクトをドラッグ状態にする。 </summary>
        private void SetDrag(ChipUI chip){
            if (chip == null) return;

            // ドラッグモードに変更
            chip.ActivateChase();
            now_drag = chip;
            chip.SetScale(ChipUI.STANDARD_DRAG_SCALE);

            if (onDrag != null){
                onDrag(chip);
            }

            RemoveChipUI(chip);

            HideChipConfig();
        }

        ///<summary> 使用中チップリストから削除する </summary>
        private void RemoveChipUI(ChipUI chip){
            if (useChips.Contains(chip)){
                useChips.Remove(chip);
            }
        }

        
        ///<summary> 操作範囲を超えているかどうか判定する。 </summary>
        private bool CheckLimitOver(float x, float y){
            float x_min = LIMIT_UI_SPACE["min x"];
            float y_min = LIMIT_UI_SPACE["min y"];
            float x_max = LIMIT_UI_SPACE["max x"];
            float y_max = LIMIT_UI_SPACE["max y"];
            return (x_min > x || x > x_max) || (y_min > y || y > y_max);
        }

        ///<summary> 操作範囲を超えているかどうか判定する。 </summary>
        private bool CheckLimitOver(ChipUI chip){
            float x_min = LIMIT_UI_SPACE["min x"];
            float y_min = LIMIT_UI_SPACE["min y"];
            float x_max = LIMIT_UI_SPACE["max x"];
            float y_max = LIMIT_UI_SPACE["max y"];

            ChipUI.Corner corner = chip.GetCorner();

            // 4辺の接触判定
            Vector2[] checks = new Vector2[8]{
                new Vector2(x_min, corner.top.y),
                new Vector2(x_min, corner.bottom.y),
                new Vector2(corner.top.x, y_min),
                new Vector2(corner.bottom.x, y_min),
                new Vector2(x_max, corner.top.y),
                new Vector2(x_max, corner.bottom.y),
                new Vector2(corner.top.x, y_max),
                new Vector2(corner.bottom.x, y_max)
            };
            
            // どこかの座標が入っていたら、範囲を超えている。
            foreach (Vector2 check in checks){
                if (chip.CheckOnPoint(check)){
                    return true;
                }
            }

            return false;
        }

        ///<summary> チップを非アクティブにする処理 </summary>
        private void DisactiveChipUI(ChipUI chip){
            RemoveAll(chip);
            chip.Disactive();

            if (chip.Name == Lobot.ChipName.GAIN){
                num_gain_chip--;
            }

            if (onDisactiveChipUI != null){
                onDisactiveChipUI(chip);
            }

            int index = -1;

            for (int ind = 0; ind < useChips.Count; ind++){
                if (chip == useChips[ind]){
                    index = ind;
                    break;
                }
            }

            if (index != -1){
                useChips.RemoveAt(index);
            }
        }

        // チップを非アクティブにした時に走る処理(Managerから登録すること前提)
        private Action<ChipUI> onDisactiveChipUI = null;
        public void SetOnDisactiveChipUI(Action<ChipUI> action){
            onDisactiveChipUI = action;
        }

        ///<summary> ChipUIを左クリックした時の処理を追加する。</summary>
        private void SetLeftClickActionToChipUI(ChipUI chip){
            // EventTriggerがアタッチされていないなら終了
            EventTrigger trigger = chip.Trigger;
            if (trigger == null) return;

            Vector3 pos = chip.transform.position;

            EventTrigger.Entry press = new EventTrigger.Entry();
            press.eventID = EventTriggerType.PointerDown;
            press.callback.AddListener((data) => {
                Debug.Log("on click chip ui");

                // 左クリック
                // ドラッグ状態に変更
                if (InputManager.CheckMouseLeft().isTouch) {
                    Debug.Log("left click");
                    SetDrag(chip);
                }
                
                // 右クリック
                // コンフィグを表示
                else if (InputManager.CheckMouseRight().isTouch) {
                    Debug.Log("right click");
                    chip_config.SetActive(true);
                }
            });
            
            trigger.triggers.Add(press);
        }

        
        ///<summary> 解除する </summary>
        public void Remove(ChipUI target, ChipUI from){
            int index = -1;
            for (int i = 0; i < from.next_list.Count; i++){
                if (target == from.next_list[i]){
                    index = i;
                    break;
                }
            }

            if (index == -1) return;

            if (from.next_list[index] != null){
                DisactiveChipUI(from.next_list[index]);
            }
            from.next_list[index] = null;
        }

        ///<summary> 全てを解除する </summary>
        public void RemoveAll(ChipUI from){
            for (int index = 0; index < from.next_list.Count; index++){
                Remove(from.next_list[index], from);
            }
        }


        ///<summary> チップを離した時の処理 </summary>
        private void DropChip(Vector3 pos){

            // サイズを固定用に変更して、固定モードにする。
            now_drag.SetScale(ChipUI.STANDARD_SCALE);

            // ドロップした座標を設定
            now_drag.SetPosition3D(pos);

            // 最も近いチップを参照する。
            ChipUI nearest = FindNearestChipUI(now_drag);

            // 最も近いチップが接続できないなら、終了
            if (nearest == null || (!nearest.IsConnectable)){
                DisactiveChipUI(now_drag);

                // なんであれドラッグ状態は解除する。
                now_drag = null;
                return;
            }

            // 接触したチップが存在しないなら、終了
            if (nearest == null){
                DisactiveChipUI(now_drag);

                // なんであれドラッグ状態は解除する。
                now_drag = null;
                return;
            }

            // 最も適した設置方向を計算する。
            Vector3 set_pos = CalcSettingPos(
                now_drag.transform.position,
                nearest.transform.position
            );

            // 内部でドラッグ状態が解除されているなら、終了
            if (now_drag == null) return;

            now_drag.SetPosition3D(set_pos);
            
            // UI範囲を超えているなら、非アクティブにして終了。
            if (CheckLimitOver(now_drag)) {
                // 画面外なので非アクティブ
                DisactiveChipUI(now_drag);

                // なんであれドラッグ状態は解除する。
                now_drag = null;
                return;
            }

            // 全く同じ場所にチップが既にあるなら終了
            foreach (ChipUI use in useChips){
                if (Vector3.Equals(use.transform.position, now_drag.transform.position)){
                    // 画面外なので非アクティブ
                    DisactiveChipUI(now_drag);

                    // なんであれドラッグ状態は解除する。
                    now_drag = null;
                    return;
                }
            }

            // 再接続の可能性を考慮
            if (now_drag.parent != null){
                now_drag.parent.Disconnect(now_drag.connect_index);
                RemoveAll(now_drag);
                now_drag.Init();
            }

            // 位置を固定
            now_drag.FreezePos();
            
            // 使用中チップ配列に登録
            useChips.Add(now_drag);

            // 接続UIを表示
            Vector3[] linked_info = CalcLinkedUIPos(now_drag, nearest);


            // ChipUIを内部的に接続状態にする
            int index = 0;
            if (nearest.isCPU){
                Vector3 direction = now_drag.transform.position - nearest.transform.position;
                if (direction.x == 0){
                    if (direction.y < 0){
                        index = 2;
                    }else{
                        index = 0;
                    }
                }else if (direction.y == 0){
                    if (direction.x < 0){
                        index = 3;
                    }else{
                        index = 1;
                    }
                }
            }

            // 接続に失敗したら終了
            if (!nearest.Connect(now_drag, index)){
                DisactiveChipUI(now_drag);

                // なんであれドラッグ状態は解除する。
                now_drag = null;
                return;
            }
            
            ShowLinked(now_drag, linked_info);

            if (now_drag.Name == Lobot.ChipName.GAIN){
                num_gain_chip++;
            }

            now_drag = null;
        }

        ///<summary> ボタンを押したときの処理を追加する </summary>
        public void SetClickDown(EventTrigger trigger, UnityEngine.Events.UnityAction<BaseEventData> onclick){

            EventTrigger.Entry press = new EventTrigger.Entry();
            press.eventID = EventTriggerType.PointerDown;
            press.callback.AddListener(onclick);
            
            trigger.triggers.Add(press);
        }

        // チップをドロップしたときに行う処理をManagerから渡す。
        private Func<ChipUI, RetDropChip> onDropChipFromManager = null;


        public void SetOnDropChip(Func<ChipUI, RetDropChip> func){
            onDropChipFromManager = func;
        }

        private Vector3[] CalcLinkedUIPos(ChipUI new_chip, ChipUI pre_chip){
            Vector3 pos_new = new_chip.transform.position;
            Vector3 pos_pre = pre_chip.transform.position;
            Vector3 direction = CalcSettingDirection(pos_new, pos_pre);

            return new Vector3[] {(pos_new + pos_pre) / 2, direction};
        }

        /// <summary> リンクUIを表示 </summary>
        public void ShowLinked(ChipUI new_chip, Vector3[] info){
            if (info[1].x == 0){
                new_chip.ShowLinkedHorizontal(info[0]);
            }

            else if (info[1].y == 0){
                new_chip.ShowLinkedVertical(info[0]);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            chipui_obj = new GameObject("ChipUIs");
            chipui_obj.transform.parent = transform;

            // chipの詳細設定メニュー
            config_delete = chip_config.transform.GetChild(0).GetComponent<Button>();
            config_delete.onClick.AddListener(() => {
                HideChipConfig();
                now_selected = null;
            });

            config_red = chip_config.transform.GetChild(1).GetComponent<Button>();
            config_red.onClick.AddListener(() => {
                now_selected.ChangeMode(Map.ColorType.RED);
            });

            config_blue = chip_config.transform.GetChild(2).GetComponent<Button>();
            config_blue.onClick.AddListener(() => {
                now_selected.ChangeMode(Map.ColorType.BLUE);
            });

            config_green = chip_config.transform.GetChild(3).GetComponent<Button>();
            config_green.onClick.AddListener(() => {
                now_selected.ChangeMode(Map.ColorType.GREEN);
            });

            config_yellow = chip_config.transform.GetChild(4).GetComponent<Button>();
            config_yellow.onClick.AddListener(() => {
                now_selected.ChangeMode(Map.ColorType.YELLOW);
            });

            // ChipUIを生成するボタンの登録
            SetClickDown(UpChipButton, (data) => {
                // 左クリックで押していないなら何もしない
                if (! InputManager.CheckMouseLeft().isTouch) return;
                if (now_drag != null) return;

               var temp = factory.GetObject(EditCircuitManager.NAME_UP_CHIP, InputManager.MousePosOnWorld(-3), chipui_obj.transform);
                SetDrag(temp.GetComponent<ChipUI>());
            } );
            SetClickDown(RightChipButton, (data) => {
                // 左クリックで押していないなら何もしない
                if (! InputManager.CheckMouseLeft().isTouch) return;
                if (now_drag != null) return;

                var temp = factory.GetObject(EditCircuitManager.NAME_RIGHT_CHIP, InputManager.MousePosOnWorld(-3), chipui_obj.transform);
                SetDrag(temp.GetComponent<ChipUI>());
            } );
            SetClickDown(LeftChipButton, (data) => {
                // 左クリックで押していないなら何もしない
                if (! InputManager.CheckMouseLeft().isTouch) return;
                if (now_drag != null) return;

                var temp = factory.GetObject(EditCircuitManager.NAME_LEFT_CHIP, InputManager.MousePosOnWorld(-3), chipui_obj.transform);
                SetDrag(temp.GetComponent<ChipUI>());
            } );
            SetClickDown(DownChipButton, (data) => {
                // 左クリックで押していないなら何もしない
                if (! InputManager.CheckMouseLeft().isTouch) return;
                if (now_drag != null) return;

                var temp = factory.GetObject(EditCircuitManager.NAME_DOWN_CHIP, InputManager.MousePosOnWorld(-3), chipui_obj.transform);
                SetDrag(temp.GetComponent<ChipUI>());
            } );
            SetClickDown(sound_chip_button, (data) => {
                // 左クリックで押していないなら何もしない
                if (! InputManager.CheckMouseLeft().isTouch) return;
                if (now_drag != null) return;

                var temp = factory.GetObject(EditCircuitManager.NAME_SOUND_CHIP, InputManager.MousePosOnWorld(-3), chipui_obj.transform);
                SetDrag(temp.GetComponent<ChipUI>());
            } );
            SetClickDown(color_chip_button, (data) => {
                // 左クリックで押していないなら何もしない
                if (! InputManager.CheckMouseLeft().isTouch) return;
                if (now_drag != null) return;

                var temp = factory.GetObject(EditCircuitManager.NAME_COLOR_CHIP, InputManager.MousePosOnWorld(-3), chipui_obj.transform);
                SetDrag(temp.GetComponent<ChipUI>());
            } );
            SetClickDown(gain_chip_button, (data) => {
                // 左クリックで押していないなら何もしない
                if (! InputManager.CheckMouseLeft().isTouch) return;
                if (now_drag != null) return;

                var temp = factory.GetObject(EditCircuitManager.NAME_GAIN_CHIP, InputManager.MousePosOnWorld(-3), chipui_obj.transform);
                SetDrag(temp.GetComponent<ChipUI>());
            } );

            // マップボタンを押したら大きなマップを表示する。
            map_button.onClick.AddListener(() => {
                if (learge_map_image != null){
                    if (!is_erase_frame_lerge_map){
                        learge_map_image.SetActive(true);
                    }
                }
            });

            // ゲームスタートボタンでシーンをゲームに変更する様にする。
            game_start_button.onClick.AddListener(() => {
                if (num_gain_chip <= 0){
                    error_dialog.SetActive(true);
                    return;
                }

                // CPUチップをルートにして、コンパイル
                Compile(useChips[0]);

                // ゲームシーンに移動
                onChangeGameScene();
            });

            
            // CPUChipをセンター配置
            var temp = factory.GetObject(EditCircuitManager.NAME_CPU_CHIP, EditCircuitUI.CPU_STANDARD_POSITION, chipui_obj.transform);
            var script = temp.GetComponent<ChipUI>();
            useChips.Add(script);
        }

        public Action<ChipUI> Compile = null;
        public Action onChangeGameScene = null;

        // 直線描画コンポーネント用のGameObjectを束ねる。
        private GameObject chipui_obj = null;

        // 色一覧
        private Color[] colors = new Color[]{
            Color.red,
            Color.yellow,
            Color.green,
            Color.blue,
            Color.cyan,
            Color.magenta,
            Color.magenta + Color.cyan,
            Color.cyan + Color.yellow,
            Color.yellow + Color.magenta,
            Color.cyan + Color.blue,
            Color.magenta + Color.red,
            Color.green + Color.yellow
        };

        ///<summary> クリック位置にChipUIがあるかをチェック </summary>
        private ChipUI GetChipWithRay(Vector3 mouse_pos){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float distance = 100f;
            LayerMask mask = LayerMask.GetMask("ChipUI");

            var hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, distance, mask);

            if (hit.collider == null) return null;
            return hit.collider.GetComponent<ChipUI>();
        }

        ///<summary> 左クリックでChipUIを取得する。 </summary>
        private ChipUI GetChipOnLeftMouseClick(){
            var click = InputManager.CheckMouseLeftDown();
            if (click.isTouch){
                foreach (ChipUI chip in useChips){
                    if (chip.CheckOnPoint(click.mousePos)){
                        return chip;
                    }
                }
            }
            return null;
        }

        ///<summary> 右クリックでChipUIを取得する。 </summary>
        private ChipUI GetChipOnRightMouseClick(){
            var click = InputManager.CheckMouseRightDown();
            if (click.isTouch){
                foreach (ChipUI chip in useChips){
                    if (chip.CheckOnPoint(click.mousePos)){
                        return chip;
                    }
                }
            }
            return null;
        }

        bool is_erase_frame_lerge_map = false;

        // Update is called once per frame
        void Update()
        {
            // マップ画像が表示されている時の処理
            is_erase_frame_lerge_map = false;
            if (learge_map_image.activeSelf){
                if (InputManager.CheckMouseLeftDown().isTouch){
                    learge_map_image.SetActive(false);
                    is_erase_frame_lerge_map = true;
                }

                return;
            }

            //エラーダイアログを表示しているときの処理
            if (error_dialog.activeSelf){
                if (InputManager.CheckMouseLeftDown().isTouch){
                    error_dialog.SetActive(false);
                }
            }


            if (now_drag != null){
                // 左クリックを離したら、その場所で固定する。
                RetMouse ret = InputManager.CheckMouseLeftUp();
                if (ret.isTouch){
                    DropChip(ret.mousePos);
                }else{
                    now_drag.SetPosition3D(InputManager.MousePosOnWorld(-3));
                }

                return;
            }


            if (now_selected == null) {
                ChipUI left_click_chip = GetChipOnLeftMouseClick();
                if (left_click_chip != null){
                    if (! left_click_chip.isCPU){
                        SetDrag(left_click_chip);
                    }

                    return;
                }
            } else {
                bool is_hide = false;

                var click_info = InputManager.CheckMouseLeftDown(true);
                if (click_info.isTouch){
                    List<RaycastResult> results = new List<RaycastResult>();
                    // マウスポインタの位置にレイ飛ばし、ヒットしたものを保存
                    // ポインタ（マウス/タッチ）イベントに関連するイベントの情報
                    var pointer = new PointerEventData(EventSystem.current);
                    pointer.position = click_info.mousePos;
                    EventSystem.current.RaycastAll(pointer, results);
                    // ヒットしたUIの名前
                    is_hide = results.Count == 0;
                }

                if (is_hide){
                    HideChipConfig();
                    now_selected = null;

                    return;
                }
            }

            ChipUI right_click_chip = GetChipOnRightMouseClick();
            if (right_click_chip != null){
                if (! right_click_chip.isCPU){
                    now_selected = right_click_chip;
                    ShowChipConfig(right_click_chip.transform.position);
                }

                return;
            }
        }

        ///<summary> チップ編集UIを表示して選択中のチップの場所に合わせる </summary>
        private void ShowChipConfig(Vector3 chip_pos){
            chip_config.SetActive(true);
            config_delete.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(
                Camera.main, chip_pos + new Vector3(1f, 1f, 0)
            );
            if (now_selected.Name == Lobot.ChipName.COLOR){
                config_red.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(
                    Camera.main, chip_pos + new Vector3(1f, 0f, 0)
                );
                config_blue.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(
                    Camera.main, chip_pos + new Vector3(2f, 0f, 0)
                );
                config_green.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(
                    Camera.main, chip_pos + new Vector3(1f, -1f, 0)
                );
                config_yellow.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(
                    Camera.main, chip_pos + new Vector3(2f, -1f, 0)
                );

                ChangeDisplayColorConfig(true);
            }
        }

        // 色変更ボタンの表示を切り替える。
        private void ChangeDisplayColorConfig(bool flag){
            config_red.gameObject.SetActive(flag);
            config_blue.gameObject.SetActive(flag);
            config_green.gameObject.SetActive(flag);
            config_yellow.gameObject.SetActive(flag);
        }

        ///<summary> チップ編集UIを隠す </summary>
        private void HideChipConfig(){
            ChangeDisplayColorConfig(false);
            chip_config.SetActive(false);
        }

        
        ///<summary> 引数のチップから最も近いチップを探索する。 </summary>
        private ChipUI FindNearestChipUI(ChipUI chip){
            Vector3 pos = chip.transform.position;

            float min_distance = float.PositiveInfinity;
            ChipUI ret = null;

            foreach (var use_chip in useChips){
                if (use_chip.CheckHit(chip)){
                    float temp_distance = Vector3.Distance(use_chip.transform.position, pos);
                    if (temp_distance < min_distance) {
                        min_distance = temp_distance;
                        ret = use_chip;
                    }
                }
            }

            return ret;
        }

        ///<summary> 距離的に付けた側を判定する。 </summary>
        private Vector3 CalcSettingDirection(Vector3 new_pos, Vector3 old_pos){
            float dis_x = new_pos.x - old_pos.x;
            float dis_y = new_pos.y - old_pos.y;

            Vector3 ret = Vector3.one;
            ret.z = 0;

            // 絶対値が大きい軸のほうにつける。
            if (Mathf.Abs(dis_x) > Mathf.Abs(dis_y))
            {
                if (dis_x < 0){
                    ret.x = -1;
                }
                ret.y = 0;
            }
            else
            {
                ret.x = 0;
                if (dis_y < 0){
                    ret.y = -1;
                }
            }

            return ret;
        }


        ///<summary> 設置位置を計算する </summary>
        private Vector3 CalcSettingPos(Vector3 new_pos, Vector3 old_pos){
            return CalcSettingDirection(new_pos, old_pos) * EditCircuitUI.DISTANCE_CHIP + old_pos;
        }
    }
}
