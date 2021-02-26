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

        private void BuildDummyCircuit(){
            var temp = tree.root;

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

        public Circuit(){
            tree = new GraphTree();
            
            var temp = new Node(new CPUChip(ChipName.CPU));
            tree.root = temp;
            tree.now = temp;
        }

        ///<summary> ChipでNodeを検索する </summary>
        public Node FindNodeFromChip(Chip chip){
            Stack<Node> stack = new Stack<Node>();
            stack.Push(tree.root);

            // 幅優先探索
            while (stack.Count > 0){
                Node check = stack.Pop();
                Chip content = check.content as Chip;

                if (content == null) continue;

                if (content == chip) return check;

                // Nextに登録されているうち、Null以外のものを追加する。
                foreach (Node next_node in check.next){
                    if (next_node != null){
                        stack.Push(next_node);
                    }
                }
            }
            
            // 見つからないなら、nullを返す
            return null;
        }

        ///<summary> 特定のノードから下のノードを全て取得する。 </summary>
        public List<Node> GetBranchUnderNode(Node _root){
            List<Node> ret = new List<Node>();
            Stack<Node> stack = new Stack<Node>();

            stack.Push(_root);

            // 幅優先探索
            while (stack.Count > 0){
                Node check = stack.Pop();

                if (check == null) continue;
                ret.Add(check);

                foreach(Node next_node in check.next){
                    if (next_node != null){
                        stack.Push(next_node);
                    }
                }
            }

            return ret;
        }

        ///<summary> 特定のノードから下のノードを全て取得する。 </summary>
        public List<Node> GetBranchUnderNode(Chip chip){
            Node root_node = FindNodeFromChip(chip);

            return GetBranchUnderNode(root_node);
        }

        ///<summary> 特定のノードから続く全てのチップを取得する。 </summary>
        public List<Chip> GetUnderChips(Node _root) {
            List<Node> under_node = GetBranchUnderNode(_root);
            List<Chip> ret = new List<Chip>();

            // 取得したノードリストから、Chipを取得する。
            foreach(var check in under_node){
                if (check.content == null) continue;

                Chip temp = check.content as Chip;

                if (temp == null) continue;

                ret.Add(temp);
            }

            return ret;
        }

        ///<summary> 次のノードを追加する。Indexは指定可能 </summary>
        public bool SetNext(Node parent, Chip chip, int index=0){
            Chip parent_chip = parent.content as Chip;

            // 接続上限に達しているなら終了
            if (parent_chip.NumConnect == parent_chip.NumConnectLimit) return false;
            parent_chip.NumConnect++;

            // リストの内容量が足りないなら、nullでうめる
            if (parent.next.Count <= index){
                int num_necessary = index - parent.next.Count + 1;
                for (int i = 0; i < num_necessary; i++){
                    parent.next.Add(null);
                }
            }

            // Nodeデータを作成
            Node node = new Node(chip);
            node.prev = parent;

            // Nextに追加
            parent.next[index] = node;

            return true;
        }


        ///<summary> 名前からインスタンスを作成して、追加 </summary>
        public bool SetNext(Node parent, ChipName name, int index=0){
            Chip chip = ChipFactory.GetInstance(name);
            return SetNext(parent, chip, index);
        }


        /// <summary> チップを順番に参照する </summary>
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
