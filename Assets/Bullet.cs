using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20;
    public float destoryTime = 1f;
    public int power = 20;

    private void Start()
    {
        Destroy(gameObject, destoryTime);
    }

    void Update()
    {
        transform.Translate(new Vector3(0,0, speed * Time.deltaTime), Space.Self); //월드축

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Zombi")) // 이 방식으로 해야 CG 생성 X
        {
            var zombi = other.GetComponent<Zombie>();
            zombi.TakeHit(power);
            Destroy(gameObject);
        }
    }
}
