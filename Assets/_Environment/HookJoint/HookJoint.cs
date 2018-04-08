using System.Collections;
using System.Collections.Generic;
using Randolph.Characters;
using Randolph.Core;
using UnityEngine;

public class HookJoint : MonoBehaviour {
	public void Activate() {
		var player = GameObject.FindGameObjectWithTag(Constants.Tag.Player).GetComponent<PlayerController>();
		player.GrappleTo(gameObject);
	}
}