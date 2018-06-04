using System;
using UnityEngine;

using SilverCat;

namespace Fountain {

    public class GameApp : AppBase {
        protected override void OnAppPreCreate() {

        }

        protected override void OnAppCreate() {
            UIManager.Open<PressAnyKeyView>(UILayer.BaseLayer, (Action)OnOpenPressAnyKeyView);
        }

        protected override void OnAppQuit() {

        }

        private void OnOpenPressAnyKeyView() {

        }
    }

}
