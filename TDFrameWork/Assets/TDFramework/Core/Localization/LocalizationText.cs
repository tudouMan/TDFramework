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
        private Text mSelfText;
        [SerializeField] private short mLocalKey;

        void Awake()
        {
            if (mSelfText == null)
                mSelfText = GetComponentInChildren<Text>();

            Refresh();

            LocalizationMgr.Instance.RefreshHandle += Refresh;
        }

        public void Refresh()
        {
            mSelfText.text= LocalizationMgr.Instance.GetTextByKey(mLocalKey);
        }

        private void OnDestroy()
        {
            LocalizationMgr.Instance.RefreshHandle -= Refresh;
        }
    }
}
