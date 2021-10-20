using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public static class CollectionExtensions
{
    private static System.Random rng = new System.Random();
    /// <summary>
    /// Returns the sum of all float itens inside list.
    /// </summary>
    public static float Sum(this IList<float> array)
    {
        float value = 0;
        for (int i = 0; i < array.Count; i++)
        {
            value += array[i];
        }
        return value;
    }
    public static int Sum(this IList<int> array)
    {
        int value = 0;
        for (int i = 0; i < array.Count; i++)
        {
            value += array[i];
        }
        return value;
    }
    /// <summary>
    /// Shuffles the list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(this IList<T> list, System.Random random = null)
    {
        System.Random r = random==null? rng: random;
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = r.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T GetRandom<T>(this T[] array, List<int> excludedIndexs = null)
    {
        if (excludedIndexs != null && array.Length > 1)
        {
            int randomIndex = rng.Next(array.Length);

            while (excludedIndexs.Contains(randomIndex))
            {
                randomIndex = rng.Next(array.Length);
            }

            return array[randomIndex];
        }
        return array[rng.Next(array.Length)];
    }
    public static T GetRandom<T>(this T[] array, params T[] excludedItens)
    {
        if (excludedItens != null && array.Length > 1)
        {
            var filteredArray = array.Except(excludedItens).ToList();
            return filteredArray[rng.Next(filteredArray.Count)];
        }
        return array[rng.Next(array.Length)];
    }
    public static T GetRandom<T>(this IList<T> list, List<int> excludedIndexs = null)
    {
        if (excludedIndexs != null && list.Count > 1)
        {
            int randomIndex = rng.Next(list.Count);

            while (excludedIndexs.Contains(randomIndex))
            {
                randomIndex = rng.Next(list.Count);
            }

            return list[randomIndex];
        }
        return list[rng.Next(list.Count)];
    }
    public static T GetRandom<T>(this IList<T> list, params T[] excludedItens)
    {
        if (excludedItens != null && list.Count > 1)
        {
            var filteredArray = list.Except(excludedItens).ToList();
            return filteredArray[rng.Next(filteredArray.Count)];
        }
        return list[rng.Next(list.Count)];
    }

}

