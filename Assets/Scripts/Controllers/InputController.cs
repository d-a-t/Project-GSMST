using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public interface BoolInput {
	bool GetBool(); 
}

public class BasicInput<T> : Variable<T>, BoolInput {
	public static implicit operator bool(BasicInput<T> self) => self.GetBool();
	public virtual bool GetBool() {
		return true;
	}

	public BasicInput(T value, bool CheckIfSame) : base(value, CheckIfSame) {}

	public BasicInput() : base() {}
}

public class BasicBoolInput : BasicInput<bool> {
	public static implicit operator bool(BasicBoolInput self) => self.GetBool();
	public override bool GetBool() {
		return base.Value;
	}
}


public class BasicInputAction : BasicInput<InputAction.CallbackContext> {
	internal InputAction internalInputAction;
	internal bool BoolVal;
	public override bool GetBool() {
		return BoolVal;
	}

	public BasicInputAction(InputAction action) : base() {
		base.CheckIfSame = false;

		internalInputAction = action;

		action.performed += ctx => {
			base.Value = ctx;
			BoolVal = true;
		};

		action.canceled += ctx => {
			base.Value = ctx;
			BoolVal = false;
		};

	}
}

public class BasicInputDict<T1, T2> : Dictionary<T1, T2> where T2 : BoolInput, new() {
	new public T2 this[T1 key] {
		get {
			if (!base.ContainsKey(key)) {
				base[key] = new T2();
			}
			return base[key];
		}
	}
}

public class BasicInputActionDict : Dictionary<InputAction, BasicInputAction> {
	new public BasicInputAction this[InputAction key] {
		get {
			if (!base.ContainsKey(key)) {
				base[key] = new BasicInputAction(key);
			}
			return base[key];
		}
	}
}

public class SpecialInput<T> where T : BoolInput {
	public List<(T, float)> Inputs;

	internal int index;
	internal float timeLast;
	internal bool canProceed;

	public SpecialInput(List<(T, float)> entry) {
		this.index = 0;
		this.timeLast = Time.time;
		this.canProceed = true;
		this.Inputs = entry;
	}
}


public class KeyStroke {
	public List<(KeyCode, float)> Inputs = new List<(KeyCode, float)>();

	internal int index = 0;
	internal float timeLast = Time.time;
	internal bool canProceed = true;

	public KeyStroke(List<(KeyCode, float)> entry) {
		this.Inputs = entry;
	}
}

public class InputStroke {
	public List<(InputAction, float)> Inputs = new List<(InputAction, float)>();

	internal int index = 0;
	internal float timeLast = Time.time;
	internal bool canProceed = true;

	public InputStroke(List<(InputAction, float)> entry) {
		this.Inputs = entry;
	}
}

public class InputStrokeDict : BasicInputDict<InputStroke, BasicBoolInput> { }

public class Mouse {
	public sealed class ScrollWheel {
		public Variable<float> Delta = new Variable<float>();
	}

	public ScrollWheel Wheel = new ScrollWheel();
	/// <summary>
	/// Defines the realtime mouse position in World.
	/// </summary>
	public Vector2 Position = new Vector2();
}

/*
public abstract class InputControllable<T> {
	public abstract Listener<bool> InputConnect(T input, Func<bool, bool> func);
}


public interface IKeyCodeControllable {
	List<(KeyCode, Listener<bool>)> KeyCodeControls { get; set; }
	Listener<bool> InputConnect(KeyCode keyCode, Func<bool, bool> func);
}

public interface IInputActionControllable {
	List<(InputAction, Listener<bool>)> InputActionControls { get; set; }
	Listener<bool> InputConnect(InputAction inputAction, Func<bool, bool> func);
}

public interface IKeyStrokeControllable {
	List<(KeyStroke, Listener<bool>)> KeyStrokeControls { get; set; }
	Listener<bool> InputConnect(KeyCode KeyStroke, Func<bool, bool> func);
}
*/

public interface IControllable<T> {
	void BindPlayerControls(T controller);
	void UnbindControls();
}

/// <summary>
/// A "static" class that allows functions to be binded whenever an input is fired. 
/// This prevents having to check for a input for every frame, as you can just bind a function to this class to fire whenever a specific input is detected. 
/// In short, you don't need to use Update() every time to check if a input is pressed.
/// </summary>
public sealed class InputController : Singleton {
	public static InputController Singleton;
	public GameObject PlayerInputPrefab;
	public KeyCode lastKey;

