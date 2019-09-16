using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _speedBoostMultiplier = 2.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.01f;
    [SerializeField]
    private int _lives = 3;
    private bool _TripleShotActive = false;
    private bool _speedBoostActive = false;
    private bool _shieldActive = false;
    private float _canFire = -1.0f;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _shieldVisual;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null");
        }
        _shieldVisual = GameObject.Find("Shield");
        _shieldVisual.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            shootLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4f, 6f), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void shootLaser()
    {
        _canFire = Time.time + _fireRate;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_TripleShotActive == true)
            {
                Vector3 topLaser = transform.position + new Vector3(0, 0.9f, 0);
                Vector3 leftLaser = transform.position + new Vector3(-0.624f, -0.28f, 0);
                Vector3 rightLaser = transform.position + new Vector3(0.624f,-0.28f,0);
                Instantiate(_laserPrefab, topLaser, Quaternion.identity);
                Instantiate(_laserPrefab, leftLaser, Quaternion.identity);
                Instantiate(_laserPrefab, rightLaser, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + (Vector3.up * 0.9f), Quaternion.identity);
            }
        }
    }

    public void Damage()
    {
        if (_shieldActive == true)
        {
            _shieldActive = false;
            _shieldVisual.SetActive(false) ;
            return;
        }

        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _TripleShotActive = true;
        StartCoroutine("TripleShotPowerDown");
    }

    IEnumerator TripleShotPowerDown()
    {
        while (_TripleShotActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _TripleShotActive = false;
        }
    }

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        _speed *= _speedBoostMultiplier;
        StartCoroutine("SpeedBoostPowerDown");
    }

    IEnumerator SpeedBoostPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _speedBoostActive = false;
        _speed /= _speedBoostMultiplier;
    }

    public void ShieldActive()
    {
        _shieldActive = true;
        _shieldVisual.SetActive(true);
    }
}
