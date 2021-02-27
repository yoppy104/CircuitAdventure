using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lobot{
    public class ColorChip : Chip
    {
        public ColorChip(ChipName name) : base(name, ChipType.BOOL){
            
        }

        ///<summary> 実行 </summary>
        public override int Execute()
        {
            return (int)colorType;
        }
    }
}
