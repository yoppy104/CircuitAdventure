using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditCircuit{

    ///<summary> ChipUI関連のオブジェクトの生成、キャッシュを管理 </summary>
    public class ChipUIFactory
    {
        // ui名 : プレハブオブジェクト
        public Dictionary<string, UnityEngine.Object> ui_prefabs{
            get;
            private set;
        } = new Dictionary<string, UnityEngine.Object>();

        // ui名 : GameObjectのキャッシュ
        private Dictionary<string, List<GameObject>> object_lists{
            get;
            set;
        } = new Dictionary<string, List<GameObject>>();

        public ChipUIFactory(){

        }

        ///<summary> ゲームオブジェクトの取得（キャッシュを利用) </summary>
        public GameObject GetObject(string name, Vector3 position, Transform parent=null){
            // キャッシュを参照
            if (object_lists.ContainsKey(name)){
                foreach (GameObject obj in object_lists[name]){
                    if (! obj.activeSelf){
                        obj.SetActive(true);
                        obj.transform.parent = parent;
                        return obj;
                    }
                }
            }else{
                object_lists.Add(name, new List<GameObject>());
            }

            // インスタンスを作成
            GameObject temp = MonoBehaviour.Instantiate(ui_prefabs[name], position, Quaternion.identity) as GameObject;
            object_lists[name].Add(temp);
            temp.transform.parent = parent;
            return temp;
        }
    }
}
