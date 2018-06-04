
using System;
using UnityEngine;

namespace SilverCat {

    public static class GameObjectTool {

        private const int _foreachDepth = 50;

        public static void ForeachAllGameObject(GameObject obj, Action<GameObject> callback) {
            if (obj == null) {
                return;
            }

            ForeachAllTransform(obj.transform, callback, _foreachDepth);
        }

        private static void ForeachAllTransform(Transform trans, Action<GameObject> callback, int depth) {
            if (depth < 1) {
                return;
            }

            if (trans == null) {
                return;
            }

            callback(trans.gameObject);

            for (int i = 0, n = trans.childCount; i < n; ++i) {
                ForeachAllTransform(trans.GetChild(i), callback, depth - 1);
            }
        }

        public static void AddChild(Transform child, Transform parent) {
            if (child.parent != parent) {
                child.SetParent(parent, false);
            }

            child.localPosition = Vector3.zero;
            child.localScale = Vector3.one;
            child.localRotation = Quaternion.identity;
        }
    }

}