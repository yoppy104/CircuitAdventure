using System;
using System.Collections.Generic;

namespace Common
{
    public static class Utility
    {
        ///<summary> 最大値のインデックスを返す </summary>
        public static int Argmax(int[] array){
            int ret = 0;
            int max_value = int.MinValue;
            for (int ind = 0; ind < array.Length; ind++){
                if (array[ind] > max_value){
                    max_value = array[ind];
                    ret = ind;
                }
            }

            return ret;
        }

        ///<summary> 最大値のインデックスを返す </summary>
        public static int Argmax(float[] array){
            int ret = 0;
            float max_value = int.MinValue;
            for (int ind = 0; ind < array.Length; ind++){
                if (array[ind] > max_value){
                    max_value = array[ind];
                    ret = ind;
                }
            }

            return ret;
        }

        ///<summary> 最大値のインデックスを返す </summary>
        public static int Argmax(List<int> list){
            return Argmax(list.ToArray());
        }

        ///<summary> 最大値のインデックスを返す </summary>
        public static int Argmax(List<float> list){
            return Argmax(list.ToArray());
        }
    }
}