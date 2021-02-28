using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph;
using Map;

namespace Lobot{
    public enum ChipName{
        CPU,
        FORWARD_MOVE,
        BACKWARD_MOVE,
        LEFT_MOVE,
        RIGHT_MOVE,
        GAIN,
        SOUND,
        COLOR
    }

    /// <summary> チップ毎の接続数上限 </summary>
    public static class LimitConnect{
        private const int cpu = 4;
        private const int move = 1;
        private const int gain = 0;
        private const int sound = 1;
        private const int color = 1;
        
        public static int Get(ChipName name){
            switch(name){
                case ChipName.CPU:
                    return cpu;

                case ChipName.FORWARD_MOVE:
                case ChipName.BACKWARD_MOVE:
                case ChipName.RIGHT_MOVE:
                case ChipName.LEFT_MOVE:
                    return move;
                
                case ChipName.GAIN:
                    return gain;

                case ChipName.SOUND:
                    return sound;
                
                case ChipName.COLOR:
                    return color;
            }
            return 0;
        }
    }

    public class Circuit
    {
        private GraphTree tree;

        public GraphTree Tree{
            get { return tree; }
        }

        public Node Root{
            get { return tree.root; }
        }

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
            
            // rootはCPUで固定
            var temp = new Node(ChipFactory.GetInstance(ChipName.CPU));
            // nextをnullでうめる
            temp.next.Clear();
            for (int i = 0; i < LimitConnect.Get(ChipName.CPU); i++){
                temp.next.Add(null);
            }

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

        ///<summary> 現在色チップかを調べる。 </summary>
        public ColorType IsColor(){
            if (tree.now == null) return ColorType.BRANK;

            if ((tree.now.content as Chip).Name == ChipName.COLOR){
                return (tree.now.content as ColorChip).colorType;
            }

            return ColorType.BRANK;
        }

        ///<summary> 現在音声チップかを調べる </summary>
        public bool IsSound(){
            if (tree.now == null) return false;

            return (tree.now.content as Chip).Name == ChipName.SOUND;
        }


        ///<summary> 名前からインスタンスを作成して、追加 </summary>
        public bool SetNext(Node parent, ChipName name, int index=0){
            Chip chip = ChipFactory.GetInstance(name);
            return SetNext(parent, chip, index);
        }

        /// <summary> チップを順番に参照する </summary>
        public int Execute(SoundType sound, ColorType color){
            Chip chip = tree.now.content as Chip;

            int result = chip.Execute();
            
            if (chip.type == ChipType.ACTION){
                tree.Next(0);
                if (tree.now == null){
                    tree.Reset();
                }
                return result;
            }
            
            // 色チェック
            else if (chip.Name == ChipName.COLOR){
                if (result == (int)color){
                    tree.Next(0);
                    if (tree.now == null){
                        tree.Reset();
                    }
                }else{
                    tree.Reset();
                }

                return -1;
            }

            // 音声チェック
            else if (chip.Name == ChipName.SOUND){
                if (result == (int) sound){
                    tree.Next(0);
                    if (tree.now == null){
                        tree.Reset();
                    }
                }else{
                    tree.Reset();
                }
                return -1;
            }

            tree.Next(result);

            if (tree.now == null){
                tree.Reset();
            }

            return -1;
        }
    }
}
