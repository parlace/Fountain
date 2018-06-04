

using UnityEngine;

namespace SilverCat{

    public abstract class UIView : ViewBase {

        private UILayer _uiLayer;

        public UILayer UILayer
        {
            get { return _uiLayer; }
            set {
                if (_uiLayer != value)
                {
                    // TODO: 
                    _uiLayer = value;
                    // TODO:
                }
            }
        }

        protected void AttachSelfAsHost() {
            base.AttachSelfAsHost((obj) => {
                obj.AddComponent<RectTransform>();
            });
        }

        protected override void OnAttachHost() {
            //if (!(this is IRender))
            //    UIManager.FitScreen(uiRootTransform);

            //if (uiLayer == UILayer.MODE_LAYER)
            //    mode = true;
            //else
            //    mode = _mode;
        }
    }

}
