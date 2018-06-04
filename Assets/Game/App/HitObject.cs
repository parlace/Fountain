using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fountain {
    public class HitObject : MonoBehaviour {

        public Camera ca;
        private Ray ra;
        private RaycastHit hit;


        // Use this for initialization
        void Start() { }

        // Update is called once per frame
        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                ra = ca.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ra, out hit)) {
                    var obj = hit.collider.gameObject;
                    Debug.LogFormat("hit object is a {0}", obj.name);
                    if (obj.name == "HouseA4") {
                        for (int i = 0, n = obj.transform.childCount; i < n; ++i) {
                            var rd = obj.transform.GetChild(i).GetComponent<Renderer>();
                            if (rd != null) {
                                rd.material.shader = Shader.Find("Nature/Terrain/Diffuse");
                            }
                        }
                    }
                }
            }
        }
    }
}