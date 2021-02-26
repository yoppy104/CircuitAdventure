using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lobot{

    ///<summary> ロボットを移動させるアクション </summary>
    public class MoveChip : Chip
    {
        // 移動量
        public int actionType{
            get;
            set;
        } = -1;

        public MoveChip(ChipName name, ActionType _action) : base(name, ChipType.ACTION){
            actionType = (int)_action;
        }

        ///<summary> 実行 </summary>
        public override int Execute()
        {
            return actionType;
        }
    }
}
