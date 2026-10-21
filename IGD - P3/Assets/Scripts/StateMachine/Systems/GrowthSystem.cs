using UnityEngine;

public class GrowthSystem : MonoBehaviour
{
    public Vector2 startScale;
    public Vector2 matureScale;
    public float matureTime;
    public bool isMature = false;

    public bool stopGrowing = false;

    float TimeWhenEntered = 0;
    BreedSystem breedSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localScale = startScale;
        breedSystem = GetComponent<BreedSystem>();

        TimeWhenEntered = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopGrowing) return;

        if(isMature == false && Time.time - TimeWhenEntered >= matureTime)
        {
            transform.localScale = matureScale;
            isMature = true;
            if(breedSystem != null)
            {
                breedSystem.IsReadyToBreed = true;
            }
        }
    }
}
