using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManagerGame : MonoBehaviour
{
    // Ojetos de la escena
    [SerializeField] Bolo[] bolos;
    [SerializeField] Bocha bocha;
    [SerializeField] EndTable endTable;
    [SerializeField] GuiaLine guiaLine;
    [SerializeField] GameObject popUpFinish;

    [Space]

    [SerializeField] PowerBar powerBar;
    [SerializeField] Text rounds;
    [SerializeField] Text points;
    [SerializeField] Text record;

    [Space]

    [SerializeField] float maxPower;
    private bool isCharging = false;
    private float currentPower = 0f;


    [Space]

    //Contador
    [SerializeField] float timeRestarShoot;
    [SerializeField] int totalShoot;
    private float currentLastTimeShoot = 0f;
    private bool isWaiting = true;
    private int currentShoot = 1;
    private int currentPoints = 0;
    private float lastRecodsPoints = 0f;

    [Space]
    private string playerPrefRecord = "record";


    void Start()
    {
        Bolo.OnAddPoints += AddPoints;
        EndTable.NextRound += NextRound;
        StartGame();

    }

    // Update is called once per frame
    void Update()
    {
        WaitingShoot();
    }
    private void StartGame()
    {
        lastRecodsPoints = PlayerPrefs.GetFloat(playerPrefRecord, 0);
        rounds.text = $"Ronda {currentShoot}/{totalShoot}";
        points.text = $"Points {currentPoints}";
        record.text = $"Record: {lastRecodsPoints}";
        
    }
    private void AddPoints(int point)
    {
        currentPoints += point;
        points.text = $"Points {currentPoints}";
    }
    private void WaitingShoot()
    {
        //aca debría controlar que numero de
        //disparo es y si la bola esta en movimiento.
        if (isWaiting)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCharging();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                StopCharging();
                LaunchBall();
            }
        }
        else
        {
            if(currentLastTimeShoot< timeRestarShoot)
            {
                currentLastTimeShoot += Time.deltaTime;
            }
            else{
                NextRound();
            }
        }
    }
    private void StartCharging()
    {
        isCharging = true;
              
    }
    public void NextRound()
    {        
        if (currentShoot < totalShoot)
        {
            currentShoot++;
            ResetRound();
            isWaiting = true;
            //al haber una ronda nueva se pone en espera del usuario
        }
        else
        {
            EndGame();
        }
    }
   
    private void StopCharging()
    {
        isCharging = false;
        isWaiting = false;
        
    }

    private void LaunchBall()
    {
        float forceKick = currentPower * maxPower;
        Vector3 forceAngle = guiaLine.gameObject.transform.forward;
        forceAngle *= forceKick;
        bocha.GetComponent<Rigidbody>().AddForce(forceAngle, ForceMode.Impulse);
        guiaLine.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isCharging)
        {
            if ((currentPower + Time.fixedDeltaTime) < 1f)
            {
                currentPower += Time.fixedDeltaTime;
                powerBar.slider.value = currentPower;
            }
            else
            {
                currentPower = Random.Range(0f, 1f);
            }
        }
    }
    private void ResetRound()
    {
        rounds.text = $"Ronda {currentShoot}/{totalShoot}";
        Destroy(bocha.gameObject);
        bocha = Instantiate(
                 bocha,
                 new Vector3(
                     0, 0.5f, 0
                 ),
                 Quaternion.Euler(0, 0, 0)
             );
        bocha.transform.position = new Vector3(0, 0.5f, 0);
        currentPower = 0;
        powerBar.slider.value = currentPower;
        currentLastTimeShoot = 0;
        guiaLine.gameObject.SetActive(true);
    }
    private void EndGame()
    {
        if(currentPoints > lastRecodsPoints)
        {
            Congratulations();
        }

        popUpFinish.SetActive(true);
    }
    private void Congratulations()
    {
        lastRecodsPoints = currentPoints;
        record.text = $"Nuevo Record: {lastRecodsPoints}!";
        PlayerPrefs.SetFloat(playerPrefRecord, currentPoints);

    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Bowling");
    }
}
