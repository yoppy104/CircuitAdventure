using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lobot{

    ///<summary> チップの種類 </summary>
    public enum ChipType{
        ACTION  = 1 << 0,     // ロボットの実動作
        BOOL    = 1 << 1,     // 真偽判定
        SYSTEM  = 1 << 2,     // システムチップ
        COUNT   = 1 << 3,     // 回数判定
        LEGEND  = 1 << 4      // 伝説チップ
    }

    public class Chip
    {
        // チップの種類
        public ChipType type{
            get;
            protected set;
        }

        // 名前
        public ChipName Name{
            get;
            protected set;
        }

        // 接続可能なチップ数
        public int NumConnectLimit{
            get;
            protected set;
        }

        public int NumConnect{
            get;
            set;
        } = 0;

        // 使用中かどうか
        public bool isUse{
            get;
            protected set;
        }

        public EditCircuit.ChipUI UI{
            get;
            set;
        } = null;

        public Map.ColorType colorType{
            get;
            set;
        } = Map.ColorType.BRANK;

        public Map.SoundType soundType{
            get;
            set;
        } = Map.SoundType.SILENCE;

        ///<summary> チップの機能を実行する。 </summary>
        public virtual int Execute(){
            return 0;
        }

        public Chip(ChipName name, ChipType _type){
            type = _type;
            Name = name;

            NumConnectLimit = LimitConnect.Get(name);
        }

        // アクティベート
        public void SetActive(bool enable){
            isUse = enable;
        }
    }
}
