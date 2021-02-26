using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lobot{
    public class ColorChip : Chip
    {
        public ColorChip(ChipName name) : base(name, ChipType.BOOL){
            NumConnectLimit = 2;
        }
    }
}
