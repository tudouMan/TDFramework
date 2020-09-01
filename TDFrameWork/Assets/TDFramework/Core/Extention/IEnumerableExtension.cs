using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Text;

#if UNITY_5_6_OR_NEWER
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
#endif


namespace TDFramework.Extention
{

    /// <summary>
    /// 可枚举的集合扩展（Array、List<T>、Dictionary<K,V>)
    /// </summary>
    public static class IEnumerableExtension
    {
        #region Array Extension

        /// <summary>
        /// 遍历数组
        /// <code>
        /// var testArray = new[] { 1, 2, 3 };
        /// testArray.ForEach(number => number.ToString());
        /// </code>
        /// </summary>
        /// <returns>The each.</returns>
        /// <param name="selfArray">Self array.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns> 返回自己 </returns>
        public static T[] ForEach<T>(this T[] selfArray, Action<T> action)
        {
            Array.ForEach(selfArray, action);
            return selfArray;
        }

        /// <summary>
        /// 遍历 IEnumerable
        /// <code>
        /// // IEnumerable<T>
        /// IEnumerable<int> testIenumerable = new List<int> { 1, 2, 3 };
        /// testIenumerable.ForEach(number => number.ToString());
        /// // 支持字典的遍历
        /// new Dictionary<string, string>()
        ///         .ForEach(keyValue => Debug.Log("key:{0},value:{1}", keyValue.Key, keyValue.Value));
        /// </code>
        /// </summary>
        /// <returns>The each.</returns>
        /// <param name="selfArray">Self array.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> selfArray, Action<T> action)
        {
            if (action == null) throw new ArgumentException();
            foreach (var item in selfArray)
            {
                action(item);
            }

            return selfArray;
        }

        /// <summary>
        /// 随机取一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="selfArray">self array</param>
        /// <returns></returns>
        public static T GetRandom<T>(this IEnumerable<T> selfArray)
        {
            if (selfArray == null) throw new ArgumentException();
            var index = Random.Range(0, selfArray.Count());
            int moveIndex = 0;
            using (var enumerator = selfArray.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    if (index == moveIndex)
                    {
                        return current;
                    }
                    moveIndex++;

                }
            }
          
            return default(T);
        }


        #endregion

        #region List Extension

        /// <summary>
        /// 倒序遍历
        /// <code>
        /// var testList = new List<int> { 1, 2, 3 };
        /// testList.ForEachReverse(number => number.ToString()); // 3, 2, 1
        /// </code>
        /// </summary>
        /// <returns>返回自己</returns>
        /// <param name="selfList">Self list.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> ForEachReverse<T>(this List<T> selfList, Action<T> action)
        {
            if (action == null) throw new ArgumentException();

            for (var i = selfList.Count - 1; i >= 0; --i)
                action(selfList[i]);

            return selfList;
        }

        /// <summary>
        /// 倒序遍历（可获得索引)
        /// <code>
        /// var testList = new List<int> { 1, 2, 3 };
        /// testList.ForEachReverse((number,index)=> number.ToString()); // 3, 2, 1
        /// </code>
        /// </summary>
        /// <returns>The each reverse.</returns>
        /// <param name="selfList">Self list.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> ForEachReverse<T>(this List<T> selfList, Action<T, int> action)
        {
            if (action == null) throw new ArgumentException();

            for (var i = selfList.Count - 1; i >= 0; --i)
                action(selfList[i], i);

            return selfList;
        }

        /// <summary>
        /// 遍历列表(可获得索引）
        /// <code>
        /// var testList = new List<int> {1, 2, 3 };
        /// testList.Foreach((number,index)=>number.ToString()); // 1, 2, 3,
        /// </code>
        /// </summary>
        /// <typeparam name="T">列表类型</typeparam>
        /// <param name="list">目标表</param>
        /// <param name="action">行为</param>
        public static void ForEach<T>(this List<T> list, Action<int, T> action)
        {
            for (var i = 0; i < list.Count; i++)
            {
                action(i, list[i]);
            }
        }

        /// <summary>
        /// 随机取一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="selfArray">self array</param>
        /// <returns></returns>
        public static T GetRandom<T>(this List<T> list)
        {
            if (list == null) throw new ArgumentException();
            var index = Random.Range(0, list.Count);
            return list[index];
        }


