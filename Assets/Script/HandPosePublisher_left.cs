using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class HandPosePublisher_left : UnityPublisher<MessageTypes.Sensor.Joy>
    {
        public string FrameId = "Unity";
        public Transform Target;
        public IKJointAngleReader_left joints;

        [Header("Manual Joint Inputs (radians)")]
        public bool useManualInput = false;

        public float joint_0;
        public float joint_1;
        public float joint_2;
        public float joint_3;

        public float joint_4;
        public float joint_5;
        public float joint_6;
        public float joint_7;

        public float joint_8;
        public float joint_9;
        public float joint_10;
        public float joint_11;

        public float joint_12;
        public float joint_13;
        public float joint_14;
        public float joint_15;

        private MessageTypes.Sensor.Joy message;
        private bool shouldPublish = false;

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }

        private void Update()
        {
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
            message.axes = new float[16];
            message.buttons = new int[0];
        }

        private void UpdateMessage()
        {
            message.header.Update();

            float[] source = new float[16];

            if (useManualInput)
            {
                source[0] = joint_0;
                source[1] = joint_1;
                source[2] = joint_2;
                source[3] = joint_3;

                source[4] = joint_4;
                source[5] = joint_5;
                source[6] = joint_6;
                source[7] = joint_7;

                source[8] = joint_8;
                source[9] = joint_9;
                source[10] = joint_10;
                source[11] = joint_11;

                source[12] = joint_12;
                source[13] = joint_13;
                source[14] = joint_14;
                source[15] = joint_15;
            }
            else if (joints != null && joints.jointAnglesRad.Count >= 16)
            {
                for (int i = 0; i < 16; i++)
                {
                    source[i] = joints.jointAnglesRad[i];
                }
            }

            for (int i = 0; i < 16; i++)
            {
                message.axes[i] = source[i];
            }
        }
    }
}