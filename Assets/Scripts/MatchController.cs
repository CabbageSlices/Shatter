using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MatchProperties
{
    public float ballInitialSpeed;
    public int roundStartCountdownTimer;

    public Vector2 ballInitialSpawnPosition;
};

public enum Team
{
    LEFT,
    RIGHT
};

/*match: [
    1. reset scores (PREGAME)
    2. round: [
        0. reset position of players and ball
        1. start countdown
        2. start playing => allow movement, ball has initial velocity
        3. someone scores => effects/timer
    ] until winning score is reached
    3. match end => display winning score etc, end or restart match.
]

*/
public class MatchController : MonoBehaviour
{

    [SerializeField]
    GameObject ballPrefab;
    GameObject ball;

    [SerializeField]


    public enum MatchState
    {
        PREGAME,
        COUNTDOWN,
        PLAYING,
        SCORING,
        ENDING,
        STOPPED,
    };

    [SerializeField]
    MatchProperties properties = new MatchProperties { ballInitialSpeed = 10, roundStartCountdownTimer = 3, ballInitialSpawnPosition = Vector2.zero };

    MatchState state = MatchState.PREGAME;

    GameObject leftGoalPost;
    GameObject rightGoalPost;

    float timeSinceCountdownStarted;
    float timeSincePlayStarted;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (state == MatchState.COUNTDOWN)
        {
            timeSinceCountdownStarted += Time.deltaTime;

            if (timeSinceCountdownStarted > properties.roundStartCountdownTimer)
            {
                startPlaying();
            }
            else
            {
                displayCurrentCountdownNumber();
            }
        }
        else
        if (state == MatchState.PLAYING)
        {
            if (ball.transform.position.x > rightGoalPost.transform.position.x)
            {
                onTeamScored(Team.LEFT);
            }
            else if (ball.transform.position.x < leftGoalPost.transform.position.x)
            {
                onTeamScored(Team.RIGHT);
            }
        }
    }

    private void Awake()
    {
        leftGoalPost = GameObject.FindWithTag("leftGoalPost");
        rightGoalPost = GameObject.FindWithTag("rightGoalPost");
    }

    public void startMatch()
    {
        resetRound();
    }

    public void resetRound()
    {
        resetBall();
        startRoundCountdown();
    }

    void startRoundCountdown()
    {
        state = MatchState.COUNTDOWN;
        timeSinceCountdownStarted = 0;
        displayCountdownMessage(properties.roundStartCountdownTimer.ToString());
    }

    public void startPlaying()
    {
        timeSincePlayStarted = 0;
        displayCountdownMessage("START");
        setBallInitialVelocity();
        state = MatchState.PLAYING;
    }

    public void stopMatch()
    {
        hideCountdownMessage();
        state = MatchState.STOPPED;
    }

    public void displayCurrentCountdownNumber()
    {
        int countdownNumber = properties.roundStartCountdownTimer - (int)timeSinceCountdownStarted;
        displayCountdownMessage(countdownNumber);
    }

    public void displayCountdownMessage(string message)
    {
        Debug.Log(message);
    }

    public void displayCountdownMessage(int message)
    {
        displayCountdownMessage(message.ToString());
    }

    public void hideCountdownMessage()
    {

    }

    public void onTeamScored(Team team)
    {
        //do something about graphics
        //idk wait for a bit now
        //restart the round
        //if (scoreThresholdReached) { onTeamWin(team); }

        resetRound();
    }

    void setBallInitialVelocity()
    {
        ball.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle.normalized * properties.ballInitialSpeed;
    }

    void resetBall()
    {
        if (!ball)
        {
            spawnBall();
        }

        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ball.GetComponent<Rigidbody2D>().position = properties.ballInitialSpawnPosition;
    }

    void spawnBall()
    {
        if (!ball)
        {
            ball = GameObject.FindGameObjectWithTag("ball");
        }
        if (!ball)
        {
            GameObject.Instantiate(ballPrefab);
        }
    }
}
