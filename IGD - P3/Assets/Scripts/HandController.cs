using UnityEngine;

public class HandController : MonoBehaviour
{
    Collider2D c2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        c2D = GetComponent<Collider2D>();
    }

    public void StartSmash()
    {
        c2D.enabled = true;
    }

    public void EndSmash()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OrganismSM sm = collision.GetComponent<OrganismSM>();
        if (sm != null)
        {
            sm.Die();
        }
    }
}