	public ConnectedPlayer[] Players;

	public InputDevice[] ValidDevices;

	public UnityEvent OnCompletedBind;

	public static BasicInputDict<BoolInput, BasicBoolInput> CustomInputs = new BasicInputDict<BoolInput, BasicBoolInput>();
	public static BasicInputDict<SpecialInput<BoolInput>, BasicInput<bool>> SpecialInputs = new BasicInputDict<SpecialInput<BoolInput>, BasicInput<bool>>();

	//private static Dictionary<string, Variable<bool>> _Keyboard = new Dictionary<string, Variable<bool>>();

	/// <summary>
	/// A Dictionary that contains a table of inputs from which you can bind a function to fire whenever a specific input is detected.
	/// </summary>
	/// <example>
	/// <code>
	/// InputController.Keyboard[KeyCode.R].Connect((bool val) => {
	/// 	yaadadadadda
	/// 	return true;
	/// })
	/// This binds a function to fire whenever the R button is pressed or released. The val defines if it's pressed (true) or released (false).
	/// </code>
	/// </example>
	public static BasicInputDict<KeyCode, BasicBoolInput> Keyboard = new BasicInputDict<KeyCode, BasicBoolInput>();

	/// <summary>
	/// A Dictionary that contains a table of inputs from which you can bind a function to fire whenever a specific input is detected.
	/// </summary>
	/// <example>
	/// <code>
	/// InputController.Keyboard[KeyCode.R].Connect((bool val) => {
	/// 	yaadadadadda
	/// 	return true;
	/// })
	/// This binds a function to fire whenever the R button is pressed or released. The val defines if it's pressed (true) or released (false).
	/// </code>
	/// </example>
	public static BasicInputDict<KeyStroke, BasicBoolInput> KeyStroke = new BasicInputDict<KeyStroke, BasicBoolInput>();
	
	public static BasicInputActionDict InputAction = new BasicInputActionDict();

	public static Mouse Mouse = new Mouse();

	void OnUnpairedDeviceUsed( InputControl control, InputEventPtr eventPtr ){
		//Debug.Log(control.device.name);
	}

	public void AddPlayer(InputDevice device) {
		for (int i = 0; i < 2; i++) {	
			if (Players[i] == null || Players[i]?.Device == null) {
				Players[i].Device = device;
				Players[i].RebindInputs();
				Players[i].transform.parent = Singleton.transform;
				return;
			}
		}
	}

	public void InputDeviceChanged(InputDevice device, InputDeviceChange change) {
        switch (change) {
            //New device added
            case InputDeviceChange.Added:
                Debug.Log("New device added: " + device.name);
                break;
            //Device disconnected
            case InputDeviceChange.Disconnected:
               //	device.Disconnected.Invoke();
                Debug.Log("Device disconnected");
				for (int i = 0; i < Players.Length; i++) {
					if (device == Players[i].Device) {
						Players[i].Device = null;
						Players[i].lastDevice = device;
					}
				}

                break;
            
            //Familiar device connected
            case InputDeviceChange.Reconnected:
                Debug.Log("Device reconnected");
				for (int i = 0; i < Players.Length; i++) {
					if (device == Players[i].lastDevice) {
						Players[i].Device = device;
					}
				}
                break;
                
            //Else
            default:
                break;
        }
    
	}

	public void Awake() {
		if (Singleton == null) {
			Singleton = this;
		}
	}

