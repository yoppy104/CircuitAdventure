using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditCircuit{
    ///<summary> 回路編集画面を管理する。 </summary>
    public class EditCircuit : MonoBehaviour
    {
        // ui
        private EditCircuitUI ui = null;

        void Start(){
            ui = GetComponent<EditCircuitUI>();

            // リソースの読み込み
            ui.ui_prefabs.Add("CPUChip", Resources.Load("Prefab/Lobot/Chip/CPUChip"));
            ui.ui_prefabs.Add("MoveChip_Down", Resources.Load("Prefab/Lobot/Chip/MoveChip_Down"));
            ui.ui_prefabs.Add("MoveChip_Up", Resources.Load("Prefab/Lobot/Chip/MoveChip_Up"));
            ui.ui_prefabs.Add("MoveChip_Right", Resources.Load("Prefab/Lobot/Chip/MoveChip_Right"));
            ui.ui_prefabs.Add("MoveChip_Left", Resources.Load("Prefab/Lobot/Chip/MoveChip_Left"));

            ui.ui_prefabs.Add("BG", Resources.Load("Prefab/BG/BG_EditCircuit"));

            
        }
    }
}
