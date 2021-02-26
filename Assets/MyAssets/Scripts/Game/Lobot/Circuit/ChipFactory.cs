using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lobot{
    public static class ChipFactory
    {
        private static Dictionary<ChipName, List<Chip>> instance_chache = new Dictionary<ChipName, List<Chip>>{
            {ChipName.CPU, new List<Chip>()},
            {ChipName.FORWARD_MOVE, new List<Chip>()},
            {ChipName.BACKWARD_MOVE, new List<Chip>()},
            {ChipName.LEFT_MOVE, new List<Chip>()},
            {ChipName.RIGHT_MOVE, new List<Chip>()},
            {ChipName.GAIN, new List<Chip>()},
            {ChipName.SOUND, new List<Chip>()},
            {ChipName.COLOR, new List<Chip>()},
        };

        public static Chip GetInstance(ChipName name){
            foreach(Chip chip in instance_chache[name]){
                if (!chip.isUse){
                    chip.SetActive(true);
                    return chip;
                }
            }

            // インスタンスを追加して使用する。
            Chip new_chip = null;
            switch(name){
                case ChipName.CPU:
                    new_chip = new CPUChip(name);
                    break;
                case ChipName.FORWARD_MOVE:
                    new_chip = new MoveChip(name, ActionType.FORWARD_MOVE);
                    break;
                case ChipName.BACKWARD_MOVE:
                    new_chip = new MoveChip(name, ActionType.BACKWARD_MOVE);
                    break;
                case ChipName.RIGHT_MOVE:
                    new_chip = new MoveChip(name, ActionType.RIGHT_MOVE);
                    break;
                case ChipName.LEFT_MOVE:
                    new_chip = new MoveChip(name, ActionType.LEFT_MOVE);
                    break;
                case ChipName.GAIN:
                    new_chip = new GainChip(name);
                    break;
                case ChipName.SOUND:
                    new_chip = new SoundChip(name);
                    break;
                case ChipName.COLOR:
                    new_chip = new ColorChip(name);
                    break;
            }
            
            if (new_chip != null) {
                instance_chache[name].Add(new_chip);
                new_chip.SetActive(true);
            }
            return new_chip;
        }
    }
}