	public void Start() {
		InputSystem.onDeviceChange += InputDeviceChanged;

		foreach (InputDevice device in InputSystem.devices) {
			if (device != null && device.name != "Mouse" && device.name != "Keyboard") {
				AddPlayer(device);
			}
		}
		OnCompletedBind.Invoke();

		/*
		Listener<float> updateKeyCodes = Runservice.BindToUpdate(Global.RunservicePriority.Heartbeat.Input, (dt) => {
			KeyCode[] copy = new KeyCode[Keyboard.Keys.Count];
			Keyboard.Keys.CopyTo(copy, 0);

			foreach (KeyCode key in copy) {
				if (Input.GetKeyDown(key) && !Keyboard[key].Value) {
					Keyboard[key].Value = true;
					lastKey = key;
				} else if (Input.GetKeyUp(key) && Keyboard[key].Value) {
					Keyboard[key].Value = false;
				}
			}
			return true;
		});
		updateKeyCodes.Name = "updateKeyCodes";
		Maid.GiveTask(updateKeyCodes);

		Listener<float> updateKeyStroke = Runservice.BindToUpdate(Global.RunservicePriority.Heartbeat.Input, (dt) => {
			KeyStroke[] copy = new KeyStroke[KeyStroke.Keys.Count];
			KeyStroke.Keys.CopyTo(copy, 0);

			foreach (KeyStroke key in copy) {
				if (key.index == 0) {
					key.timeLast = Time.time;
				}
				if (key.index < key.Inputs.Count) {
					(KeyCode, float) current = key.Inputs[key.index];

					if (key.canProceed) {
						if (InputController.Keyboard[current.Item1].Value) {
							if (Time.time - key.timeLast < current.Item2) {
								if  (key.index < key.Inputs.Count-1) {
									if (key.Inputs[key.index].Item1 == key.Inputs[key.index+1].Item1) {
										key.canProceed = false;
										InputController.Keyboard[key.Inputs[key.index].Item1].Connect((bool val)=> {
											if (!val) {
												key.canProceed = true;
												return false;
											}
											return true;
										});
									}
								}
								key.index++;
							} else {
								key.timeLast = Time.time;
								key.index = 0;
							}
						}
					}
				} else {
					key.index = 0;
					KeyStroke[key].Value = true;

					//Getting last key in keystroke to connect a function that activates when last key pressed up to say let go of keystroke.
					Keyboard[key.Inputs[key.Inputs.Count-1].Item1].Connect((bool val) => {
						if (!val) {
							KeyStroke[key].Value = false;
							return false;
						}
						return true;
					});
				}
			}
			return true;
		});
		updateKeyStroke.Name = "updateKeyStroke";
		Maid.GiveTask(updateKeyStroke);


		Listener<float> updateCustomInputs = Runservice.BindToUpdate(Global.RunservicePriority.Heartbeat.Input+1, (dt) => {
			BasicBoolInput[] copy = new BasicBoolInput[CustomInputs.Values.Count];
			CustomInputs.Values.CopyTo(copy, 0);
			
			foreach (BasicBoolInput val in copy) {
				CustomInputs[val].Value = val.Value;
			}

			foreach (BasicInput<bool> val in SpecialInputs.Values) {
				CustomInputs[val].Value = val.Value;
			}

			foreach (BasicBoolInput val in Keyboard.Values) {
				CustomInputs[val].Value = val.Value;
			}

			foreach (BoolInput val in InputAction.Values) {
				CustomInputs[val].Value = val.GetBool();
			}

			return true;
		});
		updateCustomInputs.Name = "updateCustomInputs";
		Maid.GiveTask(updateCustomInputs);

		
		Listener<float> updateSpecialInputs = Runservice.BindToUpdate(Global.RunservicePriority.Heartbeat.Input+2, (dt) => {
			SpecialInput<BoolInput>[] copy = new SpecialInput<BoolInput>[SpecialInputs.Keys.Count];
			SpecialInputs.Keys.CopyTo(copy, 0);

			foreach (SpecialInput<BoolInput> key in copy) {
				if (key.index == 0) {
					key.timeLast = Time.time;
				}
				if (key.index < key.Inputs.Count) {
					(BoolInput, float) current = key.Inputs[key.index];

					BoolInput InputKey = current.Item1;
					float time_dt = current.Item2;

					if (key.canProceed) {
						if (InputKey.GetBool()) {
							if (Time.time - key.timeLast < current.Item2) {
								if  (key.index < key.Inputs.Count-1) {
									if (InputKey == key.Inputs[key.index+1].Item1) {
										key.canProceed = false;

										//Getting original key inputs since this is a copy.
										InputController.CustomInputs[InputKey].Connect((bool val)=> {
											if (val) {
												key.canProceed = true;
												return false;
											}
											return true;
										});
									}
								}
								key.index++;
							} else {
								key.timeLast = Time.time;
								key.index = 0;
							}
						}
					}
				} else {
					key.index = 0;
					InputController.SpecialInputs[key].Value = true;

					//Getting last key in keystroke to connect a function that activates when last key pressed up to say let go of keystroke.
					InputController.CustomInputs[key.Inputs[key.Inputs.Count-1].Item1].Connect((bool val) => {
						if (!val) {
							InputController.SpecialInputs[key].Value = false;
							return false;
						}
						return true;
					});
				}
			}
			return true;
		});
		updateSpecialInputs.Name = "updateSpecialInputs";
		Maid.GiveTask(updateSpecialInputs);
		*/
	}


