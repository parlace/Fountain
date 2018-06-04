using System.Collections.Generic;
using UnityEngine;

namespace SilverCat {

    public partial class GameObjectFinder {

        private static Dictionary<int, GameObjectFinder> _cacheDict = null;

        public static bool IsUsingCache {
            get { return _cacheDict != null; }
        }

        public void UseCache() {
            if (_cacheDict == null) {
                _cacheDict = new Dictionary<int, GameObjectFinder>();
            }
        }

        public void UnuseCache() {
            if (_cacheDict != null) {
                foreach (var item in _cacheDict) {
                    item.Value.DetachHost();
                }
                _cacheDict.Clear();
            }
        }

        public void RemoveFromCache(GameObject obj) {
            if (!IsUsingCache) {
                return;
            }

            if (obj == null) {
                return;
            }

            int id = obj.GetInstanceID();
            GameObjectFinder finder;
            if (_cacheDict.TryGetValue(id, out finder)) {
                finder.DetachHost();
                _cacheDict.Remove(id);
            }
        }
    }
}
