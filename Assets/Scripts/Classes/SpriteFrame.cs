using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// A class that attaches itself to a empty gameObject. Makes it easy to toggle if it should render.
/// </summary>
public class SpriteFrame : MonoBehaviour, IDisposable {
	public Maid Maid = new Maid();

	public CFrame CFrame {
		get { return gameObject.transform.GetCFrame(); }
		set { gameObject.transform.UpdateFromCFrame(value); }
	}

	[Header("Rendering")]
	private bool _FlipX = false;
	private bool _FlipY = false;
	public bool FlipX {
		get { return _FlipX; }
		set {
			if (_FlipX != value) {
				_FlipX = value;
				FlipSpriteX(value);
			}
		}
	}
	
	public bool FlipY {
		get { return _FlipY; }
		set {
			if (_FlipX != value) {
				_FlipY = value;
				FlipSpriteY(value);
			}
		}
	}

	private void FlipSpriteX(bool val) {
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

	private void FlipSpriteY(bool val) {
		transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
	}

	public virtual void Awake() {
		Maid.GiveTask(gameObject);
	}

	public virtual void Dispose() {
		Maid.Dispose();
		if (this && this.gameObject) {
			GameObject.Destroy(this.gameObject);
		}
	}

	/// <summary>
	/// This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy() {
		Maid.Dispose();
	}
}
