using UnityEngine;
using Mirror;

public class AutoConnect : MonoBehaviour
{
    private void Start()
    {
        if (NetworkServer.active || NetworkClient.active)
        {
            Debug.Log("Мережеве з'єднання вже активне!");
            return;
        }

        if (IsFirstPlayer())
        {
            Debug.Log("Запуск сервера...");
            NetworkManager.singleton.StartHost(); // Перший гравець — хост
        }
        else
        {
            Debug.Log("Підключення до сервера...");
            NetworkManager.singleton.StartClient(); // Інші гравці підключаються
        }
    }

    private bool IsFirstPlayer()
    {
        // Перевіряємо, чи є активний сервер (якщо ні — значить, ми перший гравець)
        return !NetworkClient.isConnected && !NetworkServer.active;
    }
}
