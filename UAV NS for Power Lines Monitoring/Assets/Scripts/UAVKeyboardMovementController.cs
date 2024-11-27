using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAVKeyboardMovementController : MonoBehaviour
{
	public Transform uav;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            //move forward
        }
		if (Input.GetKeyDown(KeyCode.A))
		{
            //move left
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
            //move right
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
            //move backward
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			//move up
		}
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			//move down
		}
	}
}
