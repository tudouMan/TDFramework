
using UnityEngine;

namespace TDFramework.Localization
{
    public class LocalizationImage : MonoBehaviour, INotification
    {
        private UnityEngine.UI.Image m_SelftImg;
        [SerializeField] private short m_LocalKey;

        void Awake()
        {
            if (m_SelftImg == null)
                m_SelftImg = GetComponentInChildren<UnityEngine.UI.Image>();

            Refresh();

            GameEntry.Localization.RefreshHandle += Refresh;
        }

        public void Refresh()
        {
            m_SelftImg.sprite = GameEntry.Localization.GetSpriteByKey(m_LocalKey);
        }

        private void OnDestroy()
        {
            GameEntry.Localization.RefreshHandle -= Refresh;
        }
    }
}
