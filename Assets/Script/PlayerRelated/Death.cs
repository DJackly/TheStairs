using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    public static Death Instance;
    public GameObject DeathCamera;
    public GameObject DarkBackGround;
    private void Awake()
    {
        Instance = this;
    }
    public void Die(GameObject lookAt = null)
    {
        if(lookAt != null)  //死亡视角
        {
            DeathCamera.transform.position = Camera.main.transform.position;
            if (Camera.main != null)
            {
                Camera.main.gameObject.SetActive(false);
            }
            DeathCamera.transform.LookAt(lookAt.transform.position);
            DeathCamera.GetComponent<AudioListener>().enabled = true;
        }
        Player.Instance.ForbidMoving(false);

        UIManager.Instance.DeathMode();
        SEManager.Instance.BrokenTV();
        StartCoroutine(DieAnima());
    }
    IEnumerator DieAnima()
    {
        yield return new WaitForSeconds(2f);
        DarkBackGround.SetActive(true);
        SpriteRenderer spriteRenderer = DarkBackGround.GetComponent<SpriteRenderer>();
        for (float i = 0f; i <= 1f;)
        {
            Color newColor = spriteRenderer.color;
            newColor.a = i; // 设置新的alpha值
            spriteRenderer.color = newColor; // 应用新的透明度
            i += 0.02f;
            yield return new WaitForSeconds(0.04f);
        }
    }
}
