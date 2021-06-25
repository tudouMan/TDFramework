using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace TDFramework.Localization
{
   public class LocalizationTextMeshProText:MonoBehaviour, INotification
    {
        private TextMeshProUGUI m_SelfText;
        [SerializeField] private short m_LocalKey;

        void Awake()
        {
            if (m_SelfText == null)
                m_SelfText = GetComponentInChildren<TextMeshProUGUI>();

            Refresh();
            GameEntry.Localization.RefreshHandle+= Refresh;
            
        }

        public void Refresh()
        {
           
            m_SelfText.text = GameEntry.Localization.GetTextByKey(m_LocalKey);
        }

        private void OnDestroy()
        {
            GameEntry.Localization.RefreshHandle -= Refresh;
        }
    }
}
