using TMPro;
using UnityEngine;
// using static LobbyManager; // Be careful with statics in Network code

public class GameManager : MonoBehaviourSingleton<GameManager>
{

    public TextMeshProUGUI MoneyText;





    [SerializeField] SpawnManager spawnManager;

    protected override void Awake()
    {
        base.Awake();

    }

    public void EndGame(string winner)
    {

    }




}
