using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using LitJson;
using TDFramework;


[MonoSingletonPath("[Net]/[Http]")]
public class NetWorkHttp : MonoSingleton<NetWorkHttp>
{

    #region 属性
    /// <summary>
    /// Web请求回调
    /// </summary>
    private Action<CallBackArgs> m_CallBack;

    /// <summary>
    /// Web回调数据
    /// </summary>
    private CallBackArgs m_CallBackArgs;

    /// <summary>
    /// 是否繁忙
    /// </summary>
    private bool m_IsBusy = false;

    /// <summary>
    /// 属性 是否繁忙
    /// </summary>
    public bool IsBusy
    {
        get { return m_IsBusy; }
    }
    #endregion

    public override void OnSingletonInit()
    {
        base.OnSingletonInit();
        m_CallBackArgs = new CallBackArgs();
    }


    #region 发送Web数据
    /// <summary>
    /// 发送Web数据
    /// </summary>
    /// <param name="url">目标地址</param>
    /// <param name="callBack">回调信息</param>
    /// <param name="isPost"></param>
    /// <param name="json"></param>
    public void SentData(string url, Action<CallBackArgs> callBack, bool isPost = false, Dictionary<string, object> dic = null)
    {
        if (m_IsBusy) return;

        m_IsBusy = true;
        m_CallBack = callBack;

        if (!isPost)
        {
            GetUrl(url);
        }
        else
        {
            //Web加密
            if (dic != null)
            {
                //客户端ID
                dic["DeviceIdentifier"] = DeviceUtil.DeviceIdentifier;
                //设备型号
                dic["DeviceModel"] = DeviceUtil.DeviceModel;
                //签名
                //long t = GlobeInit.Instance.CurrServerTime;
                //dic["sign"] = MFEncryptUtil.Md5(string.Format("{0}:{1}",t, DeviceUtil.DeviceIdentifier));
                //时间戳
                //dic["t"] = t;
            }
            PostUrl(url, dic == null ? "" : JsonMapper.ToJson(dic));
        }
    }
    #endregion

    #region get请求  获取
    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="url"></param>
    private void GetUrl(string url)
    {
        WWW data = new WWW(url);  //GET方式请求的内容会附在url的后面一起做为URL向服务器发送请求（请求的内容使用&符号隔开）
        StartCoroutine(Request(data));
    }


    #endregion

    #region 请求服务器
    /// <summary>
    /// 请求服务器
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private IEnumerator Request(WWW data)
    {
        yield return data;

        m_IsBusy = false;

        if (string.IsNullOrEmpty(data.error))
        {
            if (data.text == "null")
            {
                m_CallBackArgs.HasError = true;
                m_CallBackArgs.ErrorInfo = "查询不到数据";
                m_CallBack(m_CallBackArgs);
            }
            else
            {
                if (m_CallBack != null)
                {
                    m_CallBackArgs.HasError = false;
                    m_CallBackArgs.Value = data.text;
                    m_CallBackArgs.ErrorInfo = string.Empty;
                    m_CallBack(m_CallBackArgs);//执行当前委托
                }
            }
        }
        else
        {
            if (m_CallBack != null)
            {
                m_CallBackArgs.HasError = true;
                m_CallBackArgs.ErrorInfo = data.error;
                m_CallBack(m_CallBackArgs);
            }

        }
    }
    #endregion

    #region post请求 发送
    private void PostUrl(string url, string json)
    {
        //定义一个表单
        WWWForm form = new WWWForm();

        //给表单添加值
        form.AddField("", json);

        WWW data = new WWW(url, form);

        StartCoroutine(Request(data));
    }
    #endregion

    #region 回调数据
    /// <summary>
    /// Web请求回调数据
    /// </summary>
    public class CallBackArgs : EventArgs
    {
        /// <summary>
        /// 是否有错
        /// </summary>
        public bool HasError;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorInfo;
        /// <summary>
        /// 返回值
        /// </summary>
        public string Value;
    }
    #endregion
}
