using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CheckDeviceType : MonoBehaviour {
	public ConnectedPlayer Player;
	public Text theText;
	void Start() {
		InputController.Singleton.OnCompletedBind.AddListener(
			delegate {
				if (Player.Device != null) {
					theText.text = Player.Device.name;
				}
			}
		);
	}

	// Update is called once per frame
	void Update() {

	}
}
