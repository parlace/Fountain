using System;
using UnityEngine;

namespace SilverCat {

    public enum ViewDisposeType {
        DestroyOnSceneChange,
        kDisposeManual,
        kDisposeManualInScene,
    }

    public abstract partial class ViewBase {

        protected virtual void OnCreate() { }

        protected virtual void OnDestroy() { }

        protected virtual void OnAttachHost() { }

        protected virtual void OnShow(object data) { }


        public virtual void Update() { }

        private bool _destroyed = true;
        public bool Destroyed {
            get { return _destroyed; }
        }

        private Transform _parent;
        public Transform Parent {
            get { return _parent; }
        }

        private Transform _view;
        public Transform View {
            get { return _view; }
        }

        private object _viewData;
        public object ViewData {
            get { return _viewData; }
        }

        private GameObject _hostObject;
        public GameObject HostObject {
            get { return _hostObject; }
        }

        private bool _canDestroyHost = true;

        private bool _createCompleted = false;
        public bool CreateCompleted {
            get { return _createCompleted; }
        }

        private ViewBehaviour _behaviour;
        public ViewBehaviour Behaviour {
            get { return _behaviour; }
        }

        public /*sealed override*/ void Destroy() {
            if (_destroyed) {
                return;
            }

            _destroyed = true;
            _parent = null;
            //updateEnabled = false;
            //enabled = false;

            if (_behaviour != null) {
                _behaviour.enabled = false;
            }

            OnDestroy();

            // TODO: 取消所有UI上的action执行，或等action执行完后真正的destroy
            //if (_rootTransform) {
            //    TimerManager.addCallLaterWithTime(onActionExit(), destroyAfterActionExit, this);
            //}
            CallbackDestroy(this);
        }

        private static void CallbackDestroy(object data) {
            ViewBase view = data as ViewBase;
            if (view == null) {
                return;
            }

            //visible = false;
            if (view.HostObject != null) {
                view.DetachHost();
            }
        }

        public bool Init(Transform parentTrans, object vData) {
            if (!InitViewData(parentTrans, vData)) {
                return false;
            }

            OnCreate();
            return true;
        }

        public bool Init(Transform parentTrans, object vData, GameObject host, bool beClone) {
            if (!InitViewData(parentTrans, vData)) {
                return false;
            }

            AttachHost(host, beClone, true);
            return true;
        }

        private bool InitViewData(Transform parentTrans, object vData) {
            if (parentTrans == null) {
                Debug.LogFormat("{0}, cannot set parent to null, init view data failed.", this);

                _destroyed = true;
                return false;
            }

            if (!_destroyed) {

            }

            _destroyed = false;
            SetParent(parentTrans);
            _viewData = vData;
            return true;
        }

        public void SetParent(Transform parentTrans) {
            if (parentTrans == null) {
                Debug.LogFormat("{0} cannot set parent to null", this);
                return;
            }

            if (_parent == parentTrans) {
                return;
            }

            _parent = parentTrans;
            UpdateParent();
        }

        private void UpdateParent() {
            if (_parent == null) {
                return;
            }

            if (_view == null || _view == _parent) {
                return;
            }

            GameObjectTool.AddChild(_view, _parent);
        }

        protected void AttachSelfAsHost(Action<GameObject> callback = null) {
            var obj = new GameObject(GetType().Name);
            if (callback != null) {
                callback(obj);
            }
            AttachHost(obj, false, true);
        }

        protected void AttachHost(GameObject host, bool beClone = true, bool canDestroyHost = true) {
            if (_destroyed) {
                return;
            }

            _canDestroyHost = canDestroyHost;

            bool isFirstCreated = _hostObject == null;
            if (isFirstCreated) {
                _hostObject = beClone ? UnityEngine.Object.Instantiate(host) : host;
                _view = _hostObject.transform;
                _objFinder = GameObjectFinder.CreateFinder(_hostObject);
            }

            if (_behaviour == null) {
                _behaviour = _hostObject.AddComponent<ViewBehaviour>();
                _behaviour.View = this;
                //_behaviour.enabled = enabled;
            }
            else if (!_behaviour.enabled) {
                _behaviour.enabled = true;
            }

            UpdateParent();

            // TODO:
            ////场景激活后才能够打开托管类型view
            //if (Main.currentScene != null && !(this is IRender)) {
            //    if (_visible && destroyType != ViewInstanceType.MANUAL_DISPOSE && Main.currentScene.statu < SceneProcessStatu.CREATIONCOMPLETE) {
            //        Main.currentScene.AddActiveView(this);
            //    }
            //}

            if (isFirstCreated) {
                OnAttachHost();
            }

            _hostObject.SetActive(false);

            ShowView();
        }

        private void DetachHost() {
            if (_hostObject != null) {
                if (_canDestroyHost) {
                    GameObjectFinder.FinderDetach(_objFinder);
                    UnityEngine.Object.Destroy(_hostObject);
                }
                _view = null;
                _hostObject = null;
                _objFinder = null;
            }
            else {
                // TODO:
                //visible = false;
                if (_behaviour != null) {
                    UnityEngine.Object.Destroy(_behaviour);
                    _behaviour = null;
                }
            }

            //UnloadAssets();
        }

        private void ShowView() {
            if (_destroyed) {
                return;
            }

            try {
                _hostObject.SetActive(true);

                //DoVisible(_visible);
                OnShow(_viewData);
                //updateDisplayIndex();
                //onActionEnter();
            }
            catch (System.Exception e) {
                Debug.LogException(e);
            }
            finally {
                _createCompleted = true;
            }
        }
    }
}