	public void Update() {
			KeyCode[] copy = new KeyCode[Keyboard.Keys.Count];
			Keyboard.Keys.CopyTo(copy, 0);

			foreach (KeyCode key in copy) {
				if (Input.GetKeyDown(key) && !Keyboard[key].Value) {
					Keyboard[key].Value = true;
					lastKey = key;
				} else if (Input.GetKeyUp(key) && Keyboard[key].Value) {
					Keyboard[key].Value = false;
				}
			}

			KeyStroke[] copy1 = new KeyStroke[KeyStroke.Keys.Count];
			KeyStroke.Keys.CopyTo(copy1, 0);

			foreach (KeyStroke key in copy1) {
				if (key.index == 0) {
					key.timeLast = Time.time;
				}
				if (key.index < key.Inputs.Count) {
					(KeyCode, float) current = key.Inputs[key.index];

					if (key.canProceed) {
						if (InputController.Keyboard[current.Item1].Value) {
							if (Time.time - key.timeLast < current.Item2) {
								if  (key.index < key.Inputs.Count-1) {
									if (key.Inputs[key.index].Item1 == key.Inputs[key.index+1].Item1) {
										key.canProceed = false;
										InputController.Keyboard[key.Inputs[key.index].Item1].Connect((bool val)=> {
											if (!val) {
												key.canProceed = true;
												return false;
											}
											return true;
										});
									}
								}
								key.index++;
							} else {
								key.timeLast = Time.time;
								key.index = 0;
							}
						}
					}
				} else {
					key.index = 0;
					KeyStroke[key].Value = true;

					//Getting last key in keystroke to connect a function that activates when last key pressed up to say let go of keystroke.
					Keyboard[key.Inputs[key.Inputs.Count-1].Item1].Connect((bool val) => {
						if (!val) {
							KeyStroke[key].Value = false;
							return false;
						}
						return true;
					});
				}
			}

			BasicBoolInput[] copy2 = new BasicBoolInput[CustomInputs.Values.Count];
			CustomInputs.Values.CopyTo(copy2, 0);
			
			foreach (BasicBoolInput val in copy2) {
				CustomInputs[val].Value = val.Value;
			}

			foreach (BasicInput<bool> val in SpecialInputs.Values) {
				CustomInputs[val].Value = val.Value;
			}

			foreach (BasicBoolInput val in Keyboard.Values) {
				CustomInputs[val].Value = val.Value;
			}

			foreach (BoolInput val in InputAction.Values) {
				CustomInputs[val].Value = val.GetBool();
			}

			SpecialInput<BoolInput>[] copy3 = new SpecialInput<BoolInput>[SpecialInputs.Keys.Count];
			SpecialInputs.Keys.CopyTo(copy3, 0);

			foreach (SpecialInput<BoolInput> key in copy3) {
				if (key.index == 0) {
					key.timeLast = Time.time;
				}
				if (key.index < key.Inputs.Count) {
					(BoolInput, float) current = key.Inputs[key.index];

					BoolInput InputKey = current.Item1;
					float time_dt = current.Item2;

					if (key.canProceed) {
						if (InputKey.GetBool()) {
							if (Time.time - key.timeLast < current.Item2) {
								if  (key.index < key.Inputs.Count-1) {
									if (InputKey == key.Inputs[key.index+1].Item1) {
										key.canProceed = false;

										//Getting original key inputs since this is a copy.
										InputController.CustomInputs[InputKey].Connect((bool val)=> {
											if (val) {
												key.canProceed = true;
												return false;
											}
											return true;
										});
									}
								}
								key.index++;
							} else {
								key.timeLast = Time.time;
								key.index = 0;
							}
						}
					}
				} else {
					key.index = 0;
					InputController.SpecialInputs[key].Value = true;

					//Getting last key in keystroke to connect a function that activates when last key pressed up to say let go of keystroke.
					InputController.CustomInputs[key.Inputs[key.Inputs.Count-1].Item1].Connect((bool val) => {
						if (!val) {
							InputController.SpecialInputs[key].Value = false;
							return false;
						}
						return true;
					});
				}
			}

	}

	/// <summary>
	/// Disconnects every binded function and itself. Don't run this unless you're planning to quit the game.
	/// </summary>
	public override void Dispose() {
		Maid.Dispose();
	}
}