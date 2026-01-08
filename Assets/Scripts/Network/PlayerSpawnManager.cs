using UnityEngine;
using Unity.Netcode;

public class PlayerSpawnManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;
    private int nextSpawnIndex;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;
    }

    private void Singleton_OnClientConnectedCallback(ulong clientId)
    {
        Transform spawnPoint = spawnPoints[nextSpawnIndex % spawnPoints.Length];

        nextSpawnIndex++;

        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }
}
