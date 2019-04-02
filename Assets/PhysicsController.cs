using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{

    public Physics physics;
    public Vector3 input;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            input.z = 1;
        if (Input.GetKey(KeyCode.S))
            input.z = -1;

        if (Input.GetKey(KeyCode.D))
            input.x = 1;
        if (Input.GetKey(KeyCode.A))
            input.x = -1;

        if (Input.GetKey(KeyCode.Q))
            input.y = 1;
        if (Input.GetKey(KeyCode.E))
            input.y = -1;

        physics.LinearInput(input);
    }
}
