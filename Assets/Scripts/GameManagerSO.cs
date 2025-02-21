using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Scriptable Objects/GameManager")]
public class GameManagerSO : ScriptableObject
{
    private Player player;
    private Vector3 newPosition = new Vector3(-1.921f, 0.897f,0f);
    private Vector2 newOrientation = new Vector2(0,-1);

    public Vector3 NewPosition { get => newPosition; }
    public Vector2 NewOrientation { get => newOrientation;}

    private void OnEnable() //Llamadas por EVENTO.
    {
        SceneManager.sceneLoaded += NuevaEscenaCargada;
    }

    private void NuevaEscenaCargada(Scene arg0, LoadSceneMode arg1)
    {
        player = FindObjectOfType<Player>(); //Buscar instancia directamente;
    }
    //----------------------------GUILLERMO--------------------------------
    public void LoadNewScene(Vector3 newPosition, Vector2 newOrientation, int newSceneIndex){
        this.newPosition = newPosition;
        this.newOrientation = newOrientation;
        SceneManager.LoadScene(newSceneIndex);

    }
    //---------------------------------------------------------------------

    public void CambiarEstadoPlayer(bool estado)
    {
        player.Interactuando = !estado;
    }
}
