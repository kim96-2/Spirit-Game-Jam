using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver_Mgr : MonoBehaviour
{
    public Button Restart_Btn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Restart_Btn != null)
        {
            Restart_Btn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("MAIN");
            });
        }

        Sound_Mgr.Inst.PlayBgm("Bgm_1", 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
