using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum Controls {
	UP, DOWN, LEFT, RIGHT, LIGHT, MEDIUM, HEAVY
}

public class KeyBindUpdater : MonoBehaviour {
	public Text UP;
	public Text DOWN;
	public Text LEFT;
	public Text RIGHT;
	public Text LIGHT;
	public Text MEDIUM;
	public Text HEAVY;
	[SerializeField] private InputActionReference jumpAction = null;
	private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

	public void StartRebinding() {
		rebindingOperation = jumpAction.action.PerformInteractiveRebinding()
			.WithControlsExcluding("Mouse")
			.OnMatchWaitForAnother(0.1f)
			.OnComplete(operation => RebindComplete())
			.Start();
	}
    public void RebindComplete() {

    }

	public void UpdateKeybind(string control) {
		UpdateKeybind((Controls)System.Enum.Parse(typeof(Controls), control));
	}

	public void UpdateKeybind(Controls control) {
		int val = 1;
		switch (control) {
			case Controls.UP: {

					UP.text = val.ToString();
					PlayerPrefs.SetString("UP", val.ToString());
					break;
				}
			case Controls.DOWN: {
					DOWN.text = val.ToString();
					PlayerPrefs.SetString("DOWN", val.ToString());
					break;
				}
			case Controls.LEFT: {
					LEFT.text = val.ToString();
					PlayerPrefs.SetString("LEFT", val.ToString());
					break;
				}
			case Controls.RIGHT: {
					RIGHT.text = val.ToString();
					PlayerPrefs.SetString("RIGHT", val.ToString());
					break;
				}
			case Controls.LIGHT: {
					LIGHT.text = val.ToString();
					PlayerPrefs.SetString("ATTACK", val.ToString());
					break;
				}
			case Controls.MEDIUM: {
					MEDIUM.text = val.ToString();
					PlayerPrefs.SetString("SHOOT", val.ToString());
					break;
				}
			case Controls.HEAVY: {
					HEAVY.text = val.ToString();
					PlayerPrefs.SetString("RUN", val.ToString());
					break;
				}
		}
	}

	// Start is called before the first frame update
	public void Start() {
		if (!PlayerPrefs.HasKey("UP")) {
			PlayerPrefs.SetString("UP", "W");
		}
		if (!PlayerPrefs.HasKey("DOWN")) {
			PlayerPrefs.SetString("DOWN", "S");
		}
		if (!PlayerPrefs.HasKey("LEFT")) {
			PlayerPrefs.SetString("LEFT", "A");
		}
		if (!PlayerPrefs.HasKey("RIGHT")) {
			PlayerPrefs.SetString("RIGHT", "D");
		}
		if (!PlayerPrefs.HasKey("SHOOT")) {
			PlayerPrefs.SetString("SHOOT", "Mouse0");
		}
		if (!PlayerPrefs.HasKey("ATTACK")) {
			PlayerPrefs.SetString("ATTACK", "Space");
		}
		if (!PlayerPrefs.HasKey("RUN")) {
			PlayerPrefs.SetString("RUN", "LeftShift");
		}

		UP.text = PlayerPrefs.GetString("UP", "W");
		LEFT.text = PlayerPrefs.GetString("LEFT", "A");
		DOWN.text = PlayerPrefs.GetString("DOWN", "S");
		RIGHT.text = PlayerPrefs.GetString("RIGHT", "D");

		//ATTACK.text = PlayerPrefs.GetString("ATTACK", "Space");
		//SHOOT.text = PlayerPrefs.GetString("SHOOT", "Mouse0");
		//RUN.text = PlayerPrefs.GetString("RUN", "LeftShift");
	}


}
