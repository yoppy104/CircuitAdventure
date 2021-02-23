using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditCircuit{
    ///<summary> 回路編集画面のUIを管理する。 </summary>
    public class EditCircuitUI : MonoBehaviour
    {
        // ui名 : プレハブオブジェクト
        public Dictionary<string, UnityEngine.Object> ui_prefabs{
            get;
            private set;
        } = new Dictionary<string, UnityEngine.Object>();

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
