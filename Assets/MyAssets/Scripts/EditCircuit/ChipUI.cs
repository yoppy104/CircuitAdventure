using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyInput;
using UnityEngine.EventSystems;
using System;

namespace EditCircuit{
    ///<summary> チップ編集画面でチップを表現するUI </summary>
    [RequireComponent(typeof(EventTrigger))]
    public class ChipUI : MonoBehaviour, IPointerClickHandler
    {
        public enum State{
            STATIC,
            CHASE_MOUSE,
            SELECTED
        }

        [SerializeField] private bool is_cpu = false;
        public bool isCPU{
            get { return is_cpu; }
        }

        public Action RightClickAction = null;
        public Action LeftClickAction = null;

        ///<summary> クリックしたときの処理 </summary>
        public void OnPointerClick(PointerEventData data){
            // 左クリックの処理
            if (InputManager.CheckMouseLeft().isTouch){
                Debug.Log("[ChipUI]left click");
            }

            // 右クリックの処理
            if (InputManager.CheckMouseRight().isTouch){
                Debug.Log("[ChipUI]right click");
            }
        }

        private readonly static Vector2 CORNER_SIZE = new Vector2(0.7f, 0.7f);
        private readonly static Vector2 DRAG_CORNER_SIZE = new Vector2(1.6f, 1.6f);

        
        private Vector2 CornerSize{
            get {
                if (state == State.STATIC){
                    return CORNER_SIZE;
                }else{
                    return DRAG_CORNER_SIZE;
                }
            }
        }

        private Dictionary<string, GameObject> connecter_image = new Dictionary<string, GameObject>();

        public EventTrigger Trigger{
            get;
            private set;
        }

        // 扱うチップの名称
        [SerializeField] private Lobot.ChipName name = Lobot.ChipName.CPU;
        public Lobot.ChipName Name{
            get { return name; }
        }

        private int connect_limit = 0;

        public int numConnect{
            get;
            set;
        } = 0;

        // 接続元のインデックス番号
        public int connect_index = -1;

        // 接続しているChipUI
        public List<ChipUI> next_list{
            get;
            private set;
        } = new List<ChipUI>();
        public ChipUI parent = null;

        ///<summary> 接続する </summary>
        public bool Connect(ChipUI chip, int index){
            if (! IsConnectable ) return false;
            if ( index >= next_list.Count ) return false;
            if ( index >= connect_limit ) return false;

            if (next_list[index] == null){
                next_list[index] = chip;
                chip.parent = this;
                numConnect++;
                chip.connect_index = index;

                return true;
            }

            return false;
        }

        ///<summary> 非アクティブ化 </summary>
        public void Disactive(){
            FreezePos();
            AllDisactiveLinkedUI();

            if (parent != null){
                Debug.Log("disconnect");
                parent.Disconnect(connect_index);
            }
            parent = null;
            connect_index = -1;

            numConnect = 0;
            next_list.Clear();

            gameObject.SetActive(false);
        }

        ///<summary> 切断する </summary>
        public void Disconnect(ChipUI chip){
            int index = -1;
            for (int i = 0; i < next_list.Count; i++){
                if (next_list[i] == chip){
                    index = 1;
                    break;
                }
            }

            Disconnect(index);
        }


        ///<summary> 切断する </summary>
        public bool Disconnect(int index){
            if ( index >= connect_limit ) return false;
            if ( index >= next_list.Count ) return false;

            var temp = next_list[index];
            if (next_list[index] != null){
                next_list[index] = null;
                temp.parent = null;
                numConnect--;
                return true;
            }

            return false;
        }

        ///<summary> 接続可能かどうか </summary>
        public bool IsConnectable{
            get { return numConnect < connect_limit; }
        }

        ///<summary> 四隅の座標を管理 </summary>
        public class Corner{
            public Vector2 top = Vector2.zero;
            public Vector2 bottom = Vector2.zero;

            public Corner(){}

            public void Update(Vector3 pos, Vector2 scale){
                Vector2 temp = new Vector2(pos.x, pos.y);

                top = temp + scale;
                bottom = temp - scale;
            }
        }

        private Corner corner = new Corner();

        // 四隅の座標を計算してから返す
        public Corner GetCorner(){
            corner.Update(transform.position, CornerSize);
            return corner;
        }

        public State state{
            get;
            private set;
        } = State.STATIC;

        ///<summary> マウスを追従する様に設定 </summary>
        public void ActivateChase(){
            state = State.CHASE_MOUSE;
        }

        ///<summary> 位置を確定して固定する </summary>
        public void FreezePos(){
            state = State.STATIC;
        }

