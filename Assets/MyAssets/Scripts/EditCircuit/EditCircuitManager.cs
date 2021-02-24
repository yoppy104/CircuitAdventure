using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditCircuit{
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
        }
    }
}
