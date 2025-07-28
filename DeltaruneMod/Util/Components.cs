using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
    }
}
