using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainSystem{

    /// <summary>シンプルコンポーネントクラス</summary>
    public class MyBehaviour : MonoBehaviour
    {
        private bool is_set_main_loop = false;

        public virtual void onStart(){

        }

        public virtual void onAwake(){

        }

        public virtual void onUpdate(){

        }

        public virtual void onLateUpdate(){

        }

        public virtual void onFixedUpdate(){
            
        }

        private void SetMainLoop(){
            if (is_set_main_loop) return;

            MainLoop instance = MainLoop.Instance;
            if (instance != null) instance.AddBehaviour(this);
        }

        public void Awake(){
            SetMainLoop();
            onAwake();
        }

        public void Start(){
            SetMainLoop();
            onStart();
        }

        public void Update(){
            SetMainLoop();
        }
    }
}
