using UnityEngine;

namespace SilverCat {

    public abstract class AppBase : MonoBehaviour {

        #region unity override
        private void Start() {
            AppLaunch();
        }

        private void OnApplicationQuit() {
            AppQuit();
        }
        #endregion

        // App base function
        private void AppLaunch() {

            OnAppPreLaunch();

            AppMain.Start(this);

            OnAppLaunch();

            //Application.backgroundLoadingPriority = ThreadPriority.Low;
        }

        private void AppQuit() {
            AppMain.Destroy();
            OnAppQuit();
        }

        protected abstract void OnAppPreLaunch();
        protected abstract void OnAppLaunch();
        protected abstract void OnAppQuit();
    }
}