using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;

namespace SilverCat {
    public enum UILayer {
        BaseLayer,      // 基础layer，一般放scene加载后常驻的UI
        WindowLayer,    // 窗口layer，一般放触发打开的UI
        ModalLayer,     // 模态layer，一般模态UI，即打开后独占需要操作继续的UI
        TopLayer,       // 顶层layer，一般放操作反馈，通知等无法交互的消息的UI
        SystemLayer,    // 系统layer, 一般放系统信息，调试信息等，release版本会隐藏
    }

    public static class UIManager {

        private static readonly Dictionary<UILayer, RectTransform> _layers = new Dictionary<UILayer, RectTransform>();

        private static readonly Dictionary<Type, UIView> _openedUIDict = new Dictionary<Type, UIView>();


        private static GameObject _uiRoot;
        public static GameObject UIRoot {
            get { return _uiRoot; }
        }

        private static Camera _uiCamera = null;
        public static Camera UICamera {
            get { return _uiCamera; }
        }

        private static EventSystem _eventSystem;
        public static EventSystem EventSystem {
            get { return _eventSystem; }
        }

        public static void Init() {
            if (_uiRoot != null) {
                return;
            }

            _uiRoot = GameObject.Find("GameApp/UIRoot");

            GameObjectFinder finder = GameObjectFinder.CreateFinder(_uiRoot);

            #region Camera
            _uiCamera = finder.GetComponent<Camera>("UICamera");
            if (_uiCamera != null) {
                _uiCamera.clearFlags = CameraClearFlags.Depth;
                _uiCamera.farClipPlane = 10000;
                _uiCamera.transform.localPosition = new Vector3(0f, 0f, -50000);
            }
            #endregion

            #region EventSystem
            _eventSystem = finder.GetComponent<EventSystem>("EventSystem");
            if (_eventSystem != null) {

            }
            #endregion

            #region Layer
            AddUILayer(UILayer.BaseLayer, ref finder, 0f);
            AddUILayer(UILayer.WindowLayer, ref finder, -10000f);
            AddUILayer(UILayer.TopLayer, ref finder, -20000f);
            AddUILayer(UILayer.ModalLayer, ref finder, -30000f);
            AddUILayer(UILayer.SystemLayer, ref finder, -40000f);
            #endregion
        }

        private static void AddUILayer(UILayer layer, ref GameObjectFinder finder, float z) {
            var name = System.Enum.GetName(layer.GetType(), layer);
            var rectTransform = finder.GetComponent<RectTransform>(name);
            if (rectTransform == null) {
                Debug.LogErrorFormat("{0} doesn't exist.", layer);
                return;
            }
            FitScreen(rectTransform);
            _layers[layer] = rectTransform;
            rectTransform.anchoredPosition3D = new Vector3(0f, 0f, z);
        }

        private static RectTransform GetLayerRectTransform(UILayer layer) {
            RectTransform result;
            _layers.TryGetValue(layer, out result);
            if (result == null) {
                result = _uiRoot.GetComponent<RectTransform>();
                Debug.LogFormat("{0} doesn't exist.", layer);
            }

            return result;
        }

        /// <summary>
        /// Create the UIView if not been created and show.
        /// </summary>
        public static T Open<T>(UILayer layer = UILayer.BaseLayer, object data = null) where T : UIView, new() {
            return Open(typeof(T), layer, data) as T;
        }

        /// <summary>
        /// Create the UIView if not been created and show.
        /// </summary>
        public static UIView Open(Type t, UILayer layer = UILayer.BaseLayer, object data = null, bool beForced = true) {
            if (_uiRoot == null) {
                return null;
            }

            var layerRectTransform = GetLayerRectTransform(layer);

            UIView uiView;
            _openedUIDict.TryGetValue(t, out uiView);

            if (uiView == null) {
                uiView = ViewManager.CreateView(t, layerRectTransform, data) as UIView;
                // TODO:
                //if (uiView is IRender) {
                //    Debug.LogException(new Exception("IRender 不能通过ViewManager创建"));
                //    return null;
                //}
            }
            else if (beForced) {
                uiView.Init(layerRectTransform, data);
            }

            uiView.UILayer = layer;

            _openedUIDict[uiView.GetType()] = uiView;

            return uiView;
        }

        /// <summary>Hide the UIView and destroy it if exist.</summary>
        public static bool Close<T>() where T : UIView {
            return Close(typeof(T));
        }

        /// <summary>Hide the UIView and destroy it if exist.</summary>
        public static bool Close(UIView uiView) {
            return Close(uiView.GetType());
        }

        public static bool Close(Type t) {
            UIView uiView;
            _openedUIDict.TryGetValue(t, out uiView);

            if (uiView == null) {
                return false;
            }

            _openedUIDict.Remove(t);
            uiView.Destroy();
            return true;
        }

        public static void Show(UIView view) {
            // TODO:
            //if (view is IRender) {
            //    return;
            //}

            _openedUIDict[view.GetType()] = view;

        }

        public static void FitScreen(RectTransform rtf) {
            if (rtf.sizeDelta != Vector2.zero)
                rtf.sizeDelta = Vector2.zero;
            if (rtf.anchorMax != Vector2.one)
                rtf.anchorMax = Vector2.one;
            if (rtf.anchorMin != Vector2.zero)
                rtf.anchorMin = Vector2.zero;
            if (rtf.anchoredPosition != Vector2.zero)
                rtf.anchoredPosition = Vector2.zero;

            rtf.pivot = Vector2.one / 2f;
            if (rtf.localScale != Vector3.one)
                rtf.localScale = Vector3.one;
        }
    }
}