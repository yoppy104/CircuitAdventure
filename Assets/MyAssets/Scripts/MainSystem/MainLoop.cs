using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainSystem{

    ///<summary> ゲームのメインループ </summary>
    public class MainLoop : MonoBehaviour
    {
        private static MainLoop instance = null;

        public static MainLoop Instance{
            get { return instance; }
        }

        private List<MyBehaviour> behaviours = new List<MyBehaviour>();

        ///<summary> コンポーネントクラスを追加する</summary>
        public void AddBehaviour(MyBehaviour behaviour){
            behaviours.Add(behaviour);
        }

        void Awake(){
            if (instance != null){
                Destroy(this.gameObject);
            }
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            foreach (MyBehaviour behaviour in behaviours){
                if (behaviour.isActiveAndEnabled){
                    behaviour.onUpdate();
                }
            }
        }

        void LateUpdate()
        {
            foreach (MyBehaviour behaviour in behaviours){
                if (behaviour.isActiveAndEnabled){
                    behaviour.onLateUpdate();
                }
            }
        }

        void FixedUpdate()
        {
            foreach (MyBehaviour behaviour in behaviours){
                if (behaviour.isActiveAndEnabled){
                    behaviour.onFixedUpdate();
                }
            }
        }
    }
}
