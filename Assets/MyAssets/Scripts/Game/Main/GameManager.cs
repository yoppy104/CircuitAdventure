using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainSystem;
using MyInput;
using Map;
using Lobot;

namespace Game{
    public class GameManager : MyBehaviour
    {
        [SerializeField] private MapManager map;
        [SerializeField] private LobotManager lobot;
        
        public MapManager Map {
            get { return map; }
        }

        public LobotManager Lobot {
            get { return lobot; }
        }


        // Start is called before the first frame update
        public override void onStart()
        {
            lobot.map = map;

            lobot.InstanciateLobot();
        }

        ///<summary> キーボード入力のチェック </summary>
        private void CheckKeyInput(){
            if (InputManager.CheckUp()){
                lobot.MoveLobot(0, 1);
            }
            if (InputManager.CheckDown()){
                lobot.MoveLobot(0, -1);
                
            }
            if (InputManager.CheckRight()){
                lobot.MoveLobot(1, 0);
                
            }
            if (InputManager.CheckLeft()){
                lobot.MoveLobot(-1, 0);
        
            }
        }  

        // Update is called once per frame
        public override void onUpdate()
        {
            CheckKeyInput();
        }
    }
}
