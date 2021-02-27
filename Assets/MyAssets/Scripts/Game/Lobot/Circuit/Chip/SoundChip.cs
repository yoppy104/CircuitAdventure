using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lobot{
    public class SoundChip : Chip
    {
        public SoundChip(ChipName name) : base(name, ChipType.BOOL){
            
        }

        ///<summary> 実行 </summary>
        public override int Execute()
        {
            return (int)soundType;
        }
    }
}
