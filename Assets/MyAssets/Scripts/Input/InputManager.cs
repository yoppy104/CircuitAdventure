using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyInput{
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


        ///<summary> 左クリックを押したときの判定(マウスポジションで返却) </summary>
        public static Vector3 CheckMouseLeftDown(){
            if(Input.GetMouseButtonDown(0)){
                return Input.mousePosition;
            }else{
                return new Vector3(-1, -1, -1);
            }
        }

        ///<summary> 左クリックを離したときの判定 </summary>
        public static Vector3 CheckMouseLeftUp(){
            if (Input.GetMouseButtonUp(0)){
                return Input.mousePosition;
            }else{
                return new Vector3(-1, -1, -1);
            }
        }

        ///<summary> 右クリックを押したときの判定（マウスポジションで返却) </summary>
        public static Vector3 CheckMouseRightDown(){
            if (Input.GetMouseButtonDown(1)){
                return Input.mousePosition;
            }else{
                return new Vector3(-1, -1, -1);
            }
        }


        ///<summary> 右クリックを離したときの判定（マウスポジションで返却) </summary>
        public static Vector3 CheckMouseRightUp(){
            if (Input.GetMouseButtonUp(1)){
                return Input.mousePosition;
            }else{
                return new Vector3(-1, -1, -1);
            }
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
