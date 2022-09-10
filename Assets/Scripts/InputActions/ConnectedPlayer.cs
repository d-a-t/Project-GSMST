using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class ConnectedPlayer : MonoBehaviour {
	public InputActionsClasses.Default InputActionDefault;
	public InputControlScheme ControlScheme;

	public InputDevice Device;
	public PlayerInput Input;

	public InputDevice lastDevice;

    public void Awake() {
        InputActionDefault = new InputActionsClasses.Default();
    }

	public void Start() {
		InputActionDefault.Enable();
		if (Device != null) {
			Input.user.AssociateActionsWithUser(InputActionDefault);
            Input.user.ActivateControlScheme(ControlScheme);
		}
	}

	public void RebindInputs() {
		InputActionDefault.devices = new[] { Device };

		string newBinds = PlayerPrefs.GetString("RebindsKey", null);

		if (newBinds != null && newBinds.Length > 0) {
			Input.actions.LoadFromJson(newBinds);
		}

		Input = PlayerInput.Instantiate(this.gameObject, -1, ControlScheme.name, -1, Device);
	}
}