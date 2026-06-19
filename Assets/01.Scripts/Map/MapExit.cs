using UnityEngine;

public class MapExit : MonoBehaviour
{   
    [SerializeField] GameObject exitCollider;

    MapComponent map;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void SetExit(MapComponent map)
    {
        this.map = map;
    }

    public void DeactivateExit()
    {
        exitCollider.SetActive(false);
    }

    public void ActivateExit()
    {
        exitCollider.SetActive(true);
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
