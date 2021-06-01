
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

            GameEntry.Localization.RefreshHandle += Refresh;
        }

        public void Refresh()
        {
            mSelftImg.sprite = GameEntry.Localization.GetSpriteByKey(mLocalKey);
        }

        private void OnDestroy()
        {
            GameEntry.Localization.RefreshHandle -= Refresh;
        }
    }
}
