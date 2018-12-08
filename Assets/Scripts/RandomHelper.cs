using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public static class RandomHelper
{
    public static IEnumerable<byte> RandomRangeNoRepeat(byte length, byte number)
    {
        byte[] list = new byte[length];
        for (byte i = 0; i < length; i++)
            list[i] = i;



        for (int i = 0; i < number; i++)
        {
            int random = Random.Range(i, length);

            byte swap = list[i];
            list[i] = list[random];
            list[random] = swap;
            
            yield return list[i];   // Only swap the required amount of times to save performance
        }
    }



    public static IEnumerable<T> Shuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int random = Random.Range(i, list.Count);

            T swap = list[i];
            list[i] = list[random];
            list[random] = swap;
            
            yield return list[i]; // Only swap the required amount of times to save performance
        }
    }
}