        public readonly static Vector3 STANDARD_SCALE = new Vector3(0.05f, 0.05f, 0.05f);
        public readonly static Vector3 STANDARD_DRAG_SCALE = new Vector3(0.035f, 0.035f, 0.035f);

        ///<summary> スケールを更新する </summary>
        public void SetScale(Vector3 scale){
            transform.localScale = scale;
        }

        ///<summary> 起動毎の初期化 </summary>
        public void Init(){
            next_list.Clear();
            connect_limit = Lobot.LimitConnect.Get(name);
            for (int i = 0; i < connect_limit; i++){
                next_list.Add(null);
            }

            numConnect = 0;

            parent = null;
            connect_index = -1;

        }

        public Transform linked_horizontal = null;
        public Transform linked_vertical = null;

        ///<summary> 全ての接続UIを非表示 </summary>
        public void AllDisactiveLinkedUI(){
            if (linked_horizontal != null){
                linked_horizontal.gameObject.SetActive(false);
            }
            if (linked_vertical != null){
                linked_vertical.gameObject.SetActive(false);
            }
        }

        void Start(){
            // 四隅の座標リストを作成
            string[] names = {"down", "left", "right", "up"};
            int ind = 0;
            foreach (Transform child in transform){
                connecter_image.Add(names[ind++], child.gameObject);
            }

            // EventTriggerを追加
            Trigger = GetComponent<EventTrigger>();

            linked_vertical = transform.GetChild(0);
            linked_horizontal = transform.GetChild(1);

            Init();
        }

        // Start is called before the first frame update
        void OnEnable()
        {
            Init();
        }

        ///<summary> 座標を更新する。 </summary>
        public void SetPosition(float x, float y){
            transform.position = new Vector3(x, y, 0f);
        }

        ///<summary> 座標を更新する</summary>
        public void SetPosition(Vector2 pos){
            SetPosition(pos.x, pos.y);
        }

        ///<summary> 3次元座標を更新する </summary>
        public void SetPosition3D(Vector3 pos){
            transform.position = pos;
        }

        ///<summary> 3次元座標を更新する </summary>
        public void SetPosition3D(float x, float y, float z){
            SetPosition3D(new Vector3(x, y, z));
        }

        ///<summary> 指定座標上に乗っているか </summary>
        public bool CheckOnPoint(float x, float y){
            Vector2 top = corner.top;
            Vector2 bottom = corner.bottom;
            return (bottom.x <= x && x <= top.x) && (bottom.y <= y && y <= top.y);
        }

        ///<summary> 指定座標上に乗っているか </summary>
        public bool CheckOnPoint(Vector2 pos){
            return CheckOnPoint(pos.x, pos.y);
        }

        ///<summary> 他のChipUIと接触しているかどうか </summary>
        public bool CheckHit(ChipUI chip) {
            Corner _corner_chip = chip.GetCorner();
            Corner _corner_this = this.GetCorner();

            // それぞれの大きさを計算する
            float size_chip = Vector2.Distance(_corner_chip.top, _corner_chip.bottom);
            float size_this = Vector2.Distance(_corner_this.top, _corner_this.bottom);

            // サイズが小さい方の頂点が大きい方の範囲内に入っていることを判定すればよい。
            bool is_check_this = true; // thisの方が小さい場合
            if (size_chip > size_this){
                is_check_this = false; // chipの方が小さい場合
            }

            // this側が小さい時の判定
            if (is_check_this){
                // this側のチェックで判定する座標群
                Vector2[] this_check = new Vector2[4] {
                    _corner_chip.top,
                    new Vector2(_corner_chip.top.x, _corner_chip.bottom.y),
                    _corner_chip.bottom,
                    new Vector2(_corner_chip.bottom.x, _corner_chip.top.y)
                };
                
                // this側のあたり判定
                foreach (Vector2 check in this_check){
                    if (this.CheckOnPoint(check)){
                        return true;
                    }
                }
            }
            // chip側が小さい時の判定
            else{
                // chip側のチェックで判定する座標群
                Vector2[] chip_check = new Vector2[4] {
                    _corner_this.top,
                    new Vector2(_corner_this.top.x, _corner_this.bottom.y),
                    _corner_this.bottom,
                    new Vector2(_corner_this.bottom.x, _corner_this.top.y)
                };

                // chip側のあたり判定
                foreach (Vector2 check in chip_check){
                    if (chip.CheckOnPoint(check)){
                        return true;
                    }
                }
            }
            
            // 全てのチェックを通過したら、接触していない
            return false;
        }
    }
}
