using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lobot{
    
    ///<summary> 取得アクションを実行する。 </summary>
    public class GainChip : Chip
    {
        public GainChip(ChipName name) : base(name, ChipType.ACTION) {
            NumConnectLimit = 0;
        }
    }
}
