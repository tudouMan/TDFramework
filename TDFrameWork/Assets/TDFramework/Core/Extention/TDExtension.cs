using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Text;
using UnityEngine.EventSystems;

#if UNITY_5_6_OR_NEWER
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;
#endif


namespace TDFramework.Extention
{
#if UNITY_5_6_OR_NEWER
    public static class BehaviourExtension
    {
        public static T Enable<T>(this T selfBehaviour) where T : Behaviour
        {
            selfBehaviour.enabled = true;
            return selfBehaviour;
        }

        public static T Disable<T>(this T selfBehaviour) where T : Behaviour
        {
            selfBehaviour.enabled = false;
            return selfBehaviour;
        }

       

    }

    public static class CameraExtension
    {
        public static Texture2D CaptureCamera(this Camera camera, Rect rect)
        {
            var renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
            camera.targetTexture = renderTexture;
            camera.Render();

            RenderTexture.active = renderTexture;

            var screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();

            camera.targetTexture = null;
            RenderTexture.active = null;
            Object.Destroy(renderTexture);

            return screenShot;
        }
    }

    public static class ColorExtension
    {
        /// <summary>
        /// #C5563CFF -> 197.0f / 255,86.0f / 255,60.0f / 255
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static Color HtmlStringToColor(this string htmlString)
        {
            Color retColor;
            var parseSucceed = ColorUtility.TryParseHtmlString(htmlString, out retColor);
            return parseSucceed ? retColor : Color.black;
        }

        /// <summary>
        /// 白色
        /// </summary>
        public static Color White = Color.white;

        /// <summary>
        /// 灰色
        /// </summary>
        public static Color Grad = new Color(125f / 255, 125f / 255, 125f / 255);
    }


    public static class ButtonExtention
    {
        /// <summary>
        /// 拓展UI Event  PointEnter
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="callback"></param>
        public static void PointEnter(this Button btn, Action callback)
        {
            if (btn == null) return;
            EventTriggerListener.Get(btn.gameObject).onEnter = (GameObject go) =>
            {
                if (go == btn.gameObject)
                    callback?.Invoke();
            };
        }

        /// <summary>
        /// 拓展UI Event PointExit
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="callback"></param>
        public static void PointExit(this Button btn, Action callback)
        {
            if (btn == null) return;
            EventTriggerListener.Get(btn.gameObject).onExit = (GameObject go) =>
            {
                if (go == btn.gameObject)
                        callback?.Invoke();

            };
        }

        /// <summary>
        /// 扩展UI Event Select
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="callback"></param>
        public static void OnSelect(this Button btn, Action callback)
        {
            if (btn == null || btn.enabled == false) return;
            EventTriggerListener.Get(btn.gameObject).onSelect = (GameObject go) =>
            {
                if (go == btn.gameObject)
                        callback?.Invoke();
            };
        }

        /// <summary>
        /// 扩展UI Event DeSelect
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="callback"></param>
        public static void OnDeSelect(this Button btn, Action callback)
        {
            if (btn == null || btn.enabled == false) return;
            EventTriggerListener.Get(btn.gameObject).onDeSelect = (GameObject go) =>
            {
                if (go == btn.gameObject)
                    callback?.Invoke();

            };
        }

        /// <summary>
        /// 扩展Button Click事件
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="callBack"></param>
        public static void OnClick(this Button btn,Action callback)
        {
            if (btn == null || btn.enabled == false) return;
            EventTriggerListener.Get(btn.gameObject).onClick = (GameObject go) =>
            {
                if (go == btn.gameObject)
                {
                    if (go == btn.gameObject)
                        callback?.Invoke();
                }

            };
        }


       
    }


    /// <summary>
    /// GameObject's Util/Static This Extension
    /// </summary>
    public static class GameObjectExtension
    {

        #region  Show

        public static GameObject Show(this GameObject selfObj)
        {
            selfObj.SetActive(true);
            return selfObj;
        }

        public static T Show<T>(this T selfComponent) where T : Component
        {
            selfComponent.gameObject.Show();
            return selfComponent;
        }

        #endregion

        #region  Hide

