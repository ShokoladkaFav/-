using UnityEngine;
using Mirror;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSpawner : NetworkManager
{
    public GameObject[] racePrefabs; // ����� ������� ��� ���

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // �������� username � PlayerPrefs (��� ������ �������)
        string username = PlayerPrefs.GetString("Username", "");
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("�������: Username �� ��������!");
            return;
        }

        StartCoroutine(GetPlayerRaceFromDB(conn, username));
    }

    private IEnumerator GetPlayerRaceFromDB(NetworkConnectionToClient conn, string username)
    {
        string url = "http://localhost/Kursach/get_race.php?username=" + username;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"������� ��������� ����: {webRequest.error}");
                yield break;
            }

            string jsonResult = webRequest.downloadHandler.text;
            RaceData raceData = JsonUtility.FromJson<RaceData>(jsonResult);

            if (!string.IsNullOrEmpty(raceData.error))
            {
                Debug.LogError($"������� ��������� ����: {raceData.error}");
                yield break;
            }

            int raceIndex = raceData.race_id - 1; // �������� ID ���� � ������ ������

            if (raceIndex < 0 || raceIndex >= racePrefabs.Length)
            {
                Debug.LogError($"����������� ������ ����: {raceData.race_id}");
                yield break;
            }

            GameObject playerPrefab = racePrefabs[raceIndex];
            Vector3 spawnPosition = GetSpawnPosition();

            GameObject playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, playerInstance);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        return new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }

    [System.Serializable]
    private class RaceData
    {
        public int race_id;
        public string error;
    }
}
