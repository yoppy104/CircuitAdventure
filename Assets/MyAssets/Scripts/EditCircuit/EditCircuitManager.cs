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

            // リソースの読み込み
            factory.ui_prefabs.Add(NAME_CPU_CHIP, Resources.Load("Prefab/Lobot/Chip/CPUChip"));
            factory.ui_prefabs.Add(NAME_DOWN_CHIP, Resources.Load("Prefab/Lobot/Chip/MoveChip_Down"));
            factory.ui_prefabs.Add(NAME_UP_CHIP, Resources.Load("Prefab/Lobot/Chip/MoveChip_Up"));
            factory.ui_prefabs.Add(NAME_RIGHT_CHIP, Resources.Load("Prefab/Lobot/Chip/MoveChip_Right"));
            factory.ui_prefabs.Add(NAME_LEFT_CHIP, Resources.Load("Prefab/Lobot/Chip/MoveChip_Left"));

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
                    Debug.Log("[Compile] null");
                    continue;
                }
                Debug.Log("[Compile]" + ((Chip)node.content).Name);
            }

            return ret;
        }

        ///<summary> あとに続くノードを全て追加したノードを返す。 </summary>
        private Node NextAttach(ChipUI chip) {
            Debug.Log("[NextAttach]" + chip.Name);

            // Chipインスタンスを作成して、追加
            Node ret = new Node(ChipFactory.GetInstance(chip.Name));
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
