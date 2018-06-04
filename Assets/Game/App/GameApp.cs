using System;
using UnityEngine;

using SilverCat;

namespace Fountain {

    public sealed class GameApp : AppBase {
        protected override void OnAppPreLaunch() {

        }

        protected override void OnAppLaunch() {
            UIManager.Open<PressAnyKeyView>(UILayer.BaseLayer, (Action)OnOpenPressAnyKeyView);
        }

        protected override void OnAppQuit() {

        }

        private void OnOpenPressAnyKeyView() {

        }
    }

}
