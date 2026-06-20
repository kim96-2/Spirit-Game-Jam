using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Mgr : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Sound_Mgr.Inst != null)
        {
            Sound_Mgr.Inst.PlayBgm("Bgm_1", 0.8f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("GAME");
        }
    }
}
