using UnityEngine;

public class MapExit : MonoBehaviour
{   
    [SerializeField] GameObject exitCollider;
    [SerializeField] GameObject exitWall;

    MapComponent map;

    public void SetExit(MapComponent map)
    {
        this.map = map;
    }

    public void DeactivateExit()
    {
        exitCollider.SetActive(false);
        exitWall.SetActive(true);
    }

    public void ActivateExit()
    {
        exitCollider.SetActive(true);
        exitWall.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            map.GoNextMap();
            this.enabled = false;
        }
    }
}
