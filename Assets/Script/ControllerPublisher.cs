using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
    public class ControllerPublisher : UnityPublisher<MessageTypes.Geometry.Transform>
    {

        public GameObject targetJoint;
        
        // public Transform PublishedTransform;
        private MessageTypes.Geometry.Transform message;

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void FixedUpdate()
        {
            if (Time.frameCount %1 == 0)
            {
                //print("play every 2 sconds");
                UpdateMessage();
                //cube.transform.localPosition=;

            }



            // debug test
            //if (interactable.isHovering)
            //{
            //    Debug.Log("Mes:publishing" + PublishedTransform.position);

            //}
            
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Geometry.Transform();
            message.translation = new MessageTypes.Geometry.Vector3();
            message.rotation = new MessageTypes.Geometry.Quaternion();
        }
        private void UpdateMessage()
        {
 
            message.translation = GetGeometryVector3(targetJoint.transform.position);
            
            message.rotation = GetGeometryQuaternion(targetJoint.transform.rotation);

            Publish(message);

            //Debug.Log("Mes:publishing" + targetJoint.name+message.translation.x+"_"+ targetJoint.name + message.translation.y + "_" + targetJoint.name + message.translation.z);


        }

        private static MessageTypes.Geometry.Vector3 GetGeometryVector3(Vector3 vector3)
        {
            MessageTypes.Geometry.Vector3 geometryVector3 = new MessageTypes.Geometry.Vector3();
            geometryVector3.x = vector3.x;
            geometryVector3.y = vector3.y;
            geometryVector3.z = vector3.z;
            return geometryVector3;
        }

        private static MessageTypes.Geometry.Quaternion GetGeometryQuaternion(Quaternion Quaternion)
        {
            MessageTypes.Geometry.Quaternion geometryQuaternion = new MessageTypes.Geometry.Quaternion();
            geometryQuaternion.x = Quaternion.x;
            geometryQuaternion.y = Quaternion.y;
            geometryQuaternion.z = Quaternion.z;
            geometryQuaternion.w = Quaternion.w;
            return geometryQuaternion;
        }

    }
}