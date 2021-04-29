using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    private bool _isAlive = true;

    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;

    [SerializeField] private AudioClip _destroyedClip;
    [SerializeField] private AudioSource _audio;

    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();

        if(_audio == null)
        {
            Debug.LogError("Enemy Audio Source is null!");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();

        _anim = GetComponent<Animator>();

        if(_player == null)
        {
            Debug.LogError("Player is null!");
        }

        if(_anim == null)
        {
            Debug.LogError("Animator is null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Time.time > _canFire && _isAlive == true)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randomX, 8.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {                   
            if(_player != null)
            {
                _player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audio.Play();
            _isAlive = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audio.Play();
            _isAlive = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }

    
}
