using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainSystem{

    ///<summary>シングルトンコンポーネントクラス</summary>
    public class SingletonBehaviour : MyBehaviour
    {
        private static SingletonBehaviour instance = null;

        public override void onAwake(){
            // シングルトン用のインスタンスを初期化
            if (instance != null){
                Destroy(this.gameObject);
            }
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
