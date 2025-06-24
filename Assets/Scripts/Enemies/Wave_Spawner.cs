using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWaveSpawnerPool : MonoBehaviour
{
    [System.Serializable]
    public class EnemyInfo
    {
        public GameObject enemyPrefab;
        public int quantity;
    }

    [System.Serializable]
    public class Wave
    {
        public string waveName = "Vague";
        public float triggerTime;
        public float spawnRate;
        public List<EnemyInfo> enemies = new List<EnemyInfo>();
    }

    [System.Serializable]
    public class DifficultyWaves
    {
        public Difficulty difficulty;
        public List<Wave> waves = new List<Wave>();
    }

    [Header("Waves par difficulté")]
    public List<DifficultyWaves> difficultyWaves = new List<DifficultyWaves>();

    [Header("Points d'apparition communs")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("UI")]
    public Text waveNameText;

    private Dictionary<GameObject, Queue<GameObject>> _enemyPools = new Dictionary<GameObject, Queue<GameObject>>();
    private int _currentWaveIndex;
    private bool _isSpawning;

    private GameTimer _gameTimer;
    private List<Wave> _currentWaves;

    private int nbr_kill = 0;
    public int KillCount => nbr_kill;

    void Start()
    {
        var difficulty = GameSettings.difficulty;
        _currentWaves = difficultyWaves.Find(dw => dw.difficulty == difficulty)?.waves;

        if (_currentWaves == null || _currentWaves.Count == 0)
        {
            Debug.LogWarning($"[EnemyWaveSpawnerPool] Aucune vague trouvée pour la difficulté {difficulty}");
            enabled = false;
            return;
        }

        InitializePools();
        _gameTimer = FindAnyObjectByType<GameTimer>();
        if (_gameTimer == null)
        {
            Debug.LogError("[EnemyWaveSpawnerPool] Aucun GameTimer trouvé dans la scène !");
        }
    }

    void Update()
    {
        if (_currentWaveIndex >= _currentWaves.Count || _gameTimer == null) return;

        float elapsedTime = GameSettings.TimerDuration(GameSettings.difficulty) - _gameTimer.GetTimeRemaining();

        if (!_isSpawning && elapsedTime >= _currentWaves[_currentWaveIndex].triggerTime)
        {
            StartCoroutine(SpawnWave(_currentWaves[_currentWaveIndex]));
            _currentWaveIndex++;
        }
    }

    void InitializePools()
    {
        foreach (var wave in _currentWaves)
        {
            foreach (var enemyInfo in wave.enemies)
            {
                if (!_enemyPools.ContainsKey(enemyInfo.enemyPrefab))
                    _enemyPools[enemyInfo.enemyPrefab] = new Queue<GameObject>();

                for (int i = 0; i < enemyInfo.quantity; i++)
                {
                    GameObject enemy = Instantiate(enemyInfo.enemyPrefab);
                    enemy.SetActive(false);
                    _enemyPools[enemyInfo.enemyPrefab].Enqueue(enemy);
                }
            }
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        _isSpawning = true;

        // 🔔 Affiche le nom de la vague
        if (waveNameText != null)
        {
            waveNameText.gameObject.SetActive(true);
            waveNameText.text = $"{wave.waveName}";
            StartCoroutine(ClearWaveTextAfterDelay(3f));
        }

        foreach (var enemyInfo in wave.enemies)
        {
            for (int i = 0; i < enemyInfo.quantity; i++)
            {
                if (_enemyPools.ContainsKey(enemyInfo.enemyPrefab) && _enemyPools[enemyInfo.enemyPrefab].Count > 0)
                {
                    GameObject enemy = _enemyPools[enemyInfo.enemyPrefab].Dequeue();
                    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                    enemy.transform.position = spawnPoint.position;
                    enemy.transform.rotation = spawnPoint.rotation;
                    enemy.SetActive(true);
                }
                else
                {
                    Debug.LogWarning($"🟡 Pool vide pour {enemyInfo.enemyPrefab.name}");
                }

                yield return new WaitForSeconds(wave.spawnRate);
            }
        }

        _isSpawning = false;
    }

    IEnumerator ClearWaveTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (waveNameText != null)
            waveNameText.text = "";
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        nbr_kill++;
        foreach (var kvp in _enemyPools)
        {
            if (enemy.name.Contains(kvp.Key.name))
            {
                kvp.Value.Enqueue(enemy);
                return;
            }
        }
        Debug.LogWarning("❌ Ennemi retourné ne correspond à aucune pool !");
    }
}
