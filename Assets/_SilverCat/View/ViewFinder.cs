using UnityEngine;

namespace SilverCat {

    public abstract partial class ViewBase {

        private GameObjectFinder _objFinder;

        protected bool IsFinderInit() {
            return _hostObject != null && _objFinder != null;
        }

        protected bool HasObject(string objName) {
            if (!IsFinderInit()) {
                Debug.LogFormat("{0} view finder isn't inited.", this);
                return false;
            }

            return _objFinder.HasObject(objName);
        }

        protected GameObject GetObject(string objName) {
            if (!IsFinderInit()) {
                Debug.LogFormat("{0} view finder isn't inited.", this);
                return null;
            }

            return _objFinder.GetObject(objName);
        }

        protected T GetComponent<T>(string objName) where T : Component {
            if (!IsFinderInit()) {
                Debug.LogFormat("{0} view finder isn't inited.", this);
                return null;
            }

            return _objFinder.GetComponent<T>(objName);
        }

    }

}
