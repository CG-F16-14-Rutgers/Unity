using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;

public interface IBehaviorUpdate
{
	/// <summary>
	/// A regular pulse to update anything requiring being ticked.
	/// </summary>
	/// <param name="deltaTime">The deltaTime for this update pulse</param>
	/// <returns>true if the manager should continue updating this object,
	/// false if the manager should forget about this object and never
	/// update it again</returns>
	RunStatus BehaviorUpdate(float deltaTime);
}

public class BehaviorManager
{
	private static BehaviorManager instance = null;
	public bool beginning = false;
	public bool middle = false;
	public bool end = false;
	public bool dance = false;

	public static BehaviorManager Instance
	{
		get
		{
			if (instance == null)
				instance = new BehaviorManager();
			return instance;
		}
	}

	protected List<IBehaviorUpdate> receivers = null;

	public BehaviorManager()
	{
		this.receivers = new List<IBehaviorUpdate>();
	}

	public void Register(IBehaviorUpdate receiver)
	{
		this.receivers.Add(receiver);
	}

	/// <summary>
	/// Updates all events and agents for a behavior tick
	/// </summary>
	// TODO: Spread this out across frames do we don't get a chug
	// every time we do a behavior update
	public void Update(float updateTime)
	{
		if(end != true) {
		for (int i = this.receivers.Count - 1; i >= 0; i--)
			if (this.receivers[i].BehaviorUpdate(updateTime) != RunStatus.Running)
				this.receivers.RemoveAt(i);
		}
	}

	/// <summary>
	/// Clears all receivers from this BehaviorManager.
	/// </summary>
	public void ClearReceivers()
	{
		this.receivers.Clear();
	}

	public void changeBeginning()
	{
		if(this.beginning == true)
			this.beginning = false;
		else {
			this.beginning = true;
		}
	}

	public void changeMiddle()
	{
		if(this.middle == true)
			this.middle = false;
		else {
			this.middle = true;
		}
	}

	public void changeEnd()
	{
		if (this.end == true)
			this.end = false;
		else {
			this.end = true;
		}
	}

	public void changeDance()
	{
		if (this.dance == true)
			this.dance = false;
		else {
			this.dance = true;
		}
	}
}
