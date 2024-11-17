using System.Collections;
using System.Collections.Generic;
using TheHeroesJourney;
using UnityEngine;

public class NormBullet : BaseBullet
{
    public GameObject _prefabBullet;

    // Start is called before the first frame update
    void Start()
    {
        distanceTraveled = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (distanceTraveled >= config.maxRange)
        {
           DestroyBullet();
        }
    }

    public override void Move()
    {
        transform.position += config.normalSpeed * Time.deltaTime * bulletMoveDirection;
        distanceTraveled += config.normalSpeed * Time.deltaTime;
    }
}
