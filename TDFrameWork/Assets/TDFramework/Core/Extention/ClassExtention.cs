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
#endif


namespace TDFramework.Extention
{

    /// <summary>
    /// 通用的扩展，类的扩展
    /// </summary>
    public static class ClassExtention
    {

        /// <summary>
        /// 功能：判断是否为空
        /// 示例：
        /// <code>
        /// var simpleObject = new object();
        ///
        /// if (simpleObject.IsNull()) // 等价于 simpleObject == null
        /// {
        ///     // do sth
        /// }
        /// </code>
        /// </summary>
        /// <param name="selfObj">判断对象(this)</param>
        /// <typeparam name="T">对象的类型（可不填）</typeparam>
        /// <returns>是否为空</returns>
        public static bool IsNull<T>(this T selfObj) where T : class
        {
            return null == selfObj;
        }

        /// <summary>
        /// 功能：判断不是为空
        /// 示例：
        /// <code>
        /// var simpleObject = new object();
        ///
        /// if (simpleObject.IsNotNull()) // 等价于 simpleObject != null
        /// {
        ///    // do sth
        /// }
        /// </code>
        /// </summary>
        /// <param name="selfObj">判断对象（this)</param>
        /// <typeparam name="T">对象的类型（可不填）</typeparam>
        /// <returns>是否不为空</returns>
        public static bool IsNotNull<T>(this T selfObj) where T : class
        {
            return null != selfObj;
        }

        /// <summary>
        /// 功能返回类型名 示例: var tempClase=new TempClass(); tempClass.TypeName();
        /// </summary>
        /// <typeparam name="T">对象类型可不填</typeparam>
        /// <param name="selfObj">对象</param>
        /// <returns></returns>
        public static string TypeName<T>(this T selfObj) where T : class
        {
            if (IsNotNull<T>(selfObj))
                return typeof(T).ToString();
            return null;
        }

    }


    /// <summary>
    /// 泛型工具
    /// 实例：
    /// <code>
    /// 示例：
    /// var typeName = GenericExtention.GetTypeName<string>();
    /// typeName.LogInfo(); // string
    /// </code>
    /// </summary>
    public static class GenericUtil
    {
        /// <summary>
        /// 获取泛型名字
        /// <code>
        /// var typeName = GenericExtention.GetTypeName<string>();
        /// typeName.LogInfo(); // string
        /// </code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTypeName<T>()
        {
            return typeof(T).ToString();
        }
    }

}
