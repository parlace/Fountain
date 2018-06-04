using System;
using System.Collections.Generic;
using UnityEngine;

namespace SilverCat {

    public partial class GameObjectFinder {

        private readonly Dictionary<string, GameObject> _objectDict = new Dictionary<string, GameObject>();

        private GameObject _hostObject = null;

        private GameObjectFinder(GameObject obj) {
            AttachHost(obj);
        }

        public static GameObjectFinder CreateFinder(GameObject obj) {
            if (obj == null) {
                return null;
            }

            GameObjectFinder finder = null;
            if (IsUsingCache) {
                int id = obj.GetInstanceID();
                if (!_cacheDict.TryGetValue(id, out finder)) {
                    finder = new GameObjectFinder(obj);
                    _cacheDict.Add(id, finder);
                }
            }
            else {
                finder = new GameObjectFinder(obj);
            }

            return finder;
        }

        public static void DestroyFinder(GameObjectFinder finder) {
            if (!IsUsingCache) {
                finder.DetachHost();
            }
            else if (finder._hostObject != null) {
                int id = finder._hostObject.GetInstanceID();
                finder.DetachHost();
                _cacheDict.Remove(id);
            }
        }

        private void AttachHost(GameObject host) {
            _hostObject = host;
            GameObjectTool.ForeachAllGameObject(host, (v) => {
                _objectDict[v.name] = v;
            });
        }

        private void DetachHost() {
            _objectDict.Clear();
            _hostObject = null;
        }

        public bool HasObject(string objName) {
            if (string.IsNullOrEmpty(objName)) {
                return false;
            }

            return _objectDict.ContainsKey(objName);
        }

        public GameObject GetObject(string objName) {
            if (string.IsNullOrEmpty(objName)) {
                return null;
            }

            GameObject result;
            _objectDict.TryGetValue(objName, out result);
            return result;
        }

        public T GetComponent<T>(string objName) where T : Component {
            if (string.IsNullOrEmpty(objName)) {
                return null;
            }

            GameObject obj = GetObject(objName);
            if (obj == null) {
                return null;
            }

            return obj.GetComponent<T>();
        }
    }
}