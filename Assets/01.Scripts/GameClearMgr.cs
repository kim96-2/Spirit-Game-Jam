using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearMgr : MonoBehaviour
{
    public Button TOMainBtn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (TOMainBtn != null)
        {
            TOMainBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("MAIN");
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
