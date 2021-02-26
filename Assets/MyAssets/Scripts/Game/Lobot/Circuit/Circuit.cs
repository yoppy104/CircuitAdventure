using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph;

namespace Lobot{
    public enum ChipName{
        CPU,
        FORWARD_MOVE,
        BACKWARD_MOVE,
        LEFT_MOVE,
        RIGHT_MOVE,
        GAIN,
        SOUND,
        COLOR,
        BATTERY
    }

    public class Circuit
    {
        private GraphTree tree;

        // バッテリーチップは特別枠
        private BatteryChip battery;

        public Circuit(){
            tree = new GraphTree();

            var temp = new Node(new CPUChip(ChipName.CPU));
            tree.root = temp;
            tree.now = temp;

            // 前進
            temp.next.Add(
                new Node(
                    new MoveChip(
                        ChipName.FORWARD_MOVE,
                        ActionType.FORWARD_MOVE
                    )
                )
            );
            temp = temp.next[0];

            // 右折
            temp.next.Add(
                new Node(
                    new MoveChip(
                        ChipName.RIGHT_MOVE,
                        ActionType.RIGHT_MOVE
                    )
                )
            );
            temp = temp.next[0];

            // 後退
            temp.next.Add(
                new Node(
                    new MoveChip(
                        ChipName.BACKWARD_MOVE,
                        ActionType.BACKWARD_MOVE
                    )
                )
            );
            temp = temp.next[0];

            // 左折
            temp.next.Add(
                new Node(
                    new MoveChip(
                        ChipName.LEFT_MOVE,
                        ActionType.LEFT_MOVE
                    )
                )
            );
        }

        public void SetNext(Node parent, ChipName name){

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
