using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour, IUdpDataReceiver
{
    public float rotSpeed = 20;
    Acc acc=new Acc();
    float pitch,roll , yaw;
    public void DecodeData(byte[] _rawData)
    {
        if (_rawData.Length ==0)
            return;
        acc.x = System.BitConverter.ToSingle(_rawData, 0);
        acc.y = System.BitConverter.ToSingle(_rawData, 4);
        acc.z = System.BitConverter.ToSingle(_rawData, 8);
        //print(">> " + acc.x + " " + acc.y + " " + acc.z);
    }

    private void Update()
    {
        pitch = 180 * Mathf.Atan(acc.x / Mathf.Sqrt(acc.y * acc.y + acc.z * acc.z)) / Mathf.PI;
        roll = 180 * Mathf.Atan(acc.y / Mathf.Sqrt(acc.x * acc.x + acc.z * acc.z)) / Mathf.PI;
        yaw = 180 * Mathf.Atan(acc.z / Mathf.Sqrt(acc.x * acc.x + acc.z * acc.z)) / Mathf.PI;

        //transform.eulerAngles = new Vector3(roll, yaw, -pitch);
        var rot = Quaternion.Euler(roll, yaw, -pitch);
        transform.rotation = Quaternion.Lerp(transform.rotation , rot,Time.deltaTime * rotSpeed);
    }

    void OnGUI() {
        GUI.Label(new Rect(10, 10, 100, 20), "roll "+roll.ToString());
        GUI.Label(new Rect(10, 35, 100, 20), "pitch " + (-pitch).ToString());
        GUI.Label(new Rect(10, 55, 100, 20), "yaw " + yaw.ToString());

    }
}
[System.Serializable]
public class Acc
{
    public float x, y, z;
}
