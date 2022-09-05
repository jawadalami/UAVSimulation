using System.Collections.Generic;
using TargetDetection;
using UAVs;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.TargetDetection
{
	[CreateAssetMenu(menuName = "Events/Uav-Targets Event Channel")]
	public class UavTargetsPairEventChannelSO: ScriptableObject
	{
		private UnityAction<Uav,List<Target>> onEventRaised;
	
		public void RaiseEvent(Uav uav,List<Target> targets)
		{
			onEventRaised?.Invoke(uav, targets);
		}

		public void Subscribe(UnityAction<Uav,List<Target>> action)
		{
			onEventRaised += action;
		}

		public void Unsubscribe(UnityAction<Uav,List<Target>> action)
		{
			onEventRaised -= action;
		}
	}
}