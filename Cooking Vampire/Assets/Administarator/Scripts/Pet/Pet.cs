using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public abstract class Pet : MonoBehaviour, IPoolObject
{
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;

    protected GameManager_Survivor gm;
    protected Player player;
    protected SpawnManager spawnManager;
    protected SpriteData spriteData;

    public PetState petState;
    public Sprite projectileSprite;
    public RuntimeAnimatorController projectileAnim;
    protected Coroutine curStateRoutine;
    protected bool isMove;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spawnManager = SpawnManager.Instance;
        spriteData = SpriteData.Instance;
        gm = GameManager_Survivor.Instance;
        player = gm.player;
    }

    public void OnGettingFromPool()
    {
        Warp();
        StartCoroutine(ChangeStateRoutine());
    }

    private void Update()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > player.scanner.defRange * 2f)
            Warp();
    }

    public void Warp()
    {
        Vector2 ranPos = player.transform.position - new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));

        transform.position = ranPos;
        Vector2 effectPos = transform.position + new Vector3(0, 0.5f);
        spawnManager.Spawn_Effect(spriteData.effects[0], effectPos);
    }

    protected void SetAnim(PetState state)
    {
        switch(state)
        {
            case PetState.Idle:
            case PetState.Walk:
            case PetState.Lick:
            case PetState.Sleep:
                anim.SetInteger("State", (int)state);
                break;
            case PetState.Atk:
            case PetState.Danger:
                anim.SetTrigger(state.ToString());
                break;
        }
    }
    protected void SetFlip(Transform target)
    {
        spriteRenderer.flipX = transform.position.x - target.position.x > 0;
    }

    protected IEnumerator ChangeStateRoutine()
    {
        while(true)
        {
            petState = (PetState)Random.Range(0, System.Enum.GetValues(typeof(PetState)).Length);

            switch(petState)
            {
                case PetState.Idle:
                case PetState.Lick:
                case PetState.Sleep:
                    isMove = false;
                    SetAnim(petState);
                    break;
                case PetState.Walk:
                    SetAnim(petState);
                    break;
                case PetState.Atk:
                case PetState.Danger:
                    break;
            }

            curStateRoutine = StartCoroutine(LoopStateRoutine());
            yield return new WaitForSeconds(player.weaponController.Find_Weapon_Pet().stat.coolTime * 4f);
            StopCoroutine(curStateRoutine);
        }
    }
    protected IEnumerator LoopStateRoutine()
    {
        while (true)
        {
            switch (petState)
            {
                case PetState.Idle:
                    break;
                case PetState.Walk:
                    yield return WalkRoutine(null);
                    break;
                case PetState.Lick:
                    break;
                case PetState.Sleep:
                    break;
                case PetState.Atk:
                    yield return AtkRoutine();
                    break;
                case PetState.Danger:
                    yield return DangerRoutine();
                    break;
            }
            yield return null;
        }
    }
    protected abstract IEnumerator WalkRoutine(Transform target);
    protected abstract IEnumerator AtkRoutine();
    protected abstract IEnumerator DangerRoutine();
}

public enum PetState { Idle = 0, Walk, Lick, Sleep, Atk, Danger }