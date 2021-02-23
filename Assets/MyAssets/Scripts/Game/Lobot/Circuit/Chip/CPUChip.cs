using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lobot{
    public class CPUChip : Chip
    {
        private bool is_sensor_connect = false;

        public CPUChip() : base(ChipType.SYSTEM){

        }

        public override bool Execute(Lobot lobot)
        {
            return false;
        }
    }
}
