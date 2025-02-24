using UnityEngine;
using Mirror;

public class AutoConnect : MonoBehaviour
{
    private void Start()
    {
        if (NetworkServer.active || NetworkClient.active)
        {
            Debug.Log("�������� �'������� ��� �������!");
            return;
        }

        if (IsFirstPlayer())
        {
            Debug.Log("������ �������...");
            NetworkManager.singleton.StartHost(); // ������ ������� � ����
        }
        else
        {
            Debug.Log("ϳ��������� �� �������...");
            NetworkManager.singleton.StartClient(); // ���� ������ ������������
        }
    }

    private bool IsFirstPlayer()
    {
        // ����������, �� � �������� ������ (���� � � �������, �� ������ �������)
        return !NetworkClient.isConnected && !NetworkServer.active;
    }
}
