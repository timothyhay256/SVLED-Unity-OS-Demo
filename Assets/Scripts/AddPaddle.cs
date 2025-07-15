using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using UnityEngine;
using System.Collections;

public class AddPaddle : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject leftPaddle;
    public GameObject rightPaddle;

    public GameObject noContact;

    public GameObject left;
    public GameObject paintSystem;

    public float scale = 1.0f;
    public bool sendPos = true;

    UdpClient client;
    IPEndPoint endpoint;
    private void Start()
    {
        StartCoroutine(WaitForChildrenAndAddComponent());

        if (sendPos)
        {
            Debug.Log("connecting on port 5005");
            client = new UdpClient();
            endpoint = new IPEndPoint(IPAddress.Loopback, 5005);
        }
    }

    private IEnumerator WaitForChildrenAndAddComponent()
    {
        yield return new WaitUntil(() => noContact.transform.childCount >= 2);

        leftHand = noContact.transform.GetChild(0).GetChild(0).gameObject;
        rightHand = noContact.transform.GetChild(1).GetChild(0).gameObject;

        left = Instantiate(leftPaddle, leftHand.transform, true);
        left.transform.localPosition = new Vector3(.007f, .00029f, -.0567f); // Derived from just manually moving the paddle until it was positioned nicely
        left.transform.localEulerAngles = new Vector3(45f, -157f, -139.425f);

        paintSystem.GetComponent<PaintSystem>().brushElement = left.transform.GetChild(0).gameObject;

        GameObject right = Instantiate(rightPaddle, rightHand.transform, true);
        right.transform.localPosition = new Vector3(0.0004f, 0.0109f, -.0056f);
        right.transform.localEulerAngles = new Vector3(-121.6f, 143.275f, -57.14f);
    }

    void Update()
    {
        if (sendPos)
        {
            if (leftHand == null)
                return;

            Vector3 scaledPos = leftHand.transform.position * scale;

            // Debug.Log($"Sending: {scaledPos}");

            byte[] data = new byte[12]; // 3 floats × 4 bytes
            Buffer.BlockCopy(BitConverter.GetBytes(scaledPos.x), 0, data, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(scaledPos.y), 0, data, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(scaledPos.z), 0, data, 8, 4);

            client.Send(data, data.Length, endpoint);
        }
    }

    void OnApplicationQuit()
    {
        if (sendPos)
        {
            client.Close();
        }
    }
}
