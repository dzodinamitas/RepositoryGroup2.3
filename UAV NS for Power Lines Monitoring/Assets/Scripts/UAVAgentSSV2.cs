using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

public class UAVAgentSSV2 : Agent
{
	public Rigidbody[] rotorsRB;
	public float[] thrustForces = new float[4];
	public float maxThrust = 10f;
	public Rigidbody body;
	float idleThrust;
	public float speed = 5f;
	public Rigidbody frameRB;
	public Rigidbody uavRB;
	public float randomTiltMaxValue = 20f;
	public float tiltForce = 10f;
	public bool enableHardcodedAreaChecking = false;
	bool firstFrame = true;
	private void FixedUpdate()
	{
		//for (int i = 0; i < rotors.Length; i++)
		//{
		//	ApplyThrust(rotors[i], thrustForces[i]);
		//}

		//Vector3 rotationAngles = body.rotation.eulerAngles;
		//float pitch = NormalizeAngle(rotationAngles.x); 
		//float roll = NormalizeAngle(rotationAngles.z);

		//Debug.Log($"Pitch: {pitch:F2}°, Roll: {roll:F2}°");
		//Debug.Log(StepCount);
		//if(StepCount > 300)
		//{
		//	EndEpisode();
		//}
		ApplyThrust(frameRB, idleThrust * 4);
		if (enableHardcodedAreaChecking)
			if (CheckForExit())
				OnTargetAreaExit();
	}
	private bool CheckForExit()
	{
		if (transform.position.x < -11 || transform.position.x > 11 || transform.position.y > 8 || transform.position.y < -3 || transform.position.z < -11 || transform.position.z > 11)
			return true;
		return false;
	}
	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(transform.position.x / 20);
		sensor.AddObservation(transform.position.y / 20);
		sensor.AddObservation(transform.position.z / 20);
		sensor.AddObservation(NormalizeAngle(body.rotation.eulerAngles.x) / 180);
		sensor.AddObservation(NormalizeAngle(body.rotation.eulerAngles.z) / 180);
	}

	private void ApplyThrust(Rigidbody rotor, float thrust)
	{
		Vector3 upwardForce = rotor.transform.up * thrust;
		rotor.AddForce(upwardForce, ForceMode.Force);
	}

	private float NormalizeAngle(float angle)
	{
		angle = angle % 360;
		if (angle > 180) angle -= 360;
		return angle;
	}
	public void OnTargetAreaExit()
	{
		AddReward(-1f);
		EndEpisode();
	}
	public override void Initialize()
	{
		float gravityForce = body.mass * Physics.gravity.magnitude;
		float balancedThrust = gravityForce / 4;
		idleThrust = balancedThrust;

		for (int i = 0; i < thrustForces.Length; i++)
		{
			thrustForces[i] = balancedThrust;
		}
		ResetAgent();
	}
	public void MoveAgent(ActionSegment<int> act)
	{
		var action = act[0];

		switch (action)
		{
			case 0:
				break;
			case 1:
				frameRB.AddTorque(frameRB.transform.right * tiltForce, ForceMode.Force);
				break;
			case 2:
				frameRB.AddTorque(-frameRB.transform.right * tiltForce, ForceMode.Force);
				break;
			case 3:
				frameRB.AddTorque(frameRB.transform.forward * tiltForce, ForceMode.Force);
				break;
			case 4:
				frameRB.AddTorque(-frameRB.transform.forward * tiltForce, ForceMode.Force);
				break;
			case 5:
				ApplyThrust(frameRB, speed);
				break;
		}
	}

	public override void OnActionReceived(ActionBuffers actionBuffers)
	{
		MoveAgent(actionBuffers.DiscreteActions);
		AddReward(5f / MaxStep);
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		var discreteActionsOut = actionsOut.DiscreteActions;

		if (Input.GetKey(KeyCode.W))
		{
			discreteActionsOut[0] = 1;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			discreteActionsOut[0] = 4;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			discreteActionsOut[0] = 2;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			discreteActionsOut[0] = 3;
		}
		else if (Input.GetKey(KeyCode.Space))
		{
			discreteActionsOut[0] = 5;
		}
		else
		{
			discreteActionsOut[0] = 0;
		}
	}

	public override void OnEpisodeBegin()
	{
		ResetAgent();
	}
	void ResetAgent()
	{
		transform.rotation = Quaternion.identity;
		transform.position = new Vector3(0, 0, 0);
		frameRB.linearVelocity = Vector3.zero;
		frameRB.angularVelocity = Vector3.zero;
		uavRB.linearVelocity = Vector3.zero;
		uavRB.angularVelocity = Vector3.zero;
		rotorsRB[0].linearVelocity = Vector3.zero;
		rotorsRB[0].angularVelocity = Vector3.zero;
		rotorsRB[1].linearVelocity = Vector3.zero;
		rotorsRB[1].angularVelocity = Vector3.zero;
		rotorsRB[2].linearVelocity = Vector3.zero;
		rotorsRB[2].angularVelocity = Vector3.zero;
		rotorsRB[3].linearVelocity = Vector3.zero;
		rotorsRB[3].angularVelocity = Vector3.zero;

		AddRandomTilt();
	}
	public void AddRandomTilt()
	{
		float randomX = Random.Range(-randomTiltMaxValue, randomTiltMaxValue);
		float randomZ = Random.Range(-randomTiltMaxValue, randomTiltMaxValue);
		transform.eulerAngles = new Vector3(randomX, transform.eulerAngles.y, randomZ);
	}
}
