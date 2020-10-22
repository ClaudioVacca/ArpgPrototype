using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    private Vector3 offsetFromPLayer;
    public float speed = 2;

    void Start()
    {
        offsetFromPLayer = player.transform.position - transform.position;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position - offsetFromPLayer, speed * Time.deltaTime);
    }
}
