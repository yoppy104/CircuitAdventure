using System.Collections.Generic;
using Lobot;

namespace Graph{
    public class Node
    {
        public Node prev = null;

        public List<Node> next = new List<Node>(){};

        public System.Object content;

        /// <summary> 次のノードを返却 </summary>
        public Node Next(int id){
            if (id >= next.Count) return null;
            return next[id];
        }

        ///<summary> 前のノードを返却 </summary>
        public Node Prev(){
            return prev;
        }

        public Node() {

        }

        public Node(System.Object obj){
            content = obj;
        }
    }
}