        public static GameObject Hide(this GameObject selfObj)
        {
            selfObj.SetActive(false);
            return selfObj;
        }

        public static T Hide<T>(this T selfComponent) where T : Component
        {
            selfComponent.gameObject.Hide();
            return selfComponent;
        }

        #endregion


        #region  DestroyGameObj

        public static void DestroyGameObj<T>(this T selfBehaviour) where T : Component
        {
            selfBehaviour.gameObject.DestroySelf();
        }

        #endregion

        #region  DestroyGameObjGracefully

        public static void DestroyGameObjGracefully<T>(this T selfBehaviour) where T : Component
        {
            if (selfBehaviour && selfBehaviour.gameObject)
            {
                selfBehaviour.gameObject.DestroySelfGracefully();
            }
        }

        #endregion

        #region  DestroyGameObjGracefully T

        public static T DestroyGameObjAfterDelay<T>(this T selfBehaviour, float delay) where T : Component
        {
            selfBehaviour.gameObject.DestroySelfAfterDelay(delay);
            return selfBehaviour;
        }

        public static T DestroyGameObjAfterDelayGracefully<T>(this T selfBehaviour, float delay) where T : Component
        {
            if (selfBehaviour && selfBehaviour.gameObject)
            {
                selfBehaviour.gameObject.DestroySelfAfterDelay(delay);
            }

            return selfBehaviour;
        }

        #endregion

        #region  Layer

        public static GameObject Layer(this GameObject selfObj, int layer)
        {
            selfObj.layer = layer;
            return selfObj;
        }

        public static T Layer<T>(this T selfComponent, int layer) where T : Component
        {
            selfComponent.gameObject.layer = layer;
            return selfComponent;
        }

        public static GameObject Layer(this GameObject selfObj, string layerName)
        {
            selfObj.layer = LayerMask.NameToLayer(layerName);
            return selfObj;
        }

        public static T Layer<T>(this T selfComponent, string layerName) where T : Component
        {
            selfComponent.gameObject.layer = LayerMask.NameToLayer(layerName);
            return selfComponent;
        }

        #endregion

        #region  Component

        public static T GetOrAddComponent<T>(this GameObject selfComponent) where T : Component
        {
            var comp = selfComponent.gameObject.GetComponent<T>();
            return comp ? comp : selfComponent.gameObject.AddComponent<T>();
        }

        public static Component GetOrAddComponent(this GameObject selfComponent, Type type)
        {
            var comp = selfComponent.gameObject.GetComponent(type);
            return comp ? comp : selfComponent.gameObject.AddComponent(type);
        }

        #endregion
    }

    public static class GraphicExtension
    {
       
        public static T ColorAlpha<T>(this T selfGraphic, float alpha) where T : Graphic
        {
            var color = selfGraphic.color;
            color.a = alpha;
            selfGraphic.color = color;
            return selfGraphic;
        }
    }


    public static class ImageExtension
    {
        public static Image FillAmount(this Image selfImage, float fillamount)
        {
            selfImage.fillAmount = fillamount;
            return selfImage;
        }


        public static Image ImageColor(this Image selfImage,Color color)
        {
            selfImage.color = color;
            return selfImage;
        }

        public static Image ImageColor(this Image selfImage,float r=255,float g=255,float b=255,float a = 255)
        {
            selfImage.color = new Color(r,g,b,a);
            return selfImage;
        }

        

    }

    public static class LightmapExtension
    {
        public static void SetAmbientLightHTMLStringColor(string htmlStringColor)
        {
            RenderSettings.ambientLight = htmlStringColor.HtmlStringToColor();
        }

    }

    public static class ObjectExtension
    {

        #region  Instantiate

        public static T Instantiate<T>(this T selfObj) where T : Object
        {
            return Object.Instantiate(selfObj);
        }

        #endregion

        #region Instantiate T

        public static T Name<T>(this T selfObj, string name) where T : Object
        {
            selfObj.name = name;
            return selfObj;
        }

        #endregion

        #region  Destroy Self

        public static void DestroySelf<T>(this T selfObj) where T : Object
        {
            Object.Destroy(selfObj);
        }

