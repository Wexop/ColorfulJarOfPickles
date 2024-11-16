using System.Collections;
using UnityEngine;

namespace ColorfulJarOfPickles.Scripts;

public class RainbowDancingJarOfPickles : RainbowColorfulJarOfPickles
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

    public override void TriggerDance(bool dance)
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

    public override void GrabItem()
    {
        base.GrabItem();
        
        isPlaying = !isPlaying;
        NetworkColorfulJar.DancePicklesServerRpc(NetworkObjectId, isPlaying);

    }
    

}