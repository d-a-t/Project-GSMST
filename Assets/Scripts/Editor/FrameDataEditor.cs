using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

[CustomEditor(typeof(FrameData))]
public class FrameDataEditor : Editor {
	//TODO check how to save this when unfocusing
	private bool showHitboxesGUI = true;

	int animationClipsIndex;
	int previousClipsIndex;
	int keyframesIndex;

	private TargetComponents targetComponents;
	private FrameDataClipsFeeder frameDataClipsFeeder;

	private static GUIStyle hitboxBackgroundStyle;

	#region Drawing Methods
	public override void OnInspectorGUI() {
		GUILayout.BeginVertical();
		GUI.skin.label.wordWrap = true;

		AAnimationClip currentClip = null;
		AKeyframe currentKeyframe = null;

		if (targetComponents.FrameData.AnimationClipsStringList != null && targetComponents.FrameData.AnimationClips.Length > 0) {
			DrawAnimationClips();
		} else {
			GUILayout.Label("You don't have any animation clips in your Animator component. Add some so we can show them.");

			GUILayout.EndVertical();
			return;
		}

		if (targetComponents.FrameData.HasLists) {

			currentClip = targetComponents.FrameData.AnimationClips[animationClipsIndex];
			currentKeyframe = currentClip.keyframes[keyframesIndex];

			DrawFrames(currentClip, currentKeyframe);
		}

		showHitboxesGUI = EditorGUILayout.Foldout(showHitboxesGUI, "Hitboxes");

		if (showHitboxesGUI) {
			DrawHitboxes(currentKeyframe);
		}

		GUILayout.EndVertical();
	}

	void OnSceneGUI() {
		serializedObject.Update();

		if (targetComponents.FrameData.AnimationClips != null) {
			AKeyframe currentKeyframe;

			currentKeyframe = targetComponents.FrameData.AnimationClips[animationClipsIndex].keyframes[keyframesIndex];

			if (currentKeyframe.hitboxes.Length > 0) {
				for (int i = 0; i < currentKeyframe.hitboxes.Length; i++) {
					Hitbox currentHitbox = currentKeyframe.hitboxes[i];

					currentKeyframe.hitboxes[i] = Hitbox.ShapeDictionary[currentHitbox.Shape]
														.DrawSceneHandle(currentHitbox, targetComponents);
				}
			}
		}

		EditorUtility.SetDirty(target);
		serializedObject.ApplyModifiedProperties();
	}

