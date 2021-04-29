using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _normalSpeed = 5f;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _canFire = -1f;
    [SerializeField] private float _powerupSpeed = 2f;
    [SerializeField] private float _thrusterSpeed = 1f;

    [SerializeField] private GameObject _laserPrefab;   
    [SerializeField] private GameObject _laserSpawn;
    [SerializeField] private GameObject _shield;

    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _leftEngine;

    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _tripleShotSpawn;

    [SerializeField] private int _lives = 3;
    [SerializeField] private int _shieldLives = 0;
    [SerializeField] private int _score;

    [SerializeField] private bool _tripleShotActive = false;
    [SerializeField] private bool _speedBoostEnabled = false;
    [SerializeField] private bool _shieldActive = false;

    [SerializeField] private AudioClip _destroyedClip;
    [SerializeField] private AudioClip _laserSoundClip;
    private AudioSource _playerAudioSource;

    private UIManager ui_Manager;

    private SpawnManager _spawnManager;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {


        _playerAudioSource = GetComponent<AudioSource>();

        ui_Manager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        

        healthBar = GameObject.FindGameObjectWithTag("ShieldHealth").GetComponent<HealthBar>();
        healthBar.SetHealth(0);

        if(_playerAudioSource == null)
        {
            Debug.LogError("Audio Source on the player is null!");
        }
        else
        {
            _playerAudioSource.clip = _laserSoundClip;
        }

        if(ui_Manager == null)
        {
            Debug.LogError("UI Manager is null!");
        }

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null!");
        }

        _shield.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        // Framework Thruster
        Thrusters();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            ShootLaser();
        }
    }

    void CalculateMovement()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);  
        
        transform.Translate(direction * _normalSpeed * Time.deltaTime);


        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), transform.position.z);

        if(transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, transform.position.z);
        }
    }

    void Thrusters()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _normalSpeed *= _thrusterSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _normalSpeed /= _thrusterSpeed;
        }
    }

    void ShootLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_tripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {          
            Instantiate(_laserPrefab, _laserSpawn.transform.position, Quaternion.identity);
        }

        _playerAudioSource.Play();

    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5f);
        _tripleShotActive = false;
    }


    public void Damage()
    {
        if (_shieldActive == true)
        {
            //Shield Strength
            if (_shieldLives == 3)
            {
                _shieldLives -= 1;
                healthBar.SetHealth(2);
                return;
            }

            if (_shieldLives == 2)
            {
                _shieldLives -= 1;
                healthBar.SetHealth(1);
                return;
            }

            if (_shieldLives == 1)
            {
                _shieldLives -= 1;
                healthBar.SetHealth(0);
                _shieldActive = false;
                _shield.SetActive(false);
                return;
            }
        }

            _lives -= 1;

            // Enable right engine smoke
            if (_lives == 2)
            {
                _rightEngine.SetActive(true);
            }
            // Enabel left engine smoke
            if (_lives == 1)
            {
                _leftEngine.SetActive(true);
            }

            ui_Manager.UpdateLives(_lives);

            if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
            }
 }


    public void SpeedBoostActive()
    {
        _speedBoostEnabled = true;
        _normalSpeed *= _powerupSpeed;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speedBoostEnabled = false;
        _normalSpeed /= _powerupSpeed;
    }

    public void ShieldsActive()
    {
        _shieldLives = 3;
        healthBar.SetHealth(3);
        _shield.SetActive(true);
        _shieldActive = true;
    }

    public void AddScore(int points)
    {
        _score += points;
        ui_Manager.UpdateScore(_score);
    }


}