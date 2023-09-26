using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyJoiner : MonoBehaviourPunCallbacks
{
    public string lobbyName = "DefaultLobby";

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Photon sunucusuna baðlan
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby(new TypedLobby(lobbyName, LobbyType.Default));
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobiye katýldýnýz: " + lobbyName);
    }
}
