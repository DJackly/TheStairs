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
        //iΪ���ֵ�������0Ϊ������2Ϊ¥��BGM
        audioSource.clip = MusicList[i];
        audioSource.Play();
    }
    public IEnumerator SwitchingToGameBGM(){   //��ʽ��ʼ��Ϸ�󲥷ű�������
        // �𽥽���������0
        float fadeOutTime = 1.5f; // ����ʱ��
        float startTime = Time.time;
        while (audioSource.volume > 0)
        {
            audioSource.volume = Mathf.Lerp(1, 0, (Time.time - startTime) / fadeOutTime);
            yield return null;
        }
        audioSource.volume = 0; // ȷ��������ȫΪ0

        // �л�����
        audioSource.clip = MusicList[0];
        audioSource.Play();

        // �����������100%
        float fadeInTime = 1.5f; // ����ʱ��
        startTime = Time.time;
        while (audioSource.volume < 1)
        {
            audioSource.volume = Mathf.Lerp(0, 1, (Time.time - startTime) / fadeInTime);
            yield return null;
        }
        audioSource.volume = 1; // ȷ��������ȫΪ1
    }
    public void SwitchFlash()
    {
        audioSource.volume = 1f;
        audioSource.PlayOneShot(SEList[0]);
    }
    public void BrokenTV()
    {
        audioSource.volume = 0.15f; //����ʱ������0.15
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
