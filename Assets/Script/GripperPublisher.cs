/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class GripperPublisher : UnityPublisher<MessageTypes.Sensor.Joy>
    {
        private JoyAxisReader[] JoyAxisReaders;
        private JoyButtonReader[] JoyButtonReaders;

        public string FrameId = "Unity";

        private MessageTypes.Sensor.Joy message;
        private int gripper_state = 0;
        private int command = 4;

        protected override void Start()
        {
            base.Start();
            InitializeGameObject();
            InitializeMessage();
        }

        private void Update()
        {
            UpdateMessage();
        }      

        private void InitializeGameObject()
        {
            JoyAxisReaders = GetComponents<JoyAxisReader>();
            JoyButtonReaders = GetComponents<JoyButtonReader>();
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Sensor.Joy();
            message.header.frame_id = FrameId;
            //message.axes = new float[JoyAxisReaders.Length];
            message.axes = new float[6];
            //message.buttons = new int[JoyButtonReaders.Length];
            message.buttons = new int[6];
        }

        private void UpdateMessage()
        {
            
            if (Input.GetKey("1") & gripper_state == 0)
            {
                command = 1;
                gripper_state = 1;
            }
            if (Input.GetKey("2") & gripper_state == 1)
            {
                command = 2;
                gripper_state = 0;
            }
            if (Input.GetKey("3"))
            {
                command = 3;
            }

            message.header.Update();

            for (int i = 0; i < JoyAxisReaders.Length; i++)
                message.axes[i] = JoyAxisReaders[i].Read();
            
            for (int i = 0; i < JoyButtonReaders.Length; i++)
                message.buttons[i] = (JoyButtonReaders[i].Read() ? 1 : 0);

            message.buttons[0] = command;
            Publish(message);
            //Debug.Log(message);
            //Debug.Log(command);
            command = 4;
        }
    }
}
