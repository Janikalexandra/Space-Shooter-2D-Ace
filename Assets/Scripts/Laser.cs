using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    [SerializeField]
    private bool _isEnemyLaser = false;

    // Update is called once per frame
    void Update()
    {
        if(_isEnemyLaser == false)
        {
            MovementUp();
        }
        else
        {
            MovementDown();
        }
    }

    void MovementUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if(transform.position.y > 8f)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }       
    }

    void MovementDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && _isEnemyLaser == true)
        {
            Player player = other.gameObject.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
        }
    }

}
