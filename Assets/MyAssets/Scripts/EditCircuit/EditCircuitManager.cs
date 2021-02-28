using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyInput;
using Lobot;
using Graph;
using UnityEngine.SceneManagement;

namespace EditCircuit{

    ///<summary> チップを離した時に走る処理の返却クラス </summary>
    public class RetDropChip{
        public bool isHit;
        public Vector3 setPos;

        public RetDropChip(bool hit, Vector3 pos){
            isHit = hit;
            setPos = pos;
        }
    }

    ///<summary> 回路編集画面を管理する。 </summary>
    public class EditCircuitManager : MonoBehaviour
    {
        public const string NAME_UP_CHIP = "MoveChip_Up";
        public const string NAME_DOWN_CHIP = "MoveChip_Down";
        public const string NAME_RIGHT_CHIP = "MoveChip_Right";
        public const string NAME_LEFT_CHIP = "MoveChip_Left";
        public const string NAME_CPU_CHIP = "CPUChip";
        public const string NAME_SOUND_CHIP = "SoundChip";
        public const string NAME_COLOR_CHIP = "ColorChip";
        public const string NAME_GAIN_CHIP = "GainChip";
        public const string NAME_BACK_GROUND = "BG";

        // ui
        private EditCircuitUI ui = null;
        
        // object
        private ChipUIFactory factory = null;

        

        void Awake(){
            // ファクトリークラス
            factory = new ChipUIFactory();

            // UI管理クラスの設定
            ui = GetComponent<EditCircuitUI>();
            ui.factory = factory;

            RandomSetPosition();
            ui.SetSGSymbol();

            // リソースの読み込み
            factory.ui_prefabs.Add(NAME_CPU_CHIP, Resources.Load("Prefab/Lobot/Chip/CPUChip"));
            factory.ui_prefabs.Add(NAME_DOWN_CHIP, Resources.Load("Prefab/Lobot/Chip/MoveChip_Down"));
            factory.ui_prefabs.Add(NAME_UP_CHIP, Resources.Load("Prefab/Lobot/Chip/MoveChip_Up"));
            factory.ui_prefabs.Add(NAME_RIGHT_CHIP, Resources.Load("Prefab/Lobot/Chip/MoveChip_Right"));
            factory.ui_prefabs.Add(NAME_LEFT_CHIP, Resources.Load("Prefab/Lobot/Chip/MoveChip_Left"));
            factory.ui_prefabs.Add(NAME_SOUND_CHIP, Resources.Load("Prefab/Lobot/Chip/SoundChip"));
            factory.ui_prefabs.Add(NAME_COLOR_CHIP, Resources.Load("Prefab/Lobot/Chip/ColorChip"));
            factory.ui_prefabs.Add(NAME_GAIN_CHIP, Resources.Load("Prefab/Lobot/Chip/GainChip"));

            factory.ui_prefabs.Add(NAME_BACK_GROUND, Resources.Load("Prefab/BG/BG_EditCircuit"));


            // コンパイルを実行
            ui.Compile = (root) => {
                var circuit = CircuitCompile(root);
                Common.SharedData.Instance.shared_circuit = circuit;
            };

            ui.onChangeGameScene = () => {
                SceneManager.LoadScene("Game");
            };
        }

        private readonly Vector2Int[] CantSetPosition = new Vector2Int[]{
            new Vector2Int(8, 0),
            new Vector2Int(9, 0),
            new Vector2Int(9, 1),
            new Vector2Int(9, 2),
            new Vector2Int(3, 2),
            new Vector2Int(4, 2),
            new Vector2Int(5, 2),
            new Vector2Int(6, 2),
            new Vector2Int(0, 3),
            new Vector2Int(6, 3),
            new Vector2Int(7, 3),
            new Vector2Int(8, 3),
            new Vector2Int(0, 4),
            new Vector2Int(1, 4),
            new Vector2Int(2, 4),
            new Vector2Int(7, 4),
            new Vector2Int(8, 4),
            new Vector2Int(0, 5),
            new Vector2Int(1, 5),
            new Vector2Int(2, 5),
            new Vector2Int(7, 5),
            new Vector2Int(8, 5),
            new Vector2Int(0, 9),
            new Vector2Int(1, 9),
            new Vector2Int(2, 9),
            new Vector2Int(3, 9),
            new Vector2Int(9, 0),
            new Vector2Int(7, 2),
            new Vector2Int(3, 6)
        };

        ///<summary> ランダムなポジションで設定する。 </summary> 
        private void RandomSetPosition(){
            Random.InitState( System.DateTime.Now.Millisecond );

            Vector2Int start_pos = new Vector2Int(0, 0);

            while (true) {
                start_pos.x = Random.Range(0, 10);
                start_pos.y = Random.Range(0, 7);

                bool is_break = true;
                foreach(var pos in CantSetPosition){
                    if (Vector2Int.Equals(start_pos, pos)){
                        is_break = false;
                        break;
                    }
                }

                if (is_break){
                    break;
                }
            }

            Vector2Int goal_pos = new Vector2Int(0, 0);
            while (true) {
                goal_pos.x = Random.Range(0, 10);
                goal_pos.y = Random.Range(0, 7);

                bool is_break = true;

                if (Vector2Int.Equals(start_pos, goal_pos)){
                    continue;
                }

                is_break = true;
                foreach(var pos in CantSetPosition){
                    if (Vector2Int.Equals(goal_pos, pos)){
                        is_break = false;
                        break;
                    }
                }

                if (is_break){
                    break;
                }
            }

            Common.SharedData.Instance.start_pos = start_pos;
            Common.SharedData.Instance.goal_pos = goal_pos;
        }

        ///<summary> 回路全体をコンパイル </summary>
        private Circuit CircuitCompile(ChipUI root){
            Circuit ret = new Circuit();

            // cpuの接続上限だけを参照
            for (int i = 0; i < LimitConnect.Get(ChipName.CPU); i++){
                var temp = root.next_list[i];
                if (temp != null){
                    ret.Root.next[i] = NextAttach(temp);
                }
            }

            //Debug
            foreach(var node in ret.Root.next){
                if (node == null) {
                    continue;
                }
            }

            return ret;
        }

        ///<summary> あとに続くノードを全て追加したノードを返す。 </summary>
        private Node NextAttach(ChipUI chip) {
            // Chipインスタンスを作成して、追加
            var instance = ChipFactory.GetInstance(chip.Name);
            if (chip.Name == ChipName.COLOR){
                instance.colorType = chip.nowColor;
            }else if (chip.Name == ChipName.SOUND){
                instance.soundType = chip.nowSound;
            }

            Node ret = new Node(instance);
            // nextをnullでうめる
            ret.next.Clear();
            for (int i = 0; i < LimitConnect.Get(chip.Name); i++){
                ret.next.Add(null);
            }

            // nextを全て参照して次に続くNodeを追加
            for (int i = 0; i < LimitConnect.Get(chip.Name); i++){
                // 次を参照して、挿入
                var temp = chip.next_list[i];
                if (temp != null){
                    ret.next[i] = NextAttach(temp);
                }
            }

            return ret;
        }
    }
}
