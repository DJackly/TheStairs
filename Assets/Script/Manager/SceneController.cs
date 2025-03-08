using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    private const string VersionKey = "AppVersion";
    [SerializeField]private const string CurrentVersion = "1.0.11";
    private void Awake() {
        Instance = this;
    }
    public void InitializePlayerPrefs(){
        PlayerPrefs.SetInt("FirstRun", 1); // 标�?�为已运行过
        PlayerPrefs.SetInt("Title",1);  //进入标�?�界�???
        PlayerPrefs.SetFloat("Volume",UIManager.Instance.MainVolumeSlider.value);   //�??一次使用默认音�??
        PlayerPrefs.SetFloat("MouseSensivity",UIManager.Instance.MouseSenstivitySlider.value);  //默�?�灵敏度
        PlayerPrefs.SetString("AbnormalCollected","");
        PlayerPrefs.Save();
    }
    private void Start() {
        string savedVersion = PlayerPrefs.GetString(VersionKey, "");
        if (savedVersion != CurrentVersion)
        {
            // 重置PlayerPrefs
            PlayerPrefs.DeleteAll();
            // 初�?�化PlayerPrefs
            InitializePlayerPrefs();
            // 更新版本�?
            PlayerPrefs.SetString(VersionKey, CurrentVersion);
            PlayerPrefs.Save();
        }
        
        if(PlayerPrefs.GetInt("Title",-1)== 1){     //1就进入标题界�???
            UIManager.Instance.SetTitleBoard(true);     //有关标�?�的一切都�???从�?��?�开�???
            SEManager.Instance.StartBGM(2);
        }
        else{   //�ޱ���
            Player.Instance.GetComponent<Animator>().enabled = false;
            TimeSystem.Instance.StartTimer();
            SEManager.Instance.StartBGM(0);
        }
        PlayerPrefs.SetInt("Title",1);  //防�?�直接退游戏，所以先设置下�?�进入标�???
    }
    public void RestartGame(){
        PlayerPrefs.SetInt("Title",0);     //设置下�?�不进入标�??
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ReloadScene(){
        PlayerPrefs.SetInt("Title",1);     //设置下�?�进入标�???
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ExitGame()  
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
