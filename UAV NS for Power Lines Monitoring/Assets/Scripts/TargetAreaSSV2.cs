using UnityEngine;

public class TargetAreaSSV2 : MonoBehaviour
{
	public UAVAgentSSV2 agent;
	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "Agent")
			agent.OnTargetAreaExit();
	}
}
