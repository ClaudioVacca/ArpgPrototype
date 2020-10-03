using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offsetFromPLayer;
    public float speed = 2;


    // Start is called before the first frame update
    void Start()
    {
        offsetFromPLayer = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position - offsetFromPLayer, speed * Time.deltaTime);
    }
}
