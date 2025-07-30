using R2API.Networking.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace DeltaruneMod.Util
{
    public class Components
    {
        public class FollowTarget : MonoBehaviour
        {
            public Transform target;
            public Vector3 offset = Vector3.zero;
            public bool followRotation = false;

            private void FixedUpdate()
            {
                if (!target) return;
                //Debug.Log(transform.gameObject.name + "following " + target.gameObject.name + "\n" + target.position + "\n" + transform.position);
                transform.position = target.position + offset;

                if (followRotation)
                {
                    transform.rotation = target.rotation;
                }
            }
        }

        public class TextController : NetworkBehaviour
        {
            public TextMeshPro textMesh;

            public void SetText(string newText)
            {
                textMesh.text = newText;
            }

        }
    }
}
