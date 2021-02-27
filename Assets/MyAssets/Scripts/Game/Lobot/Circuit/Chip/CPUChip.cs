using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lobot{
    public class CPUChip : Chip
    {
        private bool is_sensor_connect = false;
        private int now_index = -1;

        public void Reset(){
            now_index = -1;
        }

        public CPUChip(ChipName name) : base(name, ChipType.SYSTEM){
            
        }

        public override int Execute()
        {
            now_index = (now_index + 1) % NumConnectLimit;

            return now_index;
        }
    }
}
