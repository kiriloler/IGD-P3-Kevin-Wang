using UnityEngine;

public class Services : MonoBehaviour
{
    public static OrganismSpawner organismSpawner;

    private void Awake()
    {
        organismSpawner = transform.GetChild(0).GetComponent<OrganismSpawner>();
    }
}
