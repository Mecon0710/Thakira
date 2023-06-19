using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VIVE.EyeGaze
{
    public class FollowCamera : MonoBehaviour
    {
        // Start is called before the first frame update
        public Transform cameraTransform;
        private Vector3 offset;
        void Start()
        {
            this.transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z + 1);
        }

        // Update is called once per frame
        void Update()
        {
            this.transform.position = new Vector3(this.transform.position.x, cameraTransform.position.y, this.transform.position.z);
        }
    }

}
