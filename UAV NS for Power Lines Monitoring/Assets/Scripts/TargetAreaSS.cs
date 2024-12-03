using UnityEngine;

public class TargetAreaSS : MonoBehaviour
{
	public UAVAgentSS agent;
	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "Agent")
			agent.OnTargetAreaExit();
	}
}
