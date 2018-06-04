using UnityEngine;

namespace SilverCat {

    public class ViewBehaviour : MonoBehaviour {
        public ViewBase View { get; set; }

        private void LateUpdate() {
            if (View == null || !View.CreateCompleted || View.Destroyed /*|| View.Enabled*/) {
                return;
            }

            try {
                View.Update();
            }
            catch (System.Exception e) {
                Debug.LogException(e);
            }
        }
    }

}
