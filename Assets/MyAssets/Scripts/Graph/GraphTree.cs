using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graph{
    public class GraphTree
    {
        // ルートノード
        public Node root {
            get;
            set;
        } = null;

        // 現在参照ノード
        public Node now {
            get;
            set;
        } = null;


        ///<summary> 参照をルートノード </summary>
        public void Reset(){
            now = root;
        }

        ///<summary> 参照ノードを次のノードに移動する </summary>
        public bool Next(int id){
            if (now == null) return false;
            
            if (id >= now.next.Count){
                now = root;
                return false;
            }

            var temp = now.Next(id);

            now = temp;
            return true;
        }

        ///<summary> 参照ノードを前のノードに移動する </summary>
        public bool Prev(){
            var temp = now.Prev();
            if (temp == null){
                now = root;
                return false;
            }

            now = temp;
            return true;
        }


        public GraphTree() {

        }
    }
}

