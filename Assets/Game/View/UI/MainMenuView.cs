
using UnityEngine;
using UnityEngine.UI;
using SilverCat;

namespace Fountain {
    internal class MainMenuView : UIView {
        protected override void OnCreate() {
            var obj = Resources.Load<GameObject>("View/Opening/MainMenuView");
            AttachHost(obj);

            GetComponent<Button>("BtnNewGame").onClick.AddListener(OnBtnNewGame);
        }

        public void OnBtnNewGame() {
            UIManager.Close(this);
            SceneManager.ChangeScene<TownScene>();
        }
    }
}