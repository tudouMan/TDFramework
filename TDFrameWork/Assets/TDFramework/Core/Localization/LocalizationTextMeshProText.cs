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
        private TextMeshProUGUI mSelfText;
        [SerializeField] private short mLocalKey;

        void Awake()
        {
            if (mSelfText == null)
                mSelfText = GetComponentInChildren<TextMeshProUGUI>();

            Refresh();
            GameEntry.Localization.RefreshHandle+= Refresh;
            
        }

        public void Refresh()
        {
           
            mSelfText.text = GameEntry.Localization.GetTextByKey(mLocalKey);
        }

        private void OnDestroy()
        {
            GameEntry.Localization.RefreshHandle -= Refresh;
        }
    }
}
