using System;

namespace SilverCat {

    public class AppMain {

        private static AppBase _appBase;
        public static AppBase AppBase {
            get { return _appBase; }
        }

        public static void Start(AppBase appBase) {
            if (_appBase != null) {
                return;
            }

            _appBase = appBase;
            UnityEngine.Object.DontDestroyOnLoad(_appBase.gameObject);

            UIManager.Init();
        }

        public static void Destroy() {
            if (_appBase == null) {
                return;
            }

            //UIManager.Destroy();
            //SceneManager.Destroy();
            UnityEngine.Object.Destroy(_appBase.gameObject);
            _appBase = null;
            GC.Collect();
        }
    }

}