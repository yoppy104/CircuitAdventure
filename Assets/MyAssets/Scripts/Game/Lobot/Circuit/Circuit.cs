using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph;

namespace Lobot{
    public class Circuit
    {
        private GraphTree tree;

        // バッテリーチップは特別枠
        private BatteryChip battery;

        public Circuit(){
            tree = new GraphTree();

            var temp = new Node(new CPUChip());
            tree.root = temp;
            tree.now = temp;

            temp.next.Add(
                new Node(
                    new MoveChip(
                        new Vector2Int(0, 1)
                    )
                )
            );
        }
    }
}
