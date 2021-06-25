using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TDFramework.Localization
{
    public class LocalizationText : MonoBehaviour, INotification
    {
        private Text m_SelfText;
        [SerializeField] private short m_LocalKey;

        void Awake()
        {
            if (m_SelfText == null)
                m_SelfText = GetComponentInChildren<Text>();

            Refresh();

            GameEntry.Localization.RefreshHandle += Refresh;
        }

        public void Refresh()
        {
            m_SelfText.text= GameEntry.Localization.GetTextByKey(m_LocalKey);
        }

        private void OnDestroy()
        {
            GameEntry.Localization.RefreshHandle -= Refresh;
        }
    }
}
