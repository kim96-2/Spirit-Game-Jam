using UnityEngine;

public class MapComponent : MonoBehaviour
{
    int stageNum;

    MapComponent nextMap;

    [SerializeField] Transform startPos;

    [SerializeField] MapExit exit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMap(int stageNum, Vector3 offsetPos, MapComponent nextMap)
    {
        transform.position = offsetPos;

        this.stageNum = stageNum;

        this.nextMap = nextMap;

    }

    public void StartMap(Transform player)
    {
        player.position = startPos.position;

        exit.SetExit(this);
        exit.DeactivateExit();

        EndMap();
    }

    public void EndMap()
    {
        exit.ActivateExit();

    }

    public void GoNextMap()
    {
        MapManager.Instance.GoNextMap(stageNum + 1);
    }

}
