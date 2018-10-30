using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class RandomHelper
{
    public static IEnumerable<int> RandomRangeNoRepeat(int start, int length, int number)
    {
        Debug.Assert(number <= length, "Number requested larger than range of random numbers.");



        int[] intList = Enumerable.Range(start, length).ToArray();

        for (int i = 0; i < number; i++)
        {
            int random = Random.Range(i, length);

            int swap = intList[i];
            intList[i] = intList[random];
            intList[random] = swap;
            
            yield return intList[i];   // Only swap the required amount of times to save performance
        }
    }

    public static IEnumerable<int> RandomRangeNoRepeat(int start, int length)
    {
        return RandomRangeNoRepeat(start, length, length);
    }



    public static IEnumerable<T> ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int random = Random.Range(i, list.Count);

            T swap = list[i];
            list[i] = list[random];
            list[random] = swap;
            
            yield return list[i];   // Only swap the required amount of times to save performance
        }
    }
}