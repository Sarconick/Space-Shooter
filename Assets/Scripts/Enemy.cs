using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;
    private bool _hasArrived = false;
    private Player _player;

    //handle to animator component
    private Animator _enemyDeath;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        _enemyDeath = GetComponent<Animator>();
        if (_enemyDeath == null)
        {
            Debug.LogError("Enemy Death animation is NULL");
        }
    }
    // Update is called once per frame
    void Update()
    {

        float randX = Random.Range(-9.0f, 9.0f);
        float randY = Random.Range(-6.5f, 6.5f);
         /* if (!_hasArrived)
         {
             _hasArrived = true;
             StartCoroutine(MoveToPoint(new Vector3(randX, randY, 0)));
         } */
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -5.47f)
        {
            transform.position = new Vector3(randX, 8, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D CollisionObject)
    {
        if (CollisionObject.CompareTag("Player"))
        {
            Player player = CollisionObject.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _enemyDeath.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            Destroy(this.gameObject,2.5f);
        }
       else if (CollisionObject.CompareTag("Laser"))
        {
            Destroy(CollisionObject.gameObject);
            if (_player != null)
            {
                _player.IncreaseScore(Random.Range(5,12));
            }
            _enemyDeath.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            Destroy(this.gameObject,2.5f);
        }
    }

    private IEnumerator MoveToPoint(Vector3 TargetPos)
    {
    	float waitBeforeMoving = 1.0f;
    	float movementDuration = 2.0f;
        float timer = 0.0f;
        Vector3 startPos = transform.position;

        while (timer < movementDuration)
        {
            timer += Time.deltaTime;
            float t = timer / movementDuration;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            transform.position = Vector3.Lerp(startPos, TargetPos, t);

            yield return null;
        }

        yield return new WaitForSeconds(waitBeforeMoving);
        _hasArrived = false;
    }
}
