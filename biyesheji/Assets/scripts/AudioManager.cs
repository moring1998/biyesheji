using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///背景音乐播放与切换、特效音乐的播放与切换，在GameManager之后实例化
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance
    {
        get;private set;
    }
    public AudioSource audioSource;
    public AudioClip[] audioclips;//特效音
    public AudioClip[] audioBGClips;//背景音
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 播放特效音
    /// </summary>
    /// <param name="soundIndex"></param>
    public void PlaySound(int soundIndex)
    {
        audioSource.PlayOneShot(audioclips[soundIndex]);
    }
    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="soundIndex"></param>
    public void ChangeBGM(int soundIndex)
    {
        audioSource.Stop();
        audioSource.clip = audioBGClips[soundIndex];
        audioSource.Play();
    }
    public void CloseBGM()
    {
        audioSource.Stop();
        //audioSource.Play();
    }
}
