using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum GameState
{
    OUT_OF_WAVE = 0,
    IN_WAVE = 1,
    PAUSED = 2
}

public class Gamesystem : MonoBehaviour
{
    public static GameState gameState = GameState.OUT_OF_WAVE;
    public static float wealth = 30;

    public static GameObject upgradeMovementButtons;
    public GameObject buttonPrefab;

    [SerializeField]
    private Text wealthText;
    private Wavesystem _wSystem;

    private bool cheatKeyDown = false;

    void Start()
    {
        upgradeMovementButtons = buttonPrefab;
        _wSystem = GameObject.FindObjectOfType<Wavesystem>();
    }

    void Update()
    {
        if (gameState == GameState.IN_WAVE && _wSystem.waveSpawned == true)
        {
            if (EnemyController.enemies.Count == 0)
            { 
                gameState = GameState.OUT_OF_WAVE;
                _wSystem.waveSpawned = false;

                for (int i = 0; i < StructureController.structures.Count; i++)
                {
                    StructureController.structures[i].GetComponent<StructureController>().hasUpMoved = false;
                }
            }
        }
        
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) && cheatKeyDown == false)
        {
            cheatKeyDown = true;
            wealth += 10;
        }

        if (Input.GetKeyUp(KeyCode.A) && cheatKeyDown == true)    
        {
            cheatKeyDown = false;
        }

        wealthText.text = wealth + " Gold";
    }
}
