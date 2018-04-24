using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickerController : MonoBehaviour {

    enum PlayerState { Active, Moving, Inactive, DoorsOpening }
    PlayerState _currentState;

    [Header("Difficulty settings")]
    [Space(11)]
    public Slider healthSlider;
    public float startHealth = 0f;
    public float target = 100f;
    public int floors = 3;
    public float[] depletionRates;
    public float clickPower = 2f;
    public int WaveCount = 2;
    public int EnemysInWave = 2;
    public float startTime = 2.0f;
    public float nextSpawnTime = 2.0f;
    public float punchPower = 1f;

    [Header("Elevator specs")]
    [Space(5)]
    public GameObject leftDoor;
    public GameObject leftDoorShort;
    public GameObject rightDoor;
    public GameObject rightDoorShort;
    public float moveDistance = 7f;
    public float waitTime = 5f;

    [Header("Enemies")]
    [Space(5)]
    public GameObject enemyPrefab;
    public List<GameObject> enemies = new List<GameObject>();
    public float enemyStrength = 2f;

    [Header("Misc")]
    [Space(5)]
    public Text floorText;
    public Text rateText;
    public bool isTimerRunning = false;
    ClickerController instance;
    Vector3 _leftDoorStart;
    Vector3 _leftDoorShortStart;
    Vector3 _rightDoorStart;
    Vector3 _rightDoorShortStart;
    float _currentHealth;
    float _moveTimeLeft;
    int _currentFloor = 1;
    float _enemyDepletionRate;
    float enemyHp = 10;
    private float failTimer = 0;
    private float failTimerFail = 20;

    //string[] _alphabet = new string[26] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };


    void Start () {
        instance = this;

        if (depletionRates.Length != floors)
        {
            Debug.LogError("Floor amount does not match with depletion rates.");
        }

        SetFloorText(_currentFloor);

        _currentState = PlayerState.Active;
        _currentHealth = startHealth;
        healthSlider.minValue = 0;
        healthSlider.maxValue = target;

        _leftDoorStart = leftDoor.transform.position;
        _leftDoorShortStart = leftDoorShort.transform.position;
        _rightDoorStart = rightDoor.transform.position;
        _rightDoorShortStart = rightDoorShort.transform.position;

        AdjustHealthSlider();
	}
	
	void Update () {

        if (isTimerRunning)
        {
            failTimer = Time.time;
            if (failTimer >= failTimerFail)
            {
                Fail();
            }
        }

        switch (_currentState)
        {
            case PlayerState.Active:

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    PushButton();
                }

                if(Input.GetKeyDown(KeyCode.P))
                {
                    HitFirstEnemy();
                }

                ReduceHealth();
                AdjustFloors(_currentHealth);

                print("Player active");
                break;

            case PlayerState.Inactive:
                print("Player inactive");
                AdjustFloors(0);
                break;

            case PlayerState.Moving:
                AdjustFloors(target);
                MoveElevator();
                print("Elevator moving.");
                break;

            case PlayerState.DoorsOpening:
                AdjustFloors(0f);
                print("Doors opening.");
                break;

            default:
                print("Default state");
                break;
        }

        //// DEBUG ENEMY SPAWNER
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    GameObject g = Instantiate(enemyPrefab,
        //                               new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f)),
        //                               Quaternion.identity);
        //    AddEnemy(g);
        //}

        // DEBUG
        rateText.text = "Depletion rate: " + (depletionRates[_currentFloor - 1] + _enemyDepletionRate).ToString();
    }

    void AdjustHealthSlider()
    {
        healthSlider.value = _currentHealth;
    }

    void ReduceHealth()
    {
        _currentHealth -= (depletionRates[_currentFloor - 1] + _enemyDepletionRate) * Time.deltaTime;
        AdjustHealthSlider();
        if(_currentHealth < 0)
        {
            _currentHealth = 0;
            // Lose condition?

            //_currentState = PlayerState.Inactive;
            //print("Player died!");
        }
    }

    void PushButton()
    {
        _currentHealth += clickPower;
        AdjustHealthSlider();

        if(_currentHealth >= target)
        {
            print("Floor " + _currentFloor + " finished.");
            RemoveAllEnemies();
            AdjustFloors(target);
            LevelFinished();
        }
    }

    void AdjustFloors(float newPosition)
    {
        var moveAmount = newPosition / target * moveDistance;
        Vector3 leftNewPos = _leftDoorStart + new Vector3(moveAmount, 0, 0);
        Vector3 leftShortNewPos = _leftDoorShortStart + new Vector3(moveAmount * 0.4f, 0, 0);
        Vector3 rightNewPos = _rightDoorStart - new Vector3(moveAmount, 0, 0);
        Vector3 rightShortNewPos = _rightDoorShortStart - new Vector3(moveAmount * 0.4f, 0, 0);

        var smooth = 5f;
        var t = Time.deltaTime;

        if (newPosition != target)
        {
            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftNewPos, t * smooth);
            leftDoorShort.transform.position = Vector3.Lerp(leftDoorShort.transform.position, leftShortNewPos, t * smooth);
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightNewPos, t * smooth);
            rightDoorShort.transform.position = Vector3.Lerp(rightDoorShort.transform.position, rightShortNewPos, t * smooth);
        }
        else
        {
            leftDoor.transform.position = leftNewPos;
            leftDoorShort.transform.position = leftShortNewPos;
            rightDoor.transform.position = rightNewPos;
            rightDoorShort.transform.position = rightShortNewPos;
        }


        if (_currentState == PlayerState.DoorsOpening)
        {
            if(Vector3.Distance(leftDoor.transform.position, _leftDoorStart) < 0.1f && Vector3.Distance(rightDoor.transform.position, _rightDoorStart) < 0.1f)
            {
                _currentFloor++;
                SetFloorText(_currentFloor);
                if(_currentFloor > floors)
                {
                    Debug.LogError("Win condition reached.");
                    Win();
                    //_currentState = PlayerState.Inactive;
                }
                else
                {
                    print("Floor " + _currentFloor + " starting.");
                    _currentState = PlayerState.Active;
                    _currentHealth = startHealth;
                }
            }
        }
    }

    void MoveElevator()
    {
        _moveTimeLeft -= Time.deltaTime;
        if(_moveTimeLeft < 0)
        {
            OpenDoors();
        }
    }

    void OpenDoors()
    {
        _currentState = PlayerState.DoorsOpening;

        failTimerFail += Time.time;

        LiftInvaderSpawner lis = GetComponent<LiftInvaderSpawner>();
        lis.canSpawnEnemys = true;

        lis.Spawn(WaveCount, EnemysInWave, startTime , nextSpawnTime);
    }

    void LevelFinished()
    {
        _moveTimeLeft = waitTime;
        _currentState = PlayerState.Moving;
        WaveCount += 1;
        EnemysInWave += 1;
        failTimer = 0;
        isTimerRunning = false;
    }

    void SetFloorText(int floorNumber)
    {
        if (floorNumber <= floors)
        {
            floorText.text = floorNumber.ToString();
        }
        else
        {
            floorText.text = "Penthouse";
        }
    }

    void RemoveAllEnemies()
    {
        LiftInvaderSpawner lis = GetComponent<LiftInvaderSpawner>();
        lis.canSpawnEnemys = false;

        GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Invader");
        for(int e = 0; e < Enemys.Length; e++)
        {
            Destroy(Enemys[e]);
        }
        Enemys = null;

        /* NOT IN USE
        foreach(GameObject enemy in enemies)
        {
            //enemies.Remove(enemy);
            Destroy(enemy);
        }
        */

        _enemyDepletionRate = 0;
    }

    public void AddEnemy(GameObject enemyAdded)
    {
        enemies.Add(enemyAdded);
        _enemyDepletionRate += enemyStrength;
        AssignButtonToEnemy(enemyAdded);
    }

    void AssignButtonToEnemy(GameObject thisEnemy)
    {
        //string s = _alphabet[Random.Range(0, _alphabet.Length)];
        //thisEnemy.name = s;
    }

    void HitFirstEnemy()
    {
        if (enemies.Count == 0)
        {
            return;
        }

        float d = 100; // Nearest enemy distance
        int j = 0; // Store nearest enemy position

        for(int i = 0; i < enemies.Count; i++)
        {
            float f = Vector3.Distance(enemies[i].transform.position, transform.position);
            if(f < d)
            {
                d = f;
                j = i;
            }
        }

        var e = enemies[j].GetComponent<LiftInvaderAI>();

        if (!e)
        {
            return;
        }

        e.Punched();
        //enemies[j].gameObject.GetComponent<Rigidbody>().AddForce((Vector3.forward + Vector3.right) * 1000f, ForceMode.VelocityChange);
        e.iHealth = punchPower;
        _enemyDepletionRate -= enemyStrength;
        print("Nearest enemy punched");
    }

    //public void RemoveEnemy(GameObject enemyRemoved)
    //{
    //    enemies.Remove(enemyRemoved);
    //    Destroy(enemyRemoved);
    //    _enemyDepletionRate -= enemyStrength;
    //}

    public void Fail()
    {
        _currentState = PlayerState.Inactive;
        // Animaation?
    }

    public void Win()
    {

    }
}