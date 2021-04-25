﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager S;

    // played once upon building.
    public AudioClip TurretBuildSFX;

    // turret fire sfx
    public AudioClip StrawTurretFireSFX;
    public AudioClip WoodTurretFireSFX;
    public AudioClip BrickTurretFireSFX;

    // enemy attack sfx
    public AudioClip WolfAttackSFX;
    public AudioClip BearAttackSFX;
    public AudioClip FoxAttackSFX;

    // enemy hit sfx
    public AudioClip WolfHitSFX;
    public AudioClip BearHitSFX;
    public AudioClip FoxHitSFX;

    // enemy death sfx
    public AudioClip WolfDeathSFX;
    public AudioClip BearDeathSFX;
    public AudioClip FoxDeathSFX;

    private AudioSource audio;

    private void Awake()
    {
        // Singleton Definition
        if (SoundManager.S)
        {
            // singleton exists, delete this object
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void MakeFireTurretSound(Material mat)
    {
        switch (mat)
        {
            case Material.STRAW:
                audio.PlayOneShot(StrawTurretFireSFX);
                break;
            case Material.WOOD:
                audio.PlayOneShot(WoodTurretFireSFX);
                break;
            case Material.BRICK:
                audio.PlayOneShot(BrickTurretFireSFX);
                break;
            default:
                audio.PlayOneShot(StrawTurretFireSFX);
                break;
        }
    }
}