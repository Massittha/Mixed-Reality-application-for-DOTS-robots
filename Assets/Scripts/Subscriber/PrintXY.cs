using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Position = RosMessageTypes.Nav.OdometryMsg;
using UnityEngine.UI;


public class PrintXY : MonoBehaviour
{
    
    private float timeElapsed;
    public float timer = 0.0f;
    public string topicname;
    public double X;
    public double Y;

    public double scaler = 1;
   
    Text text;

    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<Position>(topicname, Show);
        print("Odometry Subscriber Registered");
        text = GetComponent<Text>(); 

    }

    void Show(Position odom)
    {

        if (timeElapsed > timer)
        {
            X = odom.pose.pose.position.x * scaler;
            Y = odom.pose.pose.position.y * scaler;
            print(odom);

        }
    }
    private void Update()
    {
        timeElapsed += Time.deltaTime;
        text.text = "Pos_X: " + X +"\n" + "Pos_Y: " + Y;
      
    }
}