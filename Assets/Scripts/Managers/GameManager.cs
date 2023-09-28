using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private int day = 0;
    public int Day
    {
        get { return day; }
        set { day = value; }
    }
    private float dayTimer = 0f;
    [SerializeField] private float secondsPerDay = 5f;
    [SerializeField] private int spawnEnemyRate = 7;
    [SerializeField] private int enemyAmount = 2;
    [SerializeField] private int maxEnemyOnMap = 10;
    private int dayCount = 0;
    [SerializeField] GameObject enemyPrefab;
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckTimeForDay();
        SpawnEnemy();
        ClearAllEnemies();
    }

    private void CheckTimeForDay()
    {
        dayTimer += Time.deltaTime;
        if (dayTimer >= secondsPerDay)
        {
            dayTimer = 0f;
            day++;
            dayCount++;
            MainUI.instance.UpdateDayText();
            TechManager.instance.CheckAllResearch();
        }
    }

    private void SpawnEnemy()
    {
        if (dayCount >= spawnEnemyRate)
        {
            for (int i = 0; i < enemyAmount; i++)
            {
                int rndX, rndZ;
                rndX = Random.Range(-400, 400);
                rndZ = Random.Range(-400, 400);
                Instantiate(enemyPrefab,
                new Vector3(rndX, 0, rndZ),
                Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
            }
            dayCount = 0;
        }
    }

    private void ClearAllEnemies()
    {
        List<GameObject> enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        if (enemies.Count > maxEnemyOnMap)
        {
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
    }
}
