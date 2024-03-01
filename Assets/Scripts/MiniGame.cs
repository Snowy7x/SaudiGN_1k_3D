using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public LayerMask enemyLayer;
    public int neededScore = 5;
    public int neededCoins = 5;
    
    private int score = 0;
    private int collectedCoins = 0;

    public static MiniGame instance;
    public GameObject miniGamePrefab;
    public Transform m_2DHolder;
    private GameObject _currentMiniGame;

    [SerializeField] private Transform camPos;
    private Thread _resetThread;

    private void Awake()
    {
        instance = this;
        Reset();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Player.instance.is2D)
        {
            HUD.instance.SetInfo("press [e] to play");
            Interactions.instance.DoInteraction += Play;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !Player.instance.is2D)
        {
            HUD.instance.SetInfo("press [e] to play");
            Interactions.instance.DoInteraction += Play;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HUD.instance.SetInfo("");
            Interactions.instance.DoInteraction -= Play;
        }
    }

    public void Play()
    {
        if (Vector3.Distance(transform.position, Player.instance.transform.position) > 7f) return;
        if (Physics.CheckSphere(transform.position, 15f, enemyLayer))
        {
            HUD.instance.SetInfo("Enemies are too close!!", 2f);
            return;
        }
        HUD.instance.SetInfo("");
        Player.instance.MiniGame_Mode(camPos);
        Player2D.instance.SetHealth(Player.instance.healthText);
    }

    void Update_MiniGameUI()
    {
        // TODO: MiniGame UI
    }

    private void Reset()
    {
        // TODO: RESET
        if (_currentMiniGame) Destroy(_currentMiniGame, 0.5f);
        GameObject go = Instantiate(miniGamePrefab, m_2DHolder);
        _currentMiniGame = go;
        collectedCoins = 0;
        score = 0;
    }

    public void AddCoin()
    {
        collectedCoins++;
    }

    public void AddKill()
    {
        score++;
    }

    public void Pre_Done()
    {
        Player.instance.FPS_Mode();
        if (collectedCoins < neededCoins || score < neededScore)
        {
            HUD.instance.SetTitle("FAILED", 1f, Color.red);
            HUD.instance.SetInfo("Try again!", 1f);
        }
        else
        {
            HUD.instance.SetTitle("Done it!", 1f, Color.green);
            HUD.instance.SetInfo("Go to the control room to leave!", 1f);
        }
    }

    public void Done()
    {
        Player.instance.FPS_Mode();
        Reset();
    }
}
