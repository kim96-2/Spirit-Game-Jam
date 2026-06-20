using System.Collections;
using Game.Core;
using UnityEngine;
using UnityEngine.UI;

public class GameStageUI : GenericSingleton<GameStageUI>
{
    [SerializeField] Text stageInfoText;

    // void Start()
    // {
    //     stageInfoText.text = "";
    // }

    void Update()
    {
        if(_time < 0f) stageInfoText.text = "";
        else
        {
            _time -= Time.deltaTime;
        }
    }
    
    float _time = 0f;
    public void SetInfoText(string infoText)
    {
        stageInfoText.text = infoText;

        _time = 2f;
    } 
}
