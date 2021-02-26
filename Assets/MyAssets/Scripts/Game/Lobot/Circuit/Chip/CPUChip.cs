using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lobot{
    public class CPUChip : Chip
    {
        private bool is_sensor_connect = false;

        public CPUChip(ChipName name) : base(name, ChipType.SYSTEM){
            NumConnectLimit = 4;
        }

        public override int Execute()
        {
            if (is_sensor_connect){

            }

            return 0;
        }
    }
}
