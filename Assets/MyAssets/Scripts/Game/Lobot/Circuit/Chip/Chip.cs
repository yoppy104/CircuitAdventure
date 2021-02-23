using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lobot{

    ///<summary> チップの種類 </summary>
    public enum ChipType{
        ACTION  = 1 << 0,     // ロボットの実動作
        BOOL    = 1 << 1,     // 真偽判定
        SYSTEM  = 1 << 2,     // システムチップ
        COUNT   = 1 << 3,     // 回数判定
        LEGEND  = 1 << 4      // 伝説チップ
    }

    public class Chip
    {
        // チップの種類
        public ChipType type{
            get;
            private set;
        }

        ///<summary> チップの機能を実行する。 </summary>
        public virtual int Execute(){
            return 0;
        }

        public Chip(ChipType _type){
            type = _type;
        }
    }
}
