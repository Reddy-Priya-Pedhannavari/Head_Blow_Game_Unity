
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class playerMoves : MonoBehaviour
{
    public Animator anim;
    public GameObject head, blue_obj, red_obj, player;
    public Text scoreText;
    public Text timeText;
    public Text scoretextPanel, timeTextPanel;
    public float Seconds;
    public int Hours;
    public int Minutes;
    public GameObject scorePanel;
    bool onground = true;
    float count = 0, positive_point_inst = 0, negative_point_inst = 0;
    int score = 0;
    bool gameOverBool = false;
    int gameover = 0;
    bool startGame = false;
    public GameObject clickToStart;

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startGame)
        {
            count += Time.deltaTime * 15f;
            positive_point_inst += Time.deltaTime * 20f;
            negative_point_inst += Time.deltaTime * 10f;
            if (!gameOverBool)
            {
                playerMovements();
                AutoheadIncrease();
                pointsInstantiating();
            }
            if (head.gameObject.transform.localScale.x >= 4f) // player floating 
            {
                rigidbody.useGravity = false;
                float yaxis = this.gameObject.transform.localPosition.y;
                yaxis += Time.deltaTime * 0.01f;
                this.transform.localPosition = new Vector3(this.transform.localPosition.x, yaxis, this.transform.localPosition.z);
                gameOverBool = true;
                gameOverMethod();
                //DestroyGameonjects();
                //Invoke(nameof(destroyPlayer), 15f);
            }
        }
        
    }

    public void StartGame()
    {
        StartTimer();
        startGame = true;
        clickToStart.SetActive(false);
    }

    void gameOverMethod()
    {
        if (gameover==0)
        {
            scoretextPanel.text = scoreText.text;
            timeTextPanel.text = timeText.text;
            scorePanel.SetActive(true);
            anim.SetBool("idle", true);
            anim.SetBool("walk", false);
            gameover = 1;
        }
        
    }
    void playerMovements()
    {

        if (Input.GetKey(KeyCode.UpArrow)) //forward
        {
            gameObject.transform.Translate(-Vector3.up * Time.deltaTime * 4f);
            gameObject.transform.Translate(Vector3.right * Time.deltaTime * 1.6f);
            anim.SetBool("idle", false);
            anim.SetBool("walk", true);
        }
        if (Input.GetKey(KeyCode.DownArrow)) //back
        {
            gameObject.transform.Translate(Vector3.up * Time.deltaTime * 4f);
            gameObject.transform.Translate(Vector3.left * Time.deltaTime * 1.6f);
            anim.SetBool("idle", false);
            anim.SetBool("walk", true);
        }
        if (Input.GetKey(KeyCode.RightArrow)) //right
        {
            gameObject.transform.Rotate(Vector3.forward * 2f);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) //left
        {
            gameObject.transform.Rotate(-Vector3.forward * 2f);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow)) //jump animation
        {
            anim.SetBool("idle", true);
            anim.SetBool("walk", false);
        }
        if (onground == true)
        {
            //jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidbody.AddForce(new Vector3(0, 7, 0), ForceMode.Impulse);
            }
        }
    }
    void AutoheadIncrease()
    {
        if (count >= 80f)//head size increasing
        {
            head.transform.localScale = new Vector3(head.transform.localScale.x + 0.2f, head.transform.localScale.y + 0.2f, head.transform.localScale.z + 0.2f);
            count = 0;
        }
    }
    void pointsInstantiating()
    {
        if (positive_point_inst >= 100f)//positive point instantiation time
        {
            float xpos = Random.Range(-16f, 15);
            float zpos = Random.Range(-15, 15);
            Vector3 position = new Vector3(xpos, -3.8f, zpos);
            Instantiate(blue_obj, position, Quaternion.identity);
            positive_point_inst = 0;
        }
        if (negative_point_inst >= 80f)//negative point instantiation time
        {
            float xpos = Random.Range(-16f, 15);
            float zpos = Random.Range(-15, 15);
            Vector3 position = new Vector3(xpos, -3.5f, zpos);
            Instantiate(red_obj, position, Quaternion.identity);
            negative_point_inst = 0;
        }
    }
    public void StartTimer()
    {
        StopAllCoroutines();
        StartCoroutine(TimerRoutine());
    }

    public void StopTimer()
    {
        StopAllCoroutines();
        GetCurrentTime();
    }

    private IEnumerator TimerRoutine()
    {
        Seconds = 0;
        Hours = 0;
        Minutes = 0;
        while (true)
        {
            Seconds += Time.deltaTime;
            if (Seconds >= 60)
            {
                Seconds -= 60.0f;
                Minutes += 1;
                if (Minutes >= 60)
                {
                    Minutes -= 60;
                    Hours += 1;
                }
            }

            GetCurrentTime();
            yield return null;
        }
    }

    public void GetCurrentTime()
    {
        timeText.text = $"Time : {Hours:00}:{Minutes:00}:{Seconds:00}";
    }


    void destroyPlayer()
    {
        Destroy(gameObject);
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            onground = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            onground = false;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            onground = true;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "positive") // catch positive points (blue sphere objects)
        {
            // decrease head size; 
            if (head.transform.localScale.x >= 1)
            {
                head.transform.localScale = new Vector3(head.transform.localScale.x - 0.2f, head.transform.localScale.y - 0.2f, head.transform.localScale.z - 0.2f);
            }
            Destroy(collision.gameObject);
            score++;
            scoreText.text = $"Score : {score}";
        }
        if (collision.gameObject.tag == "negative") // catch negative points (red sphere objects)
        {
            // increase head size; 
            head.transform.localScale = new Vector3(head.transform.localScale.x + 0.4f, head.transform.localScale.y + 0.4f, head.transform.localScale.z + 0.4f);
            Destroy(collision.gameObject);
        }
    }

    void DestroyGameonjects()
    {
        GameObject[] killEmAllpos;
        killEmAllpos = GameObject.FindGameObjectsWithTag("positive");
        for (int i = 0; i < killEmAllpos.Length; i++)
        {
            Destroy(killEmAllpos[i].gameObject);
        }
        GameObject[] killEmAll;
        killEmAll = GameObject.FindGameObjectsWithTag("negative");
        for (int i = 0; i < killEmAll.Length; i++)
        {
            Destroy(killEmAll[i].gameObject);
        }
    }
    public void ResetGame()
    {
        rigidbody.useGravity = true;
        anim.SetBool("idle", true);
        anim.SetBool("walk", false);
        score = 0;
        scoreText.text = $"Score : {score}";
        StartTimer();
        //Destroy(red_obj);
        //Destroy(blue_obj);
        DestroyGameonjects();
        scorePanel.SetActive(false);
        gameOverBool = false;
        gameover = 0;
        startGame = false;
        clickToStart.SetActive(true);
        head.transform.localScale = new Vector3(1f, 1f, 1f);
        player.transform.position = new Vector3(0f,0.006f,0f);
    }

    public void Exit()
    {
        Application.Quit(0);
    }
}
