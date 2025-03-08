using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SEManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public static SEManager Instance;
    public List<AudioClip> SEList = new List<AudioClip>();
    public List<AudioClip> MusicList = new List<AudioClip>();
    public AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }
    public void ChangeMainVolume(float v){
        audioMixer.SetFloat("MainVolume",v);
    }
    public void StartBGM(int i){
        //i为音乐的索引，0为风声，2为楼梯BGM
        audioSource.clip = MusicList[i];
        audioSource.Play();
    }
    public IEnumerator SwitchingToGameBGM(){   //正式开始游戏后播放背景风声
        // 逐渐降低音量至0
        float fadeOutTime = 1.5f; // 淡出时间
        float startTime = Time.time;
        while (audioSource.volume > 0)
        {
            audioSource.volume = Mathf.Lerp(1, 0, (Time.time - startTime) / fadeOutTime);
            yield return null;
        }
        audioSource.volume = 0; // 确保音量完全为0

        // 切换音乐
        audioSource.clip = MusicList[0];
        audioSource.Play();

        // 逐渐提高音量至100%
        float fadeInTime = 1.5f; // 淡入时间
        startTime = Time.time;
        while (audioSource.volume < 1)
        {
            audioSource.volume = Mathf.Lerp(0, 1, (Time.time - startTime) / fadeInTime);
            yield return null;
        }
        audioSource.volume = 1; // 确保音量完全为1
    }
    public void SwitchFlash()
    {
        audioSource.volume = 1f;
        audioSource.PlayOneShot(SEList[0]);
    }
    public void BrokenTV()
    {
        audioSource.volume = 0.15f; //死亡时的音量0.15
        audioSource.clip = MusicList[1];
        audioSource.Play();
    }
    public void PianoScary()
    {
        audioSource.volume = 1f;
        audioSource.PlayOneShot(SEList[1]);
    }
    public void PaperFlip()
    {
        audioSource.volume = 1.1f;
        audioSource.PlayOneShot(SEList[2]);
    }
    public void OpenDoor()
    {
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(SEList[3]);
    }
    public void CloseDoor()
    {
        audioSource.volume = 0.9f;
        audioSource.PlayOneShot(SEList[4]);
    }
    public void PressButton(){
        audioSource.volume = 1f;
        audioSource.PlayOneShot(SEList[5]);
    }
}
