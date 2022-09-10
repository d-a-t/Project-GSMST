using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heat : MonoBehaviour
{
	public Text Numerical;
	public Slider Slider;
	public Image SliderImage;
	public Image[] Backfills;
	public Color32[] Colors;

	public float Value = 1;

	void UpdateMeter(float val) {
		int roundDown = Mathf.FloorToInt(val);
		Color32 color = Colors[roundDown];

		foreach(Image image in Backfills) {
			image.color = color;
		}

		Slider.enabled = !(val - roundDown == 0);
		
		Numerical.text = roundDown.ToString();
		Slider.value = val - roundDown;

		if (roundDown < Colors.Length) {
			SliderImage.color = Colors[roundDown + 1];
		}
	}

    void Update()
    {
		UpdateMeter(Value);
	}
}
