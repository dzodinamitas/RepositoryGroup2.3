using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class UAVAgentSS : Agent
{
	public Rigidbody[] rotors;
	public float[] thrustForces = new float[4];
	public float maxThrust = 10f;
	public Rigidbody body;
	float idleThrust;
	public float speed = 5f;
	public Rigidbody rb;

	private void FixedUpdate()
	{
		//for (int i = 0; i < rotors.Length; i++)
		//{
		//	ApplyThrust(rotors[i], thrustForces[i]);
		//}

		Vector3 rotationAngles = body.rotation.eulerAngles;
		float pitch = NormalizeAngle(rotationAngles.x); 
		float roll = NormalizeAngle(rotationAngles.z);

		//Debug.Log($"Pitch: {pitch:F2}°, Roll: {roll:F2}°");
		//Debug.Log(StepCount);
		//if(StepCount > 300)
		//{
		//	EndEpisode();
		//}
	}

	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(transform.position.x);
		sensor.AddObservation(transform.position.y);
		sensor.AddObservation(transform.position.z);
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
		var dirToGo = Vector3.zero;
		var rotateDir = Vector3.zero;

		var action = act[0];

		switch (action)
		{
			case 0:
				ApplyThrust(rotors[0], idleThrust);
				ApplyThrust(rotors[1], idleThrust);
				ApplyThrust(rotors[2], idleThrust);
				ApplyThrust(rotors[3], idleThrust);
				break;
			case 1:
				ApplyThrust(rotors[0], idleThrust - speed);
				ApplyThrust(rotors[1], idleThrust - speed);
				ApplyThrust(rotors[2], idleThrust + speed);
				ApplyThrust(rotors[3], idleThrust + speed);
				break;
			case 2:
				ApplyThrust(rotors[0], idleThrust + speed);
				ApplyThrust(rotors[1], idleThrust + speed);
				ApplyThrust(rotors[2], idleThrust - speed);
				ApplyThrust(rotors[3], idleThrust - speed);
				break;
			case 3:
				ApplyThrust(rotors[0], idleThrust - speed);
				ApplyThrust(rotors[1], idleThrust + speed);
				ApplyThrust(rotors[2], idleThrust + speed);
				ApplyThrust(rotors[3], idleThrust - speed);
				break;
			case 4:
				ApplyThrust(rotors[0], idleThrust + speed);
				ApplyThrust(rotors[1], idleThrust - speed);
				ApplyThrust(rotors[2], idleThrust - speed);
				ApplyThrust(rotors[3], idleThrust + speed);
				break;
			case 5:
				ApplyThrust(rotors[0], idleThrust + speed);
				ApplyThrust(rotors[1], idleThrust + speed);
				ApplyThrust(rotors[2], idleThrust + speed);
				ApplyThrust(rotors[3], idleThrust + speed);
				break;
		}
	}

	public override void OnActionReceived(ActionBuffers actionBuffers)
	{
		MoveAgent(actionBuffers.DiscreteActions);
		AddReward(1f / MaxStep);
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
		rb.linearVelocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rotors[0].linearVelocity = Vector3.zero;
		rotors[0].angularVelocity = Vector3.zero;
		rotors[1].linearVelocity = Vector3.zero;
		rotors[1].angularVelocity = Vector3.zero;
		rotors[2].linearVelocity = Vector3.zero;
		rotors[2].angularVelocity = Vector3.zero;
		rotors[3].linearVelocity = Vector3.zero;
		rotors[3].angularVelocity = Vector3.zero;
	}

}
