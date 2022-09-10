using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// This takes a Character and binds controls onto them.
/// </summary>
public sealed class PlayerController : Singleton {
	public static PlayerController Singleton;
	public Variable<int> Score = new Variable<int>();

	public Fighter Player1;
	public Fighter Player2;

	[Header("Game User Interface")]
	public Canvas GUI;
	public Slider HealthBar1;
	public Heat Heat1;

	public Slider HealthBar2;
	public Heat Heat2;

	public void RebindAllControls() {
		Player1.UnbindControls();
		Player2.UnbindControls();

		Player1.BindPlayerControls(InputController.Singleton.Players[0].InputActionDefault);
		Player2.BindPlayerControls(InputController.Singleton.Players[1].InputActionDefault);
	}

	public void RebindPlayerWithThisControl(InputActionsClasses.Default oldControl, InputActionsClasses.Default newControl) { 
		if (Player1.ControlledInputAction == oldControl) {
			Player1.UnbindControls();
			Player1.BindPlayerControls(newControl);
		}
		if (Player2.ControlledInputAction == oldControl) {
			Player2.UnbindControls();
			Player2.BindPlayerControls(newControl);
		}
	}

	void Awake() {
		if (Singleton == null) {
			Singleton = this;
		}
	}

	public void Start() {
		if (Player1) {
			if (HealthBar1) {
				HealthBar1.maxValue = Player1.MaxHealth;
				HealthBar1.value = Player1.Health.Value;
				Player1.Maid.GiveTask(
						Player1.Health.Connect(
						(float val) => {
							HealthBar1.value = val;
							if (val <= 0) {
								SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
							}
							return true;
						}
					)
				);

				Heat1.Value = Player1.Heat.Value;
				Player1.Maid.GiveTask(
						Player1.Heat.Connect(
						(float val) => {
							Heat1.Value = val;
							return true;
						}
					)
					);
			}
		}

		if (Player2) {			
			if (HealthBar2) {
				HealthBar2.maxValue = Player2.MaxHealth;
				HealthBar2.value = Player2.Health.Value;

				Player2.Maid.GiveTask(
						Player2.Health.Connect(
						(float val) => {
							HealthBar2.value = val;
							if (val <= 0) {
								SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
							}
							return true;
						}
					)
				);

				Heat2.Value = Player2.Heat.Value;
				Player2.Maid.GiveTask(
					Player2.Heat.Connect(
						(float val) => {
							Heat2.Value = val;
							return true;
						}
					)
				);
			}
		}
	}

	public override void Dispose() {
		Maid.Dispose();
	}
}
