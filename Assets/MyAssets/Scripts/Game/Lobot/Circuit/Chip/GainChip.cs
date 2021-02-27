using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lobot{
    
    ///<summary> 取得アクションを実行する。 </summary>
    public class GainChip : Chip
    {
        // 移動量
        public int actionType{
            get;
            set;
        } = (int)ActionType.GAIN;

        public GainChip(ChipName name) : base(name, ChipType.ACTION) {
            
        }

        ///<summary> 実行 </summary>
        public override int Execute()
        {
            return actionType;
        }
    }
}
