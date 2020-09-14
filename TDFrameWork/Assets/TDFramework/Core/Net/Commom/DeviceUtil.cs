using UnityEngine;

public class DeviceUtil  {

    /// <summary>
    /// 客户端设备ID
    /// </summary>
    public static string DeviceIdentifier
    {
        get
        {
            return SystemInfo.deviceUniqueIdentifier; //一个唯一的设备标识符。这是保证为每一台设备是唯一的
        }
    }

    /// <summary>
    /// 设备型号
    /// </summary>
    public static string DeviceModel
    {
        get
        {

#if UNITY_IPHONE && !UNITY_EDITOR
            return Device.generation.ToString();
#else
            return SystemInfo.deviceModel;
#endif
        }
    }

}
