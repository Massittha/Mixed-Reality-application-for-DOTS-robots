using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using MessageType = RosMessageTypes.Nav.OdometryMsg;

public class AnyMessage : MonoBehaviour
{
    public string topicName;
   
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<MessageType>(topicName, Show);
        print("Subscriber Registered");
    }

    void Show(MessageType message)
    {
        print(message);
    }
}



