using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace SilverCat {

    public abstract partial class ViewBase {

        private string _assetName;

        protected void AttachHost(string assetsName) {
            if (string.IsNullOrEmpty(assetsName)) {
                return;
            }

            if (_assetName == assetsName && _hostObject) {
                AttachHost(_hostObject, _canDestroyHost);
                return;
            }

            _assetName = assetsName;

            //if (this is IRender || )

            //if ()
        }
    }

}
