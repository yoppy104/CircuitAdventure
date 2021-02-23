using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainSystem;

namespace Lobot{
    public class Lobot : MyBehaviour 
    {
        // 基盤
        private Circuit circuit;

        // マップ上での座標
        public Vector2Int positionOnMap {
            get;
            set;
        } = Vector2Int.zero;


        public void Move(int dx, int dy){
            positionOnMap += new Vector2Int(dx, dy);

            transform.position = MapCommon.MapScale2WorldScale(positionOnMap);
        }


        ///<summary> 基盤の実行 </summary>
        public void Execute(){

        }

        ///<summary> 基盤に従ったアクションを実行 </summary>
        public void Action(){

        }

        // Start is called before the first frame update
        public override void onStart()
        {
            
        }

        // Update is called once per frame
        public override void onUpdate()
        {
            
        }
    }
}
