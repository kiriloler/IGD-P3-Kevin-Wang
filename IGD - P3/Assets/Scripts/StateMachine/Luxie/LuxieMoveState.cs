using UnityEngine;
using UnityEngine.AI;

public class LuxieMoveState : LuxieStateBase
{
    Vector2 Destination = Vector2.positiveInfinity;
    //projectile properties
    bool isMoving = false;
    readonly float jumpTime = 0.5625f;
    float g;
    float vx0;
    float vy0;
    float TimeWhenStartMoving = 0;
    Vector2 positionWhenStartMoving;

    public LuxieMoveState(LuxieSM luxie) : base(luxie)
    {
    }

    public override void Enter()
    {
        Luxie.StopAgent();
        Luxie.agent.updatePosition = false;
        Luxie.PlayAnimation("Move", 0f);
        Luxie.hungerSystem.huntColliderHook.OnTriggerStay += OnPreyDetected;
        Luxie.attackColliderHook.OnTriggerEnter += OnAttackHit;
    }

    public override void Update()
    {
        if (isMoving)
        {
            float dt = Time.time - TimeWhenStartMoving;
            if(dt <= jumpTime)
            {
                float dx = vx0 * dt;
                float dy = vy0 * dt + 0.5f * g * dt * dt;
                Vector2 newPos = new Vector2(dx, dy);
                Luxie.transform.position = (Vector3)(positionWhenStartMoving + newPos);
            }
            else
            {
                AnimatorStateInfo currentInfo = Luxie.animator.GetCurrentAnimatorStateInfo(0);
                if(currentInfo.normalizedTime >= 1)
                {
                    isMoving = false;
                    if (Random.Range(0f, 1f) < 0.7f)
                    {
                        Destination = Vector2.positiveInfinity;
                        Luxie.PlayAnimation("Move", 0f);
                    }
                    else
                    {
                        Luxie.ChangeState(Luxie.idleState, true);
                        return;
                    }
                }
            }
        }
    }
    public override void Exit()
    {
        Luxie.hungerSystem.EndHunting();
        Luxie.EndAttack();
        Destination = Vector2.positiveInfinity;
        Luxie.agent.Warp(Luxie.transform.position);
        Luxie.agent.updatePosition = true;
        Luxie.hungerSystem.huntColliderHook.OnTriggerStay -= OnPreyDetected;
        Luxie.attackColliderHook.OnTriggerEnter -= OnAttackHit;
    }

    public void StartJumping()
    {
        if(Destination.Equals(Vector2.positiveInfinity))
        {
            Destination = FindPointToMove();
            Debug.Log($"Destination: {Destination}");
        }
        isMoving = true;
        float deltaX = Destination.x - Luxie.transform.position.x;
        float deltaY = Destination.y - Luxie.transform.position.y;
        vx0 = deltaX / jumpTime;
        vy0 = Luxie.VelocityY.Random();
        g = 2 * (deltaY - vy0 * jumpTime) / (jumpTime * jumpTime);
        TimeWhenStartMoving = Time.time;
        positionWhenStartMoving = Luxie.transform.position;
    }

    private void OnPreyDetected(Collider2D collider)
    {
        if (Luxie.hungerSystem.IsHungry == false) return;

        Luxie.hungerSystem.StartHunting();
        if (Destination.Equals(Vector2.positiveInfinity))
        {
            Debug.Log("catch prey");
            Destination = collider.transform.position;
        }
    }

    private void OnAttackHit(Collider2D collider)
    {
        OrganismSM preySM = collider.GetComponent<OrganismSM>();
        if (preySM != null && preySM.IsAlive)
        {
            preySM.Die();
            Luxie.hungerSystem.IncreaseEnergy(preySM.hungerSystem.Energy / 2.5f);
            Services.organismSpawner.AddOrganism("Luxie", Luxie.transform.position);
            Luxie.ChangeState(Luxie.idleState, true);
        }
    }
}