	private void DrawAnimationClips() {
		animationClipsIndex = EditorGUILayout.Popup("Animation Clip", animationClipsIndex, targetComponents.FrameData.AnimationClipsStringList);

		//If different then a change was made(another animation clip was selected)
		if (previousClipsIndex != animationClipsIndex) {
			previousClipsIndex = animationClipsIndex;
			keyframesIndex = 0;
		}

		GUILayout.BeginVertical();
		if (GUILayout.Button("Refresh Animations", GUILayout.Width(260))) {
			frameDataClipsFeeder.ReloadAnimationClips(targetComponents.Animator);
		}
		GUILayout.EndVertical();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Add Keyframe", GUILayout.Width(130))) {
			Array.Resize(ref targetComponents.FrameData.AnimationClips[animationClipsIndex].keyframes,
						 targetComponents.FrameData.AnimationClips[animationClipsIndex].keyframes.Length + 1);
			targetComponents.FrameData.AnimationClips[animationClipsIndex].keyframes[targetComponents.FrameData.AnimationClips[animationClipsIndex].keyframes.Length - 1] = new AKeyframe(null);
		}
		if (GUILayout.Button("Remove Keyframe", GUILayout.Width(130))) {
			AKeyframe[] copy = new AKeyframe[targetComponents.FrameData.AnimationClips[animationClipsIndex].keyframes.Length-1];

			int b = 0;
			for (int i = 0; i < targetComponents.FrameData.AnimationClips[animationClipsIndex].keyframes.Length; i++) {
				if (i != keyframesIndex) {
					copy[b] = targetComponents.FrameData.AnimationClips[animationClipsIndex].keyframes[i];
					b++;
				}
			}

			targetComponents.FrameData.AnimationClips[animationClipsIndex].keyframes = copy;
			keyframesIndex--;
			if (keyframesIndex < 0) {
				keyframesIndex = 0;
			}
		}
		GUILayout.EndHorizontal();
	}
	private void DrawFrames(AAnimationClip currentClip, AKeyframe currentKeyframe) {
		GUILayout.BeginHorizontal();
		GUILayout.Label("Sprite", GUILayout.Width(70));
		currentKeyframe.Sprite = EditorGUILayout.ObjectField(currentKeyframe.Sprite, typeof(Sprite), true) as Sprite;
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Frame " + (keyframesIndex + 1) + "/" +
										 currentClip.keyframes.Length);

		if (GUILayout.Button("<", GUILayout.Width(30))) {
			keyframesIndex--;
			if (keyframesIndex < 0)
				keyframesIndex = currentClip.keyframes.Length - 1;
		}
		if (GUILayout.Button(">", GUILayout.Width(30))) {
			keyframesIndex++;
			if (keyframesIndex >= currentClip.keyframes.Length) {
				keyframesIndex = 0;
			}
		}
		
		GUILayout.EndHorizontal();

		GUILayout.BeginVertical();
		SerializedProperty prop = serializedObject.FindProperty("AnimationClips")?.GetArrayElementAtIndex(animationClipsIndex);
		if (prop != null) {
			serializedObject.Update();

			EditorGUILayout.PropertyField(prop.FindPropertyRelative("OnHitboxCollisionEnter"));
			EditorGUILayout.PropertyField(prop.FindPropertyRelative("OnHitboxCollisionStay"));
			EditorGUILayout.PropertyField(prop.FindPropertyRelative("OnHitboxCollisionExit"));
			EditorUtility.SetDirty(target);
			serializedObject.ApplyModifiedProperties();
		}
		EditorGUILayout.EndVertical();
	}
	private void DrawHitboxes(AKeyframe currentKeyframe) {
		if (currentKeyframe == null)
			return;

		if (currentKeyframe.hitboxes != null && currentKeyframe.hitboxes.Length > 0) {

			for (int i = 0; i < currentKeyframe.hitboxes.Length; i++) {
				GUILayout.BeginVertical(hitboxBackgroundStyle);
				GUILayout.Label("Hitbox " + i);

				GUILayout.BeginHorizontal();
				currentKeyframe.hitboxes[i].Type = (HitboxType)EditorGUILayout.EnumPopup(currentKeyframe.hitboxes[i].Type, GUILayout.Width(100));
				currentKeyframe.hitboxes[i].Shape = (HitboxShape)EditorGUILayout.EnumPopup(currentKeyframe.hitboxes[i].Shape, GUILayout.Width(100));
				GUILayout.EndHorizontal();

				if (currentKeyframe.hitboxes[i].Shape == HitboxShape.Rectangle) {
					currentKeyframe.hitboxes[i].Boundaries = EditorGUILayout.RectField(currentKeyframe.hitboxes[i].Boundaries);
				} else {
					//currentKeyframe.hitboxes[i].Boundaries = EditorGUILayout.FloatField("Radius ", currentKeyframe.hitboxes[i].Boundaries);
				}

				GUILayout.BeginHorizontal();
				GUILayout.Label("", GUILayout.ExpandWidth(true));
				if (GUILayout.Button("Remove", GUILayout.Width(60))) {
					for (int j = i; j < currentKeyframe.hitboxes.Length - 1; j++) {
						currentKeyframe.hitboxes[j] = currentKeyframe.hitboxes[j + 1];
					}

					Array.Resize(ref currentKeyframe.hitboxes, currentKeyframe.hitboxes.Length - 1);
					SceneView.RepaintAll();
				}
				GUILayout.EndHorizontal();
				GUILayout.EndVertical();

				GUILayout.BeginVertical(GUILayout.Height(10));
				GUILayout.Label("", GUILayout.Height(10));
				GUILayout.EndVertical();
			}
		}

		if (currentKeyframe.hitboxes.Length == 0 && GUILayout.Button("Copy previous frame Hitboxes", GUILayout.Width(250))) {
			if (keyframesIndex != 0) {
				Hitbox[] hitboxes = targetComponents.FrameData.AnimationClips[animationClipsIndex].keyframes[keyframesIndex - 1].hitboxes.Clone() as Hitbox[];
				Array.Resize(ref currentKeyframe.hitboxes, hitboxes.Length);

				for (int i = 0; i < hitboxes.Length; i++) {
					Hitbox hitbox = new Hitbox(hitboxes[i].Boundaries, hitboxes[i].Type);
					hitbox.Shape = hitboxes[i].Shape;
					hitbox.Boundaries = hitboxes[i].Boundaries;

					currentKeyframe.hitboxes[i] = hitbox;
				}

				SceneView.RepaintAll();
			} else {
				Debug.LogError("You are on the first frame, can't copy previous frame Hitboxes!");
			}
		}

		if (GUILayout.Button("Add Hitbox", GUILayout.Width(100))) {
			Array.Resize(ref currentKeyframe.hitboxes, currentKeyframe.hitboxes.Length + 1);
			currentKeyframe.hitboxes[currentKeyframe.hitboxes.Length - 1] = new Hitbox(new Rect(0f, 0f, 1f, 1.5f), HitboxType.Hurtbox);

			SceneView.RepaintAll();
		}
	}
	private Texture2D MakeTexture(int width, int height, Color col) {
		Color[] pix = new Color[width * height];

		for (int i = 0; i < pix.Length; i++)
			pix[i] = col;

		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();

		return result;
	}
	#endregion

	void OnEnable() {
		EditorApplication.update += OnEditorUpdate;

		if (hitboxBackgroundStyle == null) {
			hitboxBackgroundStyle = new GUIStyle();
			hitboxBackgroundStyle.normal.background = MakeTexture(1, 1, new Color(0f, 0f, 0f, 0.2f));
		}
		//When changing scenes the background is deleted
		else if (hitboxBackgroundStyle.normal.background == null) {
			hitboxBackgroundStyle.normal.background = MakeTexture(1, 1, new Color(0f, 0f, 0f, 0.2f));
		}

		targetComponents = new TargetComponents(target);
		frameDataClipsFeeder = new FrameDataClipsFeeder(targetComponents.FrameData);

		frameDataClipsFeeder.FeedAnimationClips(targetComponents.Animator);

		//TODO maybe get previously set value?
		keyframesIndex = 0;
		animationClipsIndex = 0;
	}

	private void OnLostFocus() {
		EditorApplication.update -= OnEditorUpdate;

		if (targetComponents.SpriteRenderer != null) {
			//TODO check why not working
			targetComponents.SpriteRenderer.sprite = targetComponents.spriteWhenGotFocus;
			targetComponents = null;
			/*
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
            */
		}
	}

	private void OnEditorUpdate() {
		if (targetComponents.FrameData == null) {
			OnLostFocus();
			return;
		}

		if (!targetComponents.FrameData.enabled)
			return;

		//TODO: check if these 2 lines don't slow down performance
		//(they are needed to update the values back to the monobehaviour, maybe use serializedproperties)
		targetComponents.FrameData.AnimationClipsIndex = animationClipsIndex;
		targetComponents.FrameData.KeyframesIndex = keyframesIndex;

		if (Selection.activeGameObject != targetComponents.GameObject) {
			OnLostFocus();
			return;
		}

		//if (targetSpriteRenderer != null && targetAlphaHitbox.AnimationClips != null)
		if (targetComponents.FrameData.HasLists && targetComponents.SpriteRenderer != null) {
			if (targetComponents.FrameData.AnimationClips[animationClipsIndex].keyframes[keyframesIndex].Sprite != null)
				targetComponents.SpriteRenderer.sprite = targetComponents.FrameData.AnimationClips[animationClipsIndex].keyframes[keyframesIndex].Sprite;
		}
	}
}

public class FrameDataClipsFeeder {
	private FrameData FrameData;

	public FrameDataClipsFeeder(FrameData FrameData) {
		this.FrameData = FrameData;
	}

	private void FeedClipsFromAnimator(Animator animator) {
		if (animator != null) {
			for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++) {
				if (FrameData.AnimationClips[i].keyframes == null || FrameData.AnimationClips[i].keyframes.Length == 0) {
					AnimationClip clip = animator.runtimeAnimatorController.animationClips[i];

					//targetAlphaHitbox.AnimationClips.Add(new AAnimationClip(clip.name));
					FrameData.AnimationClips[i] = new AAnimationClip(clip.name);

					var bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
					for (int j = 0; j < bindings.Length; j++) {
						ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, bindings[j]);
						//targetAlphaHitbox.AnimationClips[i].keyframes = new List<AKeyframe>();
						FrameData.AnimationClips[i].keyframes = new AKeyframe[keyframes.Length];

						for (int k = 0; k < keyframes.Length; k++) {
							AKeyframe keyframe = new AKeyframe(keyframes[k].value as Sprite);
							//targetAlphaHitbox.AnimationClips[i].keyframes.Add(keyframe);
							FrameData.AnimationClips[i].keyframes[k] = keyframe;
						}
					}
				}
			}
		}
	}

	private void FeedClipsStringFromAnimator(Animator animator) {
		if (animator != null) {
			for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++) {
				FrameData.AnimationClipsStringList[i] = animator.runtimeAnimatorController.animationClips[i].name;
			}
		}
	}

	public void FeedAnimationClips(Animator animator) {
		if ((FrameData.AnimationClips == null || FrameData.AnimationClips.Length == 0)
			&& animator != null
			&& animator.runtimeAnimatorController != null) {
			FrameData.AnimationClips = new AAnimationClip[animator.runtimeAnimatorController.animationClips.Length];
			FeedClipsFromAnimator(animator);

			FrameData.AnimationClipsStringList = new string[animator.runtimeAnimatorController.animationClips.Length];
			FeedClipsStringFromAnimator(animator);

			return;
		}
		
		if (FrameData.AnimationClips.Length < animator.runtimeAnimatorController.animationClips.Length) {
			Array.Resize(ref FrameData.AnimationClips, animator.runtimeAnimatorController.animationClips.Length);
			FeedClipsFromAnimator(animator);

			FrameData.AnimationClipsStringList = new string[animator.runtimeAnimatorController.animationClips.Length];
			FeedClipsStringFromAnimator(animator);
			return;
		}
	}

	public void ReloadAnimationClips(Animator animator) {
		
			Array.Resize(ref FrameData.AnimationClips, animator.runtimeAnimatorController.animationClips.Length);
			FeedClipsFromAnimator(animator);

			FrameData.AnimationClipsStringList = new string[animator.runtimeAnimatorController.animationClips.Length];
			FeedClipsStringFromAnimator(animator);
	
	}
}