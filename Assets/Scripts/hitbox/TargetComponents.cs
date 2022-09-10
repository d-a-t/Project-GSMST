using UnityEngine;
using System.Collections.Generic;
using System;

public class TargetComponents
{
    public TargetComponents(UnityEngine.Object target)
    {
        FrameData = (target as FrameData);
        GameObject = FrameData.gameObject;

        Animator = FrameData.GetComponent<Animator>();

        SpriteRenderer = GameObject.GetComponent<SpriteRenderer>();
        spriteWhenGotFocus = SpriteRenderer.sprite;
    }

    public GameObject GameObject { get; set; }
    public FrameData FrameData { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }
    public Sprite spriteWhenGotFocus { get; set; } //TODO always has to get this in OnEnable, even if targetcomponents are the same
    public Animator Animator { get; set; }
}