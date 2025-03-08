using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingBoard : MonoBehaviour
{
    public TextMeshProUGUI EndTime;
    public GameObject Button;
    CanvasGroup canvasGroup;    //��ť�ģ�����
    private void Start() {
        canvasGroup = Button.GetComponent<CanvasGroup>();
        GetComponent<CanvasGroup>().alpha = 0;
        Button.SetActive(false);
    }
    public void Show(){
        EndTime.text = TimeSystem.Instance.GetTime();
        Button.SetActive(true);
        StartCoroutine(FadeLoop());
    }
    private IEnumerator FadeLoop()
    {
        while (true)
        {
            // ��1������0
            yield return FadeCanvasGroupRoutine(canvasGroup, 1, 0, 1f);
            // ��0���뵽1
            yield return FadeCanvasGroupRoutine(canvasGroup, 0, 1, 1f);
        }
    }

    private IEnumerator FadeCanvasGroupRoutine(CanvasGroup cg, float start, float end, float duration)
    {
        float counter = 0f;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, counter / duration);
            yield return null;
        }
        cg.alpha = end; // ȷ������͸����׼ȷ
    }
}
