using System;
using TDFramework.UI;
using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{

    public partial class TestView : UIPanel
    {
        public Text Text;
        public TMPro.TextMeshProUGUI TMPText;
        public Image Image;
        public RawImage RawImage;
        public Button Button;
        public Toggle Toggle;
        public Slider Slider;
        public Canvas Canvas;
        public Scrollbar Scrollbar;
        public Dropdown Dropdown;
        public InputField InputField;
        public ScrollRect ScrollView;

        public override void ClearData()
        {
            base.ClearData();
            Text = null;
            TMPText = null;
            Image = null;
            RawImage = null;
            Button = null;
            Toggle = null;
            Slider = null;
            Canvas = null;
            Scrollbar = null;
            Dropdown = null;
            InputField = null;
            ScrollView = null;
        }

    }
}