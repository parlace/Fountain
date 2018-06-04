using System;
using System.Collections;
using System.Collections.Generic;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace SilverCat {

    public static class SceneManager {

        public static void ChangeScene<T>() where T : SceneBase, new () {
            ChangeScene(typeof(T));
        }

        public static void ChangeScene(Type t) {
            SceneBase sc = Activator.CreateInstance(t) as SceneBase;

            sc.Init();
        }
    }

}
