using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public enum GameState
    {
        NOT_INITIALIZED,
        INITIALIZED,
        PLAYING,
    };

    GameState state = GameState.NOT_INITIALIZED;

    [SerializeField]
    GameObject matchControllerPrefab;

    GameObject matchController;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void beginMatch()
    {
        if (!matchController)
        {
            matchController = GameObject.FindWithTag("matchController");
        }

        if (!matchController)
        {
            matchController = GameObject.Instantiate(matchControllerPrefab);
        }

        matchController.GetComponent<MatchController>().startMatch();
    }

    public void endMatch()
    {

        if (matchController)
        {
            matchController.GetComponent<MatchController>().stopMatch();
        }
    }
}
