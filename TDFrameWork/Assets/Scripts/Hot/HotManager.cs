using UnityEngine;
namespace Game
{
    public class HotManager
    {
        public int Debug(int num)
        {
            GameObject obj = new GameObject();
            obj.name = "new obj";
            UnityEngine.Debug.Log("Start Hot Fix Debug num:" + num);
            return num * 3;
        }
    }

}
