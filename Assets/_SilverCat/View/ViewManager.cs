using System;
using System.Collections.Generic;
using UnityEngine;

namespace SilverCat {

    public static class ViewManager {
        private static readonly Dictionary<Type, ViewBase> _viewDict = new Dictionary<Type, ViewBase>();

        public static T CreateView<T>(Transform parentTrans, object data = null, GameObject host = null, bool beClone = false)
            where T : ViewBase, new()
        {
            return CreateView(typeof(T), parentTrans, data, host, beClone) as T;
        }

        public static ViewBase CreateView(Type type, Transform parentTrans, object data = null, GameObject host = null, bool beClone = false) {
            ViewBase view = null;

            if (_viewDict.ContainsKey(type)) {
                view = _viewDict[type];
            }

            if (view == null) {
                view = Activator.CreateInstance(type) as ViewBase;
            }

            if (view == null) {
                return null;
            }

            if (host == null) {
                return view.Init(parentTrans, data) ? view : null;
            }
            else {
                return view.Init(parentTrans, data, host, beClone) ? view : null;
            }
        }
    }
}