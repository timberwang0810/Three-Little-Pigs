using System.Collections;
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
    public AudioClip[] WolfAttackSFXs;
    public AudioClip[] BearAttackSFXs;
    public AudioClip[] FoxAttackSFXs;
    public AudioClip[] FoxMotorAttackWithVoiceSFXs;
    public AudioClip[] FoxMotorAttackWithoutVoiceSFXs;

    // enemy hit sfx
    public AudioClip WolfHitSFX;
    public AudioClip BearHitSFX;
    public AudioClip FoxHitSFX;

    // enemy death sfx
    public AudioClip[] WolfDeathSFXs;
    public AudioClip[] BearDeathSFXs;
    public AudioClip[] FoxDeathSFXs;

    public AudioSource audio;
    public AudioSource quieterAudio;

    // UI sfx
    public AudioClip UIConfirmSFX;
    public AudioClip UIExitSFX;

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
        DontDestroyOnLoad(this);
        audio.volume = 1.0f;
        quieterAudio.volume = 0.3f;
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
                break;
        }
    }

    public void MakeAttackSound(EnemyType type, bool isOnBike)
    {
        switch (type)
        {
            case EnemyType.WOLF:
                audio.PlayOneShot(WolfAttackSFXs[Random.Range(0, WolfAttackSFXs.Length)]);
                break;
            case EnemyType.BEAR:
                audio.PlayOneShot(BearAttackSFXs[Random.Range(0, BearAttackSFXs.Length)]);
                break;
            case EnemyType.FOX:
                if (isOnBike)
                {
                    if (Random.Range(0,1.0f) <= 0.2f) quieterAudio.PlayOneShot(FoxMotorAttackWithVoiceSFXs[Random.Range(0, FoxMotorAttackWithVoiceSFXs.Length)]);
                    else quieterAudio.PlayOneShot(FoxMotorAttackWithoutVoiceSFXs[Random.Range(0, FoxMotorAttackWithoutVoiceSFXs.Length)]);
                }
                else quieterAudio.PlayOneShot(FoxAttackSFXs[Random.Range(0, FoxAttackSFXs.Length)]);
                break;
            default:
                break;
        }
    }

    public void MakeDeathSound(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.WOLF:
                audio.PlayOneShot(WolfDeathSFXs[Random.Range(0, WolfDeathSFXs.Length)]);
                break;
            case EnemyType.BEAR:
                audio.PlayOneShot(BearDeathSFXs[Random.Range(0, BearDeathSFXs.Length)]);
                break;
            case EnemyType.FOX:
                quieterAudio.PlayOneShot(FoxDeathSFXs[Random.Range(0, FoxDeathSFXs.Length)]);
                break;
            default:
                break;
        }
    }

    public void OnBuildTurretSound()
    {
        audio.PlayOneShot(TurretBuildSFX);
    }

    public void OnUIConfirm()
    {
        audio.PlayOneShot(UIConfirmSFX);
    }
    public void OnUIExit()
    {
        audio.PlayOneShot(UIExitSFX);
    }
}


