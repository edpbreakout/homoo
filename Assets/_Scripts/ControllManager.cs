using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerInfo
{
    public Dictionary<string, KeyCode> controlls;
    public GameObject player;
    public GameObject spawnPoint;
}


public class ControllManager : MonoBehaviour
{
    public static ControllManager S;
    public List<GameObject> playerPrefabs;
    public List<GameObject> spawnPoints;
    public List<PlayerInfo> players;
    public float respawnTime;
    
    IEnumerator Delay(GameObject plr)
    {
        yield return new WaitForSeconds(respawnTime);
        plr.GetComponent<Battling>().Spawn();
    }
    public void respawn(int playerNo)
    {
        GameObject plr;
        plr = players[playerNo].player;
        plr.transform.position = players[playerNo].spawnPoint.transform.position;
        print("Player " + playerNo + " is dead, and going to respawn in " + respawnTime + " seconds");
        StartCoroutine(Delay(plr));
        plr.SetActive(false);
    }
    private void Awake()
    {
        S = this;

        players = new List<PlayerInfo>();
        PlayerInfo tmp;
        //Player 0
        tmp.controlls = new Dictionary<string, KeyCode>()
        {
            {"attack", KeyCode.E},
            {"shield", KeyCode.Q},
            {"left", KeyCode.A },
            {"right",KeyCode.D },
            {"jump",KeyCode.W },
            {"change",KeyCode.R }
        };
        tmp.player = playerPrefabs[0];
        tmp.spawnPoint = spawnPoints[0];
        players.Add(tmp);

        //Player1
        tmp.controlls = new Dictionary<string, KeyCode>()
        {
            {"attack", KeyCode.O},
            {"shield", KeyCode.U},
            {"left", KeyCode.J},
            {"right",KeyCode.L},
            {"jump",KeyCode.I },
            {"change", KeyCode.P }
        };
        tmp.player = playerPrefabs[1];
        tmp.spawnPoint = spawnPoints[1];
        players.Add(tmp);
    }

}
