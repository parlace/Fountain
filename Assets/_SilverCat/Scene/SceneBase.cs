
using System.Collections;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace SilverCat {
    public class SceneBase {

        protected virtual void OnCreate() { }

        private string _name;

        public void Init() {
            OnCreate();
            AppMain.AppBase.StartCoroutine(LoadChangeScene());
        }

        public void AttachHost(string sceneName) {
            _name = sceneName;
        }

        IEnumerator LoadChangeScene() {
            yield return UnitySceneManager.LoadSceneAsync(_name);
        }
    }
}
