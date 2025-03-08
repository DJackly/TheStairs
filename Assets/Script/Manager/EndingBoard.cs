using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingBoard : MonoBehaviour
{
    public TextMeshProUGUI EndTime;
    public GameObject Button;
    CanvasGroup canvasGroup;    //按钮的！！！
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
            // 从1淡出到0
            yield return FadeCanvasGroupRoutine(canvasGroup, 1, 0, 1f);
            // 从0淡入到1
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
        cg.alpha = end; // 确保最终透明度准确
    }
}
