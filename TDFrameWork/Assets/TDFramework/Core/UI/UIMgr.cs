using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TDFramework.UI
{
    public static class UIMgr
    {
        public static Camera UICamera { get => UIManager.Instance.UICamera; }
        public static Canvas UICanvas { get => UIManager.Instance.UICanvas; }

        public static IPanel GetPanel(string panelName)
        {
            return UIManager.Instance.GetPanel(panelName);
        }

        public static IPanel GetPanel<T>() where T : IPanel
        {
            return UIManager.Instance.GetPanel<T>();
        }


        public static void OpenPanel(string panelName, UILevel uilevel, IUIData uidata = null)
        {
            UIManager.Instance.OpenPanel(panelName, uilevel, uidata);
        }

        public static void OpenPanel<T>(UILevel uilevel, IUIData uidata = null)
        {
            UIManager.Instance.OpenPanel<T>(uilevel, uidata);
        }

        public static void ClosePanel(string panelName)
        {
            UIManager.Instance.ClosePanel(panelName);
        }

        public static void ClosePanel<T>()
        {
            UIManager.Instance.ClosePanel<T>();
        }

        public static void ShowPanel(string panelName)
        {
            UIManager.Instance.ShowPanel(panelName);
        }

        public static void ShowPanel<T>()
        {
            UIManager.Instance.ShowPanel<T>();
        }


        public static void HidePanel(string panelName)
        {
            UIManager.Instance.HidePanel(panelName);
        }

        public static void HidePanel<T>()
        {
            UIManager.Instance.HidePanel<T>();
        }

        public static void PushPanel(IPanel panel)
        {
            UIManager.Instance.PushPanel(panel);
        }

        public static void PushPanel<T>() where T : IPanel
        {
            UIManager.Instance.PushPanel<T>();
        }


        public static void BackPanel(string panelName=null)
        {
            UIManager.Instance.BackPanel(panelName);
        }
    }
}
