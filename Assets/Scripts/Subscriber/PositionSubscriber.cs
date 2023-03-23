using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Position = RosMessageTypes.Nav.OdometryMsg;


public class PositionSubscriber : MonoBehaviour
{
    public GameObject DOTS;
    public string topicname;
    public double X;
    public double Y;
    private Quaternion RotationQ;
    private Quaternion RotationE;
    public double scaler = 1;
    public static Vector3 subscriber;
    
    

    void Start()
    {
       ROSConnection.GetOrCreateInstance().Subscribe<Position>(topicname, Show);
        subscriber.x = (float)X;
        subscriber.z = (float)Y;

 
    }

    void Show(Position odom)
    {

            X = odom.pose.pose.position.x * scaler;
            Y = odom.pose.pose.position.y * scaler;

            RotationQ.x = (float)odom.pose.pose.orientation.x;
            RotationQ.y = (float)odom.pose.pose.orientation.y;
            RotationQ.z = (float)odom.pose.pose.orientation.z;
            RotationQ.w = (float)odom.pose.pose.orientation.w;

            RotationE.eulerAngles = new Vector3(0.0f, -RotationQ.eulerAngles.z, 0.0f);
           

        
    }
    private void Update()
    {
     
        DOTS.transform.localPosition = new Vector3((float)X, 0.1f, (float)Y); //* Tracker.transform.localScale.x;// Tracker.transform.position;
        DOTS.transform.localRotation = RotationE;
        subscriber = DOTS.transform.localPosition;
    }
}   