using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[Serializable]
public class OrganismAttributes
{
    public string name;
    public int count;
    public GameObject prefab;
    public int maxCount { get; private set; }

    public OrganismAttributes(string name, int count, GameObject prefab)
    {
        this.name = name;
        this.count = count;
        this.prefab = prefab;
        maxCount = 0;
    }

    public void AddCount()
    {
        maxCount++;
        Debug.Log($"max count of {name}: {maxCount}");
    }
}
public class OrganismSpawner : MonoBehaviour
{
    public List<OrganismAttributes> Settings;
    public List<GameObject> Organisms;
    public Action OnSpawnStart;
    bool spawningStarted = false;
    public Action OnEcosystemEnd;

    public float TimeWhenEcosystemStarts = 0;
    public float TimeLasted => Time.time - TimeWhenEcosystemStarts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        InitializeOrganisms();
        OnSpawnStart?.Invoke();
        spawningStarted = true;
        TimeWhenEcosystemStarts = Time.time;
    }

    public void EcosystemEnds()
    {
        OnEcosystemEnd?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if(spawningStarted && Organisms.Count <= 1)
        {
            EcosystemEnds();
            spawningStarted = false;
        }
    }

    public void AddOrganism(string name, Vector2 position)
    {
        foreach(var setting in Settings)
        {
            if (setting.name == name)
            {
                Debug.Log($"instan name: {name}");
                NavMeshAgent agent = setting.prefab.GetComponent<NavMeshAgent>();
                if(NavMesh.SamplePosition(position, out NavMeshHit hit, 5f, agent.areaMask))
                {
                    GameObject organism = Instantiate(setting.prefab, hit.position, Quaternion.identity);
                    Organisms.Add(organism);
                    setting.AddCount();
                }
                return;
            }
        }
    }

    public void DeleteOrganism(GameObject organism)
    {
        if(Organisms.Contains(organism)) Organisms.Remove(organism);
        if (spawningStarted && OnlyExistsOfType("Harry"))
        {
            EcosystemEnds();
            spawningStarted = false;
        }
    }

    private void InitializeOrganisms()
    {
        foreach(var setting in Settings)
        {
            for(int i = 0; i < setting.count; i++)
            {
                GameObject organism = Instantiate(setting.prefab, Vector2.zero, Quaternion.identity);
                NavMeshAgent agent = organism.GetComponent<NavMeshAgent>();
                Vector2 pos = GetSpawnPoint(agent);
                organism.transform.position = pos;
                Organisms.Add(organism);
                setting.AddCount();
            }
        }
    }

    public void SetCount(string name,  int count)
    {
        for(int i = 0; i < Settings.Count; i++)
        {
            if(Settings[i].name == name)
            {
                Settings[i].count = count;
                return;
            }
        }
    }

    public virtual Vector2 GetSpawnPoint(NavMeshAgent agent)
    {
        while (true)
        {
            Camera cam = Camera.main;

            float randomX = UnityEngine.Random.Range(0.1f, 0.9f);
            float randomY = UnityEngine.Random.Range(0.1f, 0.9f);

            Vector3 viewportPoint = new Vector3(randomX, randomY, Mathf.Abs(cam.transform.position.z));
            Vector3 worldPoint = cam.ViewportToWorldPoint(viewportPoint);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(worldPoint, out hit, 5f, agent.areaMask))
            {
                return hit.position;
            }
        }
    }

    public bool OnlyExistsOfType(string name)
    {
        foreach(var gameObject in Organisms)
        {
            if (gameObject.name.Contains(name))
            {
                continue;
            }
            return false;
        }
        return true;
    }
}
