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
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