        public static T DestroySelfGracefully<T>(this T selfObj) where T : Object
        {
            if (selfObj)
            {
                Object.Destroy(selfObj);
            }

            return selfObj;
        }

        #endregion

        #region  Destroy Self AfterDelay 

        public static T DestroySelfAfterDelay<T>(this T selfObj, float afterDelay) where T : Object
        {
            Object.Destroy(selfObj, afterDelay);
            return selfObj;
        }

        public static T DestroySelfAfterDelayGracefully<T>(this T selfObj, float delay) where T : Object
        {
            if (selfObj)
            {
                Object.Destroy(selfObj, delay);
            }

            return selfObj;
        }

        #endregion

 
        #region  DontDestroyOnLoad

        public static T DontDestroyOnLoad<T>(this T selfObj) where T : Object
        {
            Object.DontDestroyOnLoad(selfObj);
            return selfObj;
        }

        #endregion


        /// <summary>
        /// 转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selfObj"></param>
        /// <returns></returns>
        public static T As<T>(this object selfObj) where T : class
        {
            return selfObj as T;
        }
    }

    public static class RectTransformExtension
    {
        public static Vector2 GetPosInRootTrans(this RectTransform selfRectTransform, Transform rootTrans)
        {
            return RectTransformUtility.CalculateRelativeRectTransformBounds(rootTrans, selfRectTransform).center;
        }

        public static RectTransform AnchorPosX(this RectTransform selfRectTrans, float anchorPosX)
        {
            var anchorPos = selfRectTrans.anchoredPosition;
            anchorPos.x = anchorPosX;
            selfRectTrans.anchoredPosition = anchorPos;
            return selfRectTrans;
        }

        public static RectTransform AnchorPosY(this RectTransform selfRectTrans, float anchorPosY)
        {
            var anchorPos = selfRectTrans.anchoredPosition;
            anchorPos.y = anchorPosY;
            selfRectTrans.anchoredPosition = anchorPos;
            return selfRectTrans;
        }

        public static RectTransform SetSizeWidth(this RectTransform selfRectTrans, float sizeWidth)
        {
            var sizeDelta = selfRectTrans.sizeDelta;
            sizeDelta.x = sizeWidth;
            selfRectTrans.sizeDelta = sizeDelta;
            return selfRectTrans;
        }

        public static RectTransform SetSizeHeight(this RectTransform selfRectTrans, float sizeHeight)
        {
            var sizeDelta = selfRectTrans.sizeDelta;
            sizeDelta.y = sizeHeight;
            selfRectTrans.sizeDelta = sizeDelta;
            return selfRectTrans;
        }

        public static Vector2 GetWorldSize(this RectTransform selfRectTrans)
        {
            return RectTransformUtility.CalculateRelativeRectTransformBounds(selfRectTrans).size;
        }

    }

    public static class SelectableExtension
    {
        public static T EnableInteract<T>(this T selfSelectable) where T : Selectable
        {
            selfSelectable.interactable = true;
            return selfSelectable;
        }

        public static T DisableInteract<T>(this T selfSelectable) where T : Selectable
        {
            selfSelectable.interactable = false;
            return selfSelectable;
        }

        public static T CancalAllTransitions<T>(this T selfSelectable) where T : Selectable
        {
            selfSelectable.transition = Selectable.Transition.None;
            return selfSelectable;
        }
    }

    public static class ToggleExtension
    {
        public static void RegOnValueChangedEvent(this Toggle selfToggle, UnityAction<bool> onValueChangedEvent)
        {
            selfToggle.onValueChanged.AddListener(onValueChangedEvent);
        }
    }


    /// <summary>
    /// Transform's Extension
    /// </summary>
    public static class TransformExtension
    {
      
        /// <summary>
        /// 缓存的一些变量,免得每次声明
        /// </summary>
        private static Vector3 mLocalPos;

        private static Vector3 mScale;
        private static Vector3 mPos;

        #region  Parent

        public static T Parent<T>(this T selfComponent, Component parentComponent) where T : Component
        {
            selfComponent.transform.SetParent(parentComponent == null ? null : parentComponent.transform);
            return selfComponent;
        }