        /// <summary>
        /// 返回List中随机Count个值 如果个数少于链表则按照链表个数输出
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="_list">List</param>
        /// <param name="count">个数</param>
        /// <returns></returns>
        public static List<T> GetRandomCountList<T>(this List<T> list, int count)
        {
            if (count >= list.Count)
                count = list.Count;

            List<T> returnList = new List<T>();
            List<int> indexList = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                indexList.Add(i);
            }

            for (int i = 0; i < count; i++)
            {
                int index = indexList.GetRandom();
                returnList.Add(list[index]);
                indexList.Remove(index);
            }
            return returnList;

        }


        /// <summary>
        /// 根据权值来获取索引
        /// </summary>
        /// <param name="powers"></param>
        /// <returns></returns>
        public static int GetRandomWithPower(this List<int> powers)
        {
            var sum = 0;
            foreach (var power in powers)
            {
                sum += power;
            }

            var randomNum = UnityEngine.Random.Range(0, sum);
            var currentSum = 0;
            for (var i = 0; i < powers.Count; i++)
            {
                var nextSum = currentSum + powers[i];
                if (randomNum >= currentSum && randomNum <= nextSum)
                {
                    return i;
                }

                currentSum = nextSum;
            }

            Debug.LogError("权值范围计算错误！");
            return -1;
        }

       

        #endregion

        #region Dictionary Extension

        /// <summary>
        /// 合并字典
        /// <code>
        /// // 示例
        /// var dictionary1 = new Dictionary<string, string> { { "1", "2" } };
        /// var dictionary2 = new Dictionary<string, string> { { "3", "4" } };
        /// var dictionary3 = dictionary1.Merge(dictionary2);
        /// dictionary3.ForEach(pair => Debug.Log("{0}:{1}", pair.Key, pair.Value));
        /// </code>
        /// </summary>
        /// <returns>The merge.</returns>
        /// <param name="dictionary">Dictionary.</param>
        /// <param name="dictionaries">Dictionaries.</param>
        /// <typeparam name="TKey">The 1st type parameter.</typeparam>
        /// <typeparam name="TValue">The 2nd type parameter.</typeparam>
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
            params Dictionary<TKey, TValue>[] dictionaries)
        {
            return dictionaries.Aggregate(dictionary,
                (current, dict) => current.Union(dict).ToDictionary(kv => kv.Key, kv => kv.Value));
        }

        /// <summary>
        /// 遍历字典
        /// <code>
        /// var dict = new Dictionary<string,string> {{"name","liangxie},{"age","18"}};
        /// dict.ForEach((key,value)=> Debug.Log("{0}:{1}",key,value);//  name:liangxie    age:18
        /// </code>
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dict"></param>
        /// <param name="action"></param>
        public static void ForEach<K, V>(this Dictionary<K, V> dict, Action<K, V> action)
        {
            var dictE = dict.GetEnumerator();

            while (dictE.MoveNext())
            {
                var current = dictE.Current;
                action(current.Key, current.Value);
            }

            dictE.Dispose();
        }

        /// <summary>
        /// 字典添加新的词典
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dict"></param>
        /// <param name="addInDict"></param>
        /// <param name="isOverride"></param>
        public static void AddRange<K, V>(this Dictionary<K, V> dict, Dictionary<K, V> addInDict,
            bool isOverride = false)
        {
            var dictE = addInDict.GetEnumerator();

            while (dictE.MoveNext())
            {
                var current = dictE.Current;
                if (dict.ContainsKey(current.Key))
                {
                    if (isOverride)
                        dict[current.Key] = current.Value;
                    continue;
                }

                dict.Add(current.Key, current.Value);
            }

            dictE.Dispose();
        }


        /// <summary>
        /// 根据权值获取值，Key为值，Value为权值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="powersDict"></param>
        /// <returns></returns>
        public static T GetRandomWithPower<T>(this Dictionary<T, int> powersDict)
        {
            var keys = new List<T>();
            var values = new List<int>();

            foreach (var key in powersDict.Keys)
            {
                keys.Add(key);
                values.Add(powersDict[key]);
            }

            var finalKeyIndex = values.GetRandomWithPower();
            return keys[finalKeyIndex];
        }
        #endregion
    }
}
