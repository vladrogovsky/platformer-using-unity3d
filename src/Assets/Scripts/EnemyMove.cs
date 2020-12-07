using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //cache
    Rigidbody2D enemyBody;
    BoxCollider2D enemyPeriscopeCollider;
    CapsuleCollider2D enemyBodyCollider;
    //options
    [SerializeField] float enemySpeed = 100f;
    int moveDirection = 1;
    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemyPeriscopeCollider = GetComponent<BoxCollider2D>();
        enemyBodyCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyRun();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        moveDirection = -moveDirection;
        transform.localScale = new Vector3(moveDirection,1,1);
    }
    private void EnemyRun()
    {
        enemyBody.velocity = new Vector2(moveDirection * enemySpeed * Time.deltaTime, 0f);
        
    }
}
