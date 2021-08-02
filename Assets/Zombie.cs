using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;
    Animator animator;
    public int hp = 100;
    float originalSpeed;

    IEnumerator Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        target = FindObjectOfType<Player>().transform;  // 

        while (hp > 0)
        {
            if (target)
                agent.destination = target.position;
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        }
    }

    public float BloodEffectYPosition = 1.3f;
    public GameObject bloodParticle;
    internal void TakeHit(int damage, Vector3 toMoveDirection)
    {
        hp -= damage;
        //animator.Play("TakeHit");
        animator.Play(Random.Range(0, 2) == 0 ? "TakeHit1" : "TakeHit2",0,0);
        // 피격 이펙트 생성(피,..)
        CreateBloodEffect();

        // 총알 맞으면 뒤로 밀려나자.
        PushBackMove(toMoveDirection);

        // 이동 스피드 잠시 0을 만들기
        agent.speed = 0;
        Invoke(nameof(SetTakeHitSpeedCo), TakeHitStopSpeedTime);
        StartCoroutine(SetTakeHitSpeedCo());

        if (hp <= 0)
        {
            GetComponent<Collider>().enabled = false;
            Invoke(nameof(Die), 1);
        }
    }

    private void CreateBloodEffect()
    {
        var pos = transform.position;
        pos.y = BloodEffectYPosition;
        Instantiate(bloodParticle, pos, Quaternion.identity);
    }

    public float moveBackDistance = 0.1f;
    public float moveBackNoise = 0.1f;
    private void PushBackMove(Vector3 toMoveDirection)
    {
        toMoveDirection.x = Random.Range(-moveBackNoise, moveBackNoise);
        toMoveDirection.z = Random.Range(-moveBackNoise, moveBackNoise);
        toMoveDirection.y = 0;
        toMoveDirection.Normalize();
        transform.Translate(toMoveDirection * moveBackDistance, Space.World);
    }

    public float TakeHitStopSpeedTime = 0.1f;
    private IEnumerator SetTakeHitSpeedCo()
    {
        yield return new WaitForSeconds(TakeHitStopSpeedTime);
        agent.speed = originalSpeed;
    }

    void Die()
    {
        animator.Play("Die");
        Destroy(gameObject, 1);
    }
}