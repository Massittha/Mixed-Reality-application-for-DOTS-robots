using UnityEngine;

//ROS Mod
using Unity.Robotics.ROSTCPConnector;
using Move = RosMessageTypes.Geometry.TwistMsg;
using Linear = RosMessageTypes.Geometry.Vector3Msg;
using Angular = RosMessageTypes.Geometry.Vector3Msg;

public class JediMode : MonoBehaviour
{
    public GameObject DOTS;
    public GameObject anchor;
    private Vector3 startPosition;
    private Vector3 translateAbsolute;
    private Vector3 translateDots;
    private Quaternion startRotation;
    private float radian;

    ROSConnection ros;
    public string topicName;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = DOTS.transform.localPosition;
        startRotation = DOTS.transform.localRotation;
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<Move>(topicName);
        anchor.transform.localPosition = new Vector3 (PositionSubscriber.subscriber.x, 0.25f, PositionSubscriber.subscriber.z);

    }

    // Update is called once per frame
    private void Update()
    {
        startPosition = PositionSubscriber.subscriber;
        if (anchor.transform.localPosition != startPosition)
        {
            radian = -DOTS.transform.localRotation.eulerAngles.y*Mathf.PI/180;
            translateAbsolute = anchor.transform.localPosition - startPosition;
            translateDots.x = translateAbsolute.x * Mathf.Cos(radian) + translateAbsolute.z * Mathf.Sin(radian);
            translateDots.z = -translateAbsolute.x * Mathf.Sin(radian) + translateAbsolute.z * Mathf.Cos(radian);
            
            Linear linear = new Linear(translateDots.x , translateDots.z, 0.0f);
            Angular angular = new Angular(0.0f, 0.0f, 0.0f);

            Move move = new Move(linear, angular);
            ros.Publish(topicName, move);
            startPosition = PositionSubscriber.subscriber;
        }
        else
        {
            Linear linear = new Linear(0.0f, 0.0f, 0.0f);
            Angular angular = new Angular(0.0f, 0.0f, 0.0f);
            translateDots = new Vector3 (0.0f, 0.0f, 0.0f);
            Move move = new Move(linear, angular);
            ros.Publish(topicName, move);
        }

    }
    private void TranslateStop()
    {
        
    }
}