        public static Transform Parent(this GameObject selfObj, Component parentComponent)
        {
            selfObj.transform.SetParent(parentComponent == null ? null : parentComponent.transform);
            return selfObj.transform;
        }


        #endregion

        #region  LocalIdentity

        public static T LocalIdentity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.localPosition = Vector3.zero;
            selfComponent.transform.localRotation = Quaternion.identity;
            selfComponent.transform.localScale = Vector3.one;
            return selfComponent;
        }


        #endregion

        #region  LocalPosition

        public static T LocalPosition<T>(this T selfComponent, Vector3 localPos) where T : Component
        {
            selfComponent.transform.localPosition = localPos;
            return selfComponent;
        }

        public static Vector3 GetLocalPosition<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.localPosition;
        }


        public static T LocalPosition<T>(this T selfComponent, float x, float y, float z) where T : Component
        {
            selfComponent.transform.localPosition = new Vector3(x, y, z);
            return selfComponent;
        }

        public static T LocalPosition<T>(this T selfComponent, float x, float y) where T : Component
        {
            mLocalPos = selfComponent.transform.localPosition;
            mLocalPos.x = x;
            mLocalPos.y = y;
            selfComponent.transform.localPosition = mLocalPos;
            return selfComponent;
        }

        public static T LocalPositionX<T>(this T selfComponent, float x) where T : Component
        {
            mLocalPos = selfComponent.transform.localPosition;
            mLocalPos.x = x;
            selfComponent.transform.localPosition = mLocalPos;
            return selfComponent;
        }

        public static T LocalPositionY<T>(this T selfComponent, float y) where T : Component
        {
            mLocalPos = selfComponent.transform.localPosition;
            mLocalPos.y = y;
            selfComponent.transform.localPosition = mLocalPos;
            return selfComponent;
        }

        public static T LocalPositionZ<T>(this T selfComponent, float z) where T : Component
        {
            mLocalPos = selfComponent.transform.localPosition;
            mLocalPos.z = z;
            selfComponent.transform.localPosition = mLocalPos;
            return selfComponent;
        }


        public static T LocalPositionIdentity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.localPosition = Vector3.zero;
            return selfComponent;
        }


        #endregion

        #region  LocalRotation

        public static Quaternion GetLocalRotation<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.localRotation;
        }

        public static T LocalRotation<T>(this T selfComponent, Quaternion localRotation) where T : Component
        {
            selfComponent.transform.localRotation = localRotation;
            return selfComponent;
        }

        public static T LocalRotationIdentity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.localRotation = Quaternion.identity;
            return selfComponent;
        }

        #endregion

        #region  LocalScale

        public static T LocalScale<T>(this T selfComponent, Vector3 scale) where T : Component
        {
            selfComponent.transform.localScale = scale;
            return selfComponent;
        }

        public static Vector3 GetLocalScale<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.localScale;
        }

        public static T LocalScale<T>(this T selfComponent, float xyz) where T : Component
        {
            selfComponent.transform.localScale = Vector3.one * xyz;
            return selfComponent;
        }

        public static T LocalScale<T>(this T selfComponent, float x, float y, float z) where T : Component
        {
            mScale = selfComponent.transform.localScale;
            mScale.x = x;
            mScale.y = y;
            mScale.z = z;
            selfComponent.transform.localScale = mScale;
            return selfComponent;
        }

        public static T LocalScale<T>(this T selfComponent, float x, float y) where T : Component
        {
            mScale = selfComponent.transform.localScale;
            mScale.x = x;
            mScale.y = y;
            selfComponent.transform.localScale = mScale;
            return selfComponent;
        }

        public static T LocalScaleX<T>(this T selfComponent, float x) where T : Component
        {
            mScale = selfComponent.transform.localScale;
            mScale.x = x;
            selfComponent.transform.localScale = mScale;
            return selfComponent;
        }

        public static T LocalScaleY<T>(this T selfComponent, float y) where T : Component
        {
            mScale = selfComponent.transform.localScale;
            mScale.y = y;
            selfComponent.transform.localScale = mScale;
            return selfComponent;
        }

        public static T LocalScaleZ<T>(this T selfComponent, float z) where T : Component
        {
            mScale = selfComponent.transform.localScale;
            mScale.z = z;
            selfComponent.transform.localScale = mScale;
            return selfComponent;
        }

        public static T LocalScaleIdentity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.localScale = Vector3.one;
            return selfComponent;
        }




        #endregion

        #region  Identity

        public static T Identity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.position = Vector3.zero;
            selfComponent.transform.rotation = Quaternion.identity;
            selfComponent.transform.localScale = Vector3.one;
            return selfComponent;
        }

        #endregion

        #region  Position

        public static T Position<T>(this T selfComponent, Vector3 position) where T : Component
        {
            selfComponent.transform.position = position;
            return selfComponent;
        }



        public static Vector3 GetPosition<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.position;
        }

        public static T Position<T>(this T selfComponent, float x, float y, float z) where T : Component
        {
            selfComponent.transform.position = new Vector3(x, y, z);
            return selfComponent;
        }

        public static T Position<T>(this T selfComponent, float x, float y) where T : Component
        {
            mPos = selfComponent.transform.position;
            mPos.x = x;
            mPos.y = y;
            selfComponent.transform.position = mPos;
            return selfComponent;
        }

        public static T PositionIdentity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.position = Vector3.zero;
            return selfComponent;
        }

        public static T PositionX<T>(this T selfComponent, float x) where T : Component
        {
            mPos = selfComponent.transform.position;
            mPos.x = x;
            selfComponent.transform.position = mPos;
            return selfComponent;
        }

        public static T PositionX<T>(this T selfComponent, Func<float, float> xSetter) where T : Component
        {
            mPos = selfComponent.transform.position;
            mPos.x = xSetter(mPos.x);
            selfComponent.transform.position = mPos;
            return selfComponent;
        }

        public static T PositionY<T>(this T selfComponent, float y) where T : Component
        {
            mPos = selfComponent.transform.position;
            mPos.y = y;
            selfComponent.transform.position = mPos;
            return selfComponent;
        }

        public static T PositionY<T>(this T selfComponent, Func<float, float> ySetter) where T : Component
        {
            mPos = selfComponent.transform.position;
            mPos.y = ySetter(mPos.y);
            selfComponent.transform.position = mPos;
            return selfComponent;
        }

        public static T PositionZ<T>(this T selfComponent, float z) where T : Component
        {
            mPos = selfComponent.transform.position;
            mPos.z = z;
            selfComponent.transform.position = mPos;
            return selfComponent;
        }

        public static T PositionZ<T>(this T selfComponent, Func<float, float> zSetter) where T : Component
        {
            mPos = selfComponent.transform.position;
            mPos.z = zSetter(mPos.z);
            selfComponent.transform.position = mPos;
            return selfComponent;
        }


        public static RectTransform RectPostion(this RectTransform selfRect,Vector3 postion)
        {
            selfRect.anchoredPosition3D = postion;
            return selfRect;
        }
        
        public static RectTransform RectPostion(this RectTransform selfRect,Vector2 postion)
        {
            selfRect.anchoredPosition = postion;
            return selfRect;
        }

        public static RectTransform RectPostionX(this RectTransform selfRect, float  x)
        {
            selfRect.anchoredPosition3D = new Vector3(x, selfRect.anchoredPosition3D.y, selfRect.anchoredPosition3D.z);
            return selfRect;
        }

        public static RectTransform RectPostionY(this RectTransform selfRect, float y)
        {
            selfRect.anchoredPosition3D = new Vector3(selfRect.anchoredPosition3D.x, y, selfRect.anchoredPosition3D.z);
            return selfRect;
        }
        public static RectTransform RectPostionZ(this RectTransform selfRect, float z)
        {
            selfRect.anchoredPosition3D = new Vector3(selfRect.anchoredPosition3D.x, selfRect.anchoredPosition3D.y, z);
            return selfRect;
        }

        public static RectTransform RectMinMaxZero(this RectTransform selfRect)
        {
            selfRect.offsetMin = new Vector2(0, 0);
            selfRect.offsetMax = new Vector2(0, 0);
            return selfRect;
        }

        public static GameObject RectMinMaxZero(this GameObject self)
        {
            RectTransform rect = self.GetComponent<RectTransform>() == null ? null : self.GetComponent<RectTransform>();
            return RectMinMaxZero(rect).gameObject;
        }

        public static Transform RectMinMaxZero(this Transform self)
        {
            RectTransform rect = self.GetComponent<RectTransform>() == null ? null : self.GetComponent<RectTransform>();
            return RectMinMaxZero(rect).transform;
        }

        #endregion

        #region  Rotation

        public static T RotationIdentity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.rotation = Quaternion.identity;
            return selfComponent;
        }

        public static T Rotation<T>(this T selfComponent, Quaternion rotation) where T : Component
        {
            selfComponent.transform.rotation = rotation;
            return selfComponent;
        }

        public static Quaternion GetRotation<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.rotation;
        }

        #endregion

        #region  WorldScale/LossyScale/GlobalScale/Scale

        public static Vector3 GetGlobalScale<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.lossyScale;
        }

        public static Vector3 GetScale<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.lossyScale;
        }

        public static Vector3 GetWorldScale<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.lossyScale;
        }

        public static Vector3 GetLossyScale<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.lossyScale;
        }

        #endregion

        #region  Destroy All Child

        public static T DestroyAllChild<T>(this T selfComponent) where T : Component
        {
            var childCount = selfComponent.transform.childCount;

            for (var i = 0; i < childCount; i++)
            {
                selfComponent.transform.GetChild(i).DestroyGameObjGracefully();
            }

            return selfComponent;
        }

        public static GameObject DestroyAllChild(this GameObject selfGameObj)
        {
            var childCount = selfGameObj.transform.childCount;

            for (var i = 0; i < childCount; i++)
            {
                selfGameObj.transform.GetChild(i).DestroyGameObjGracefully();
            }

            return selfGameObj;
        }

        #endregion

        #region  Sibling Index

        public static T AsLastSibling<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.SetAsLastSibling();
            return selfComponent;
        }

        public static T AsFirstSibling<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.SetAsFirstSibling();
            return selfComponent;
        }

        public static T SiblingIndex<T>(this T selfComponent, int index) where T : Component
        {
            selfComponent.transform.SetSiblingIndex(index);
            return selfComponent;
        }

        #endregion


        /// <summary>
        /// 查找物体根据Path
        /// </summary>
        /// <param name="selfTrans"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Transform FindByPath(this Transform selfTrans, string path)
        {
            return selfTrans.Find(path.Replace(".", "/"));
        }

        /// <summary>
        /// 查找物体 邻居层没有则递归
        /// </summary>
        /// <param name="selfTransform"></param>
        /// <param name="uniqueName"></param>
        /// <returns></returns>
        public static Transform SeekTrans(this Transform selfTransform, string uniqueName)
        {
            var childTrans = selfTransform.Find(uniqueName);

            if (null != childTrans)
                return childTrans;

            foreach (Transform trans in selfTransform)
            {
                childTrans = trans.SeekTrans(uniqueName);

                if (null != childTrans)
                    return childTrans;
            }

            return null;
        }

        /// <summary>
        /// Show Child Transform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selfComponent"></param>
        /// <param name="tranformPath"></param>
        /// <returns></returns>
        public static T ShowChildTransByPath<T>(this T selfComponent, string tranformPath) where T : Component
        {
            selfComponent.transform.Find(tranformPath).gameObject.Show();
            return selfComponent;
        }

        /// <summary>
        /// Hide Child Transform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selfComponent"></param>
        /// <param name="tranformPath"></param>
        /// <returns></returns>
        public static T HideChildTransByPath<T>(this T selfComponent, string tranformPath) where T : Component
        {
            selfComponent.transform.Find(tranformPath).Hide();
            return selfComponent;
        }


        /// <summary>
        /// copy transform infomation by self
        /// </summary>
        /// <param name="selfTrans"></param>
        /// <param name="fromTrans"></param>
        public static void CopyDataFromTrans(this Transform selfTrans, Transform fromTrans)
        {
            selfTrans.SetParent(fromTrans.parent);
            selfTrans.localPosition = fromTrans.localPosition;
            selfTrans.localRotation = fromTrans.localRotation;
            selfTrans.localScale = fromTrans.localScale;
        }

        /// <summary>
        /// 递归遍历子物体，并调用函数
        /// </summary>
        /// <param name="tfParent"></param>
        /// <param name="action"></param>
        public static void ActionRecursion(this Transform tfParent, Action<Transform> action)
        {
            action(tfParent);
            foreach (Transform tfChild in tfParent)
            {
                tfChild.ActionRecursion(action);
            }
        }

        /// <summary>
        /// 递归遍历查找指定的名字的子物体
        /// </summary>
        /// <param name="tfParent">当前Transform</param>
        /// <param name="name">目标名</param>
        /// <param name="stringComparison">字符串比较规则</param>
        /// <returns></returns>
        public static Transform FindChildRecursion(this Transform tfParent, string name,
            StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (tfParent.name.Equals(name, stringComparison))
            {
                //Debug.Log("Hit " + tfParent.name);
                return tfParent;
            }

            foreach (Transform tfChild in tfParent)
            {
                Transform tfFinal = null;
                tfFinal = tfChild.FindChildRecursion(name, stringComparison);
                if (tfFinal)
                {
                    return tfFinal;
                }
            }

            return null;
        }

        /// <summary>
        /// 递归遍历查找相应条件的子物体
        /// </summary>
        /// <param name="tfParent">当前Transform</param>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public static Transform FindChildRecursion(this Transform tfParent, Func<Transform, bool> predicate)
        {
            if (predicate(tfParent))
            {
                Debug.Log("Hit " + tfParent.name);
                return tfParent;
            }

            foreach (Transform tfChild in tfParent)
            {
                Transform tfFinal = null;
                tfFinal = tfChild.FindChildRecursion(predicate);
                if (tfFinal)
                {
                    return tfFinal;
                }
            }

            return null;
        }

        public static string GetPath(this Transform transform)
        {
            var sb = new System.Text.StringBuilder();
            var t = transform;
            while (true)
            {
                sb.Insert(0, t.name);
                t = t.parent;
                if (t)
                {
                    sb.Insert(0, "/");
                }
                else
                {
                    return sb.ToString();
                }
            }
        }

        public static void Apply<T>(this Transform selfBehavior, Action<T> ac) where T : Behaviour
        {
            ac?.Invoke(selfBehavior.GetComponent<T>());
        }
    }

    public static class UnityActionExtension
    {
      

        /// <summary>
        /// Call action
        /// </summary>
        /// <param name="selfAction"></param>
        /// <returns> call succeed</returns>
        public static bool InvokeGracefully(this UnityAction selfAction)
        {
            if (null != selfAction)
            {
                selfAction();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Call action
        /// </summary>
        /// <param name="selfAction"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool InvokeGracefully<T>(this UnityAction<T> selfAction, T t)
        {
            if (null != selfAction)
            {
                selfAction(t);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Call action
        /// </summary>
        /// <param name="selfAction"></param>
        /// <returns> call succeed</returns>
        public static bool InvokeGracefully<T, K>(this UnityAction<T, K> selfAction, T t, K k)
        {
            if (null != selfAction)
            {
                selfAction(t, k);
                return true;
            }

            return false;
        }

       

    }
#endif


    #region EventTriggerListener
    public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
    {
        public delegate void VoidDelegate(GameObject go);
        public VoidDelegate onClick;
        public VoidDelegate onDown;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;
        public VoidDelegate onDeSelect;

        static public EventTriggerListener Get(GameObject go)
        {
            EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
            if (listener == null) listener = go.AddComponent<EventTriggerListener>();
            return listener;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null) onClick(gameObject);
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null) onDown(gameObject);
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null) onEnter(gameObject);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null) onExit(gameObject);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null) onUp(gameObject);
        }
        public override void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null) onSelect(gameObject);
        }
        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null) onUpdateSelect(gameObject);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            if (onDeSelect != null) onDeSelect(gameObject);
        }
    }
    #endregion
}

