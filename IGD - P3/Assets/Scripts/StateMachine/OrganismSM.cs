using UnityEngine;
using UnityEngine.AI;

public class OrganismSM : MonoBehaviour
{
    public bool IsAlive = true;
    StateBase currentState;
    public StateBase CurrentState => currentState;

    #region properties
    public FloatRange IdleTime;
    public FloatRange WanderTime;
    public float WanderRadius;
    #endregion

    #region components
    public NavMeshAgent agent { get; private set; }
    public Animator animator { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public HungerSystem hungerSystem { get; private set; }
    #endregion

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hungerSystem = GetComponent<HungerSystem>();

        //no 3d rotations
        if(agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }

    protected virtual void Update()
    {
        currentState?.Update();
    }

    protected virtual void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void PlayAnimation(string animationName, float fadeTime)
    {
        animator.CrossFadeInFixedTime(animationName, fadeTime);
    }

    public void ChangeState(StateBase state, bool restart = true)
    {
        if (currentState == state && restart == false) return;
        if(currentState != null)currentState.Exit();
        currentState = state;
        currentState.Enter();
    }

    public virtual void UpdateFaceDirection()
    {
        if(agent.velocity.x < -0.5f)
        {
            spriteRenderer.flipX = true;
        }
        else if(agent.velocity.x > 0.5f)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void StopAgent()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();
    }

    public virtual void Die()
    {
        IsAlive = false;
    }

    public virtual void Vanish()
    {
        Services.organismSpawner.DeleteOrganism(gameObject);
        Destroy(gameObject);
    }
}