using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lobot{

    ///<summary> ロボットを移動させるアクション </summary>
    public class MoveChip : Chip
    {
        // 移動量
        public Vector2Int deltaMove{
            get;
            set;
        } = Vector2Int.zero;

        public MoveChip(Vector2Int delta) : base(ChipType.ACTION){
            deltaMove = delta;
        }
    }
}
