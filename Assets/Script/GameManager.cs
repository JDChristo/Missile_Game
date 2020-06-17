using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private bool isGameOver;
    public bool IsGameOver { get { return isGameOver; } }

    [SerializeField]
    private GameControllerOption gameControl = GameControllerOption.SIDE_TOUCH;
    public GameControllerOption GameControl { get { return gameControl; } }

    [SerializeField]
    private GameObject plane = null;
    [SerializeField]
    private GameObject joyStick = null;
    [SerializeField]
    private GameObject sideIndicator = null;
    [SerializeField]
    private GameObject missilePrefab = null;
    [SerializeField]
    private GameObject starPrefab = null;
    [SerializeField]
    private Vector2 minMaxMissileLifeTime;
    [SerializeField]
    private TextMeshProUGUI coinText = null;
    [SerializeField]
    private TextMeshProUGUI scoreText = null;

    [Header("All Canvas")]
    [SerializeField]
    private GameObject runtime_Canvas = null;
    [SerializeField]
    private GameObject pauseMenu_Canvas = null;
    [SerializeField]
    private GameObject mainMenu_Canvas = null;
    [SerializeField]
    private GameObject gameOver_Canvas = null;

    [Header("Controller UI")]
    [SerializeField]
    private Sprite sideUI;
    [SerializeField]
    private Sprite joyStickUI;
    [SerializeField]
    private Image controllerUI;

    private bool uiImageSwitch = false;
    private Transform target;
    private VirtualJoystick joyStickScript;
    private Vector3 respawnLocation;
    private Vector3 prevLoc = Vector3.zero;
    public Transform Target { get => target; }
    public Vector2 PlaneDirection { get; private set; }

    private void Awake()
    {
        _instance = this;
        isGameOver = true;
        
    }
    void Start()
    {
        joyStickScript = joyStick.GetComponentInChildren<VirtualJoystick>();
        CreatePlane(Vector3.zero);
        SwitchCanvas(mainMenu_Canvas);
    }
    private void Update()
    {
        if (target != null)
        {
            Vector2 pos = (target.position - prevLoc) / Time.deltaTime;
            PlaneDirection = new Vector2(Mathf.Clamp(pos.x, -1, 1), Mathf.Clamp(pos.y, -1, 1));
            prevLoc = target.position;
        }
    }
    public void CreatePlane(Vector3 pos)
    {
        target = Instantiate(plane, pos, plane.transform.rotation).transform;
        target.gameObject.GetComponent<PlayerController>().joyStick = joyStickScript;
    }
    public void ControlSetting()
    {
        uiImageSwitch = !uiImageSwitch;
        if(uiImageSwitch)
        {
            controllerUI.sprite = joyStickUI;
            gameControl = GameControllerOption.VIRTUAL_PAD;
        }
        else
        {
            controllerUI.sprite = sideUI;
            gameControl = GameControllerOption.SIDE_TOUCH;
        }

    }

    public void SwitchControls(GameControllerOption controllerOption)
    {
        switch(controllerOption)
        {
            case GameControllerOption.SIDE_TOUCH:
                sideIndicator.SetActive(true);
                joyStick.SetActive(false);
                break;
            case GameControllerOption.VIRTUAL_PAD:
                sideIndicator.SetActive(false);
                joyStick.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void OnPlay()
    {
        Time.timeScale = 1;
        isGameOver = false;
        if(!target)
        {
            CreatePlane(respawnLocation);
        }
        SwitchCanvas(runtime_Canvas);
        SwitchControls(gameControl);
        StartCoroutine(SpawnMissile());
        SpawnStar();
    }
    public void GameOverPlay()
    {
        SwitchCanvas(mainMenu_Canvas);
    }
    public void OnPauseResume(float time)
    {
        Time.timeScale = time;
        if(time == 1)
        {
            SwitchCanvas(runtime_Canvas);
        }
        if(time == 0)
        {
            SwitchCanvas(pauseMenu_Canvas);
        }
    }

    public void GameOver(Transform obj)
    {
        respawnLocation = obj.position;
        isGameOver = true;
        SwitchCanvas(gameOver_Canvas);
        scoreText.text = coinText.text;
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }
    IEnumerator SpawnMissile()
    {
        while (!IsGameOver)
        {
            int j = (target.rotation.z < 180) ? 8 : -8;
            int i = (target.rotation.z < 180) ? 10 : -10;

            Vector3 spawnLocation = target.position + new Vector3(Random.Range(j, i), Random.Range(j, i), 0f);
            GameObject tempMissile = Instantiate(missilePrefab, spawnLocation, missilePrefab.transform.rotation,transform);
            tempMissile.GetComponent<MissileBehaviour>().target = target;
            tempMissile.GetComponent<MissileBehaviour>().lifeTime = Random.Range(minMaxMissileLifeTime.x, minMaxMissileLifeTime.y);
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }
    public void SpawnStar()
    {
        if (!IsGameOver)
        {
            int j = (target.rotation.z < 180) ? 1 : -1;
            int i = (target.rotation.z < 180) ? 4 : -4;
            Vector3 spawnLocation = target.position + new Vector3(Random.Range(j, i), Random.Range(j, i), 0f);
            GameObject tempStar = Instantiate(starPrefab, spawnLocation, starPrefab.transform.rotation, transform);
        }
    }

    public void SwitchCanvas(GameObject canvas)
    {
        runtime_Canvas.SetActive(false);
        pauseMenu_Canvas.SetActive(false);
        mainMenu_Canvas.SetActive(false);
        gameOver_Canvas.SetActive(false);

        canvas.SetActive(true);
    }
    public void ScoreCalulator(float score)
    {
        coinText.text = score.ToString();
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}

[System.Serializable]
public enum GameControllerOption
{
    VIRTUAL_PAD,
    SIDE_TOUCH,
    FOLLOW_FINGER,
    NONE
}
