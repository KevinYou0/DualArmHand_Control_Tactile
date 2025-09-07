using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class JoyPosePublisher : UnityPublisher<MessageTypes.Sensor.Joy>
    {
        public string FrameId = "Unity";
        public Transform Target;

        private MessageTypes.Sensor.Joy message;
        private bool shouldPublish = false; // control flag

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void Update()
        {
            // Toggle publishing on/off with the "P" key
            if (Input.GetKeyDown(KeyCode.P))
                shouldPublish = !shouldPublish;

            if (shouldPublish)
            {
                UpdateMessage();
                Publish(message);
            }
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Sensor.Joy();
            message.header.frame_id = FrameId;
            message.axes = new float[7]; // 3 for position, 4 for quaternion
            message.buttons = new int[0];
        }

        private void UpdateMessage()
        {
            message.header.Update();

            message.axes[0] = Target.position.x;
            message.axes[1] = Target.position.y;
            message.axes[2] = Target.position.z;
            message.axes[3] = Target.rotation.x;
            message.axes[4] = Target.rotation.y;
            message.axes[5] = Target.rotation.z;
            message.axes[6] = Target.rotation.w;
        }
    }
}
