using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyInput{

    ///<sumamry> マウス判定の返却用クラス </summary>
    public class RetMouse{
        public bool isTouch;
        public Vector3 mousePos;

        public RetMouse(bool is_touch, Vector3 pos){
            isTouch = is_touch;
            mousePos = pos;
        }

        public static RetMouse Failure{
            get;
        } = new RetMouse(false, Vector3.zero);
    }

    public static class InputManager
    {
        /// <summary> 上方向の入力がされたかを判定 </summary>
        public static bool CheckUp(){
            return Input.GetKeyDown(KeyCode.UpArrow);
        }

        /// <summary> 下方向の入力がされたかを判定 </summary>
        public static bool CheckDown(){
            return Input.GetKeyUp(KeyCode.DownArrow);
        }

        /// <summary> 右方向の入力がされたかを判定 </summary>
        public static bool CheckRight(){
            return Input.GetKeyDown(KeyCode.RightArrow);
        }

        /// <summary> 左方向の入力がされたかを判定 </summary>
        public static bool CheckLeft(){
            return Input.GetKeyDown(KeyCode.LeftArrow);
        }

        /// <summary> 決定の入力がされたかを判定 </summary>
        public static bool CheckOK(){
            return Input.GetKeyDown(KeyCode.Return);
        }


        ///<summary> 左クリックを押しているときの判定 </summary>
        public static RetMouse CheckMouseLeft(bool is_raw=false){
            if (is_raw){
                return new RetMouse(Input.GetMouseButton(0), RawMousePos());
            }
            return new RetMouse(Input.GetMouseButton(0), MousePosOnWorld());
        }

        ///<summary> 左クリックを押したときの判定</summary>
        public static RetMouse CheckMouseLeftDown(bool is_raw=false){
            if (is_raw){
                return new RetMouse(Input.GetMouseButtonDown(0), RawMousePos());
            }
            return new RetMouse(Input.GetMouseButtonDown(0), MousePosOnWorld());
        }

        ///<summary> 左クリックを離したときの判定 </summary>
        public static RetMouse CheckMouseLeftUp(bool is_raw=false){
            if (is_raw){
                return new RetMouse(Input.GetMouseButtonUp(0), RawMousePos());
            }
            return new RetMouse(Input.GetMouseButtonUp(0), MousePosOnWorld());
        }

        ///<summary> 右クリックを押しているときの判定 </summary>
        public static RetMouse CheckMouseRight(bool is_raw=false){
            if (is_raw){
                return new RetMouse(Input.GetMouseButton(1), RawMousePos());
            }
            return new RetMouse(Input.GetMouseButton(1), MousePosOnWorld());
        }

        ///<summary> 右クリックを押したときの判定 </summary>
        public static RetMouse CheckMouseRightDown(bool is_raw=false){
            if (is_raw){
                return new RetMouse(Input.GetMouseButtonDown(1), RawMousePos());
            }
            return new RetMouse(Input.GetMouseButtonDown(1), MousePosOnWorld());
        }


        ///<summary> 右クリックを離したときの判定 </summary>
        public static RetMouse CheckMouseRightUp(bool is_raw=false){
            if (is_raw){
                return new RetMouse(Input.GetMouseButtonUp(1), RawMousePos());
            }
            return new RetMouse(Input.GetMouseButtonUp(1), MousePosOnWorld());
        }

        ///<summary> 右左どちらかのマウスボタンが押された </summary>
        public static bool CheckClickLeftOrRight(){
            return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
        }

        ///<summary> ワールド座標に直したマウス位置を返す </summary>
        public static Vector3 MousePosOnWorld(float distance_rate=0){
            Vector3 pos = RawMousePos();
            pos.z = 10f + distance_rate;
            Vector3 res = Camera.main.ScreenToWorldPoint(pos);
            return res;
        }

        ///<summary> 生のマウス座標を返す </summary>
        public static Vector3 RawMousePos(){
            return Input.mousePosition;
        }

        // 長押し時間管理
        private static Dictionary<KeyCode, int> hold_time = new Dictionary<KeyCode, int>();

        /// <summary> 長押し判定の実処理 </summary>
        private static int ReturnHoldTime(KeyCode code){
            if (hold_time.ContainsKey(code)){
                hold_time.Add(code, 0);
            }
            if (Input.GetKey(code)){
                hold_time[code] += 1;
            }else{
                hold_time[code] = 0;
            }

            return hold_time[code];
        }

        /// <summary> 上方向の長押し </summary>
        public static int UpHoldTime(){
            return ReturnHoldTime(KeyCode.RightArrow);
        }

        /// <summary> 下方向の長押し </summary>
        public static int DownHoldTime(){
            return ReturnHoldTime(KeyCode.DownArrow);
        }

        /// <summary> 右方向の長押し </summary>
        public static int RightHoldTime(){
            return ReturnHoldTime(KeyCode.RightArrow);
        }

        /// <summary> 左方向の長押し </summary>
        public static int LeftHoldTime(){
            return ReturnHoldTime(KeyCode.LeftArrow);
        }
    }
}
