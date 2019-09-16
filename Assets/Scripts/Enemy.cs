using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool hasArrived = false;
        float randX = Random.Range(-9.0f, 9.0f);
        float randY = Random.Range(-1.5f, 1.5f);
        //transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        transform.position = new Vector3(startPos,TargetPos,t); //randomize movement after enemy spawn and then go into IF check 
        if (!hasArrived)
        {
            hasArrived = true;
            StartCoroutine(MoveToPoint(new Vector3(randX, randY, 0)));
        }

        if (transform.position.y < -7.0f)
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

            Destroy(this.gameObject);
        }
       else if (CollisionObject.CompareTag("Laser"))
        {
            Destroy(CollisionObject.gameObject);
            Destroy(this.gameObject);
        }
    }

    private IEnumerator MoveToPoint(Vector3 TargetPos)
    {
        
    	float waitBeforeMoving = 2.0f;
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
        hasArrived = false;
    }
}
