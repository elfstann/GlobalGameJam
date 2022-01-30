using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomizeHelper
{
    public static Stack<T> ToRandomStack<T>(this List<T> array)
    {
        var randomArray = array.OrderBy(_ => Random.value);
        var stack = new Stack<T>(randomArray);
        return stack;
    }

    public static Stack<T> ToRandomStack<T>(this IEnumerable<T> array)
    {
        var randomArray = array.OrderBy(_ => Random.value);
        var stack = new Stack<T>(randomArray);
        return stack;
    }

    public static T GetRandom<T>(this List<T> list)
    {
        var random = Random.Range(0, list.Count);
        return list[random];
    }
}
