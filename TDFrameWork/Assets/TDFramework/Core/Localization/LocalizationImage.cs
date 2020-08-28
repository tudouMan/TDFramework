
using UnityEngine;

namespace TDFramework.Localization
{
    public class LocalizationImage : MonoBehaviour, INotification
    {
        private UnityEngine.UI.Image mSelftImg;
        [SerializeField] private short mLocalKey;

        void Awake()
        {
            if (mSelftImg == null)
                mSelftImg = GetComponentInChildren<UnityEngine.UI.Image>();

            Refresh();

            LocalizationMgr.Instance.RefreshHandle += Refresh;
        }

        public void Refresh()
        {
            mSelftImg.sprite = LocalizationMgr.Instance.GetSpriteByKey(mLocalKey);
        }

        private void OnDestroy()
        {
            LocalizationMgr.Instance.RefreshHandle -= Refresh;
        }
    }
}
