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

            // 前進
            temp.next.Add(
                new Node(
                    new MoveChip(
                        ActionType.FORWARD_MOVE
                    )
                )
            );
            temp = temp.next[0];

            // 右折
            temp.next.Add(
                new Node(
                    new MoveChip(
                        ActionType.RIGHT_MOVE
                    )
                )
            );
            temp = temp.next[0];

            // 後退
            temp.next.Add(
                new Node(
                    new MoveChip(
                        ActionType.BACKWARD_MOVE
                    )
                )
            );
            temp = temp.next[0];

            // 左折
            temp.next.Add(
                new Node(
                    new MoveChip(
                        ActionType.LEFT_MOVE
                    )
                )
            );
        }

        public int Execute(){
            var chip = tree.now.content as Chip;

            int result = chip.Execute();
            
            if (chip.type == ChipType.ACTION){
                tree.Next(0);
                return result;
            }

            tree.Next(result);

            if (((Chip)tree.now.content).type == ChipType.SYSTEM){
                return -1;
            }

            return Execute();
        }
    }
}
