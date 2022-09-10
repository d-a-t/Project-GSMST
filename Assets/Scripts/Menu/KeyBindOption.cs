using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class KeyBindOption : MonoBehaviour {
	public ConnectedPlayer Player;
	public InputActionReference ActionToBind;
	public Text BindingDisplayNameText;
	public Button Button;
	public InputActionRebindingExtensions.RebindingOperation rebindingOperation;

	private int BindingIndex;

	void Start() {
		InputController.Singleton.OnCompletedBind.AddListener(
			delegate {
				Button.onClick.AddListener(StartRebinding);
				
				if (Player.Device is Gamepad) {
					BindingIndex = 1;
				} else {
					BindingIndex = 0;
				}

				string newBinds = PlayerPrefs.GetString("RebindsKey", null);
				if (newBinds != null && newBinds.Length > 0) {
					Player.Input.actions.LoadFromJson(newBinds);
				}

				BindingDisplayNameText.text = InputControlPath.ToHumanReadableString(
					Player.Input.actions.FindAction(ActionToBind.name).bindings[BindingIndex].effectivePath,
					InputControlPath.HumanReadableStringOptions.OmitDevice);
			}
		);
	}

	public void Save() {
		string rebinds = Player.Input.actions.ToJson();
		PlayerPrefs.SetString("RebindsKey", rebinds);
	}

	public void StartRebinding() {
		EventSystem.current.SetSelectedGameObject(null);
		Player.Input.actions.FindAction(ActionToBind.name).Disable();

		BindingDisplayNameText.text = "Listening";
		rebindingOperation = Player.Input.actions.FindAction(ActionToBind.name).PerformInteractiveRebinding()
			.WithControlsExcluding("Mouse")
			.WithControlsHavingToMatchPath(Player.Device.path)
			.OnMatchWaitForAnother(0.1f)
			.OnComplete(operation => RebindComplete())
			.Start();
	}

	public void RebindComplete() {
		BindingDisplayNameText.text = InputControlPath.ToHumanReadableString(
			Player.Input.actions.FindAction(ActionToBind.name).bindings[BindingIndex].effectivePath,
			InputControlPath.HumanReadableStringOptions.OmitDevice);

		rebindingOperation.Dispose();
        Player.Input.actions.FindAction(ActionToBind.name).Enable();

		Save();
	}

}
