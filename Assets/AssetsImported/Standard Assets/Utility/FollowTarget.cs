using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace UnityStandardAssets.Utility
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        public float x;
        public float y;

        private void Start()
        {
            offset = transform.position - target.position;
        }

        private void LateUpdate()
        {
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Camera.main.fieldOfView -= zoom*20;
                Camera.main.fieldOfView=Mathf.Clamp(Camera.main.fieldOfView, 0, 50);
            }     
            transform.position = target.position + offset;
        }

        
    }
}
