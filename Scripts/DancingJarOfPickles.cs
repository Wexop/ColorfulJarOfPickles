﻿using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace ColorfulJarOfPickles.Scripts;

public class DancingJarOfPickles : NetworkBehaviour
{
    private static readonly int Playing = Animator.StringToHash("playing");


    public Animator animator;
    public AudioClip happySong;
    
    public AudioSource audioSource;
    
    private bool isPlaying;

    public IEnumerator onPlayingSong()
    {
        yield return new WaitUntil(() => audioSource.isPlaying == false);
        isPlaying = false;
        animator.SetBool(Playing, false);
    }

    public void TriggerDance(bool dance)
    {
        animator.SetBool(Playing, dance);

        if (dance)
        {
            audioSource.volume = ColorfulJarOfPicklesPlugin.instance.dancingMusicVolume.Value;
            audioSource.PlayOneShot(happySong);
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        StartCoroutine(onPlayingSong());
    }

    public void OnGrabItem()
    {

        DancePicklesServerRpc();

    }
    
    [ServerRpc(RequireOwnership = false)]
    public void DancePicklesServerRpc()
    {
        isPlaying = !isPlaying;
        DancePicklesClientRpc(isPlaying);
    }


    [ClientRpc]
    public void DancePicklesClientRpc(bool dance)
    {
        TriggerDance(dance);
    }
    

}