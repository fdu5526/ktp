using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer {

	float prevActivationTime;
	float cooldownTime;

	public Timer(float c) {
		cooldownTime = c;
		prevActivationTime = -c;
	}
	public float TimeLeft(float time) { return cooldownTime - (time - prevActivationTime); }
	public bool IsOffCooldown(float time) { return time - prevActivationTime > cooldownTime; }
	public float CooldownTime { get { return cooldownTime; } set { cooldownTime = value; } }
	public void Reset(float time) { prevActivationTime = time; }
}
