using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PLAYERTWO.PlatformerProject;
using TMPro;
using System.Data;
using Unity.VisualScripting;

public class GameManagerSingle : GameManager
{

    [SerializeField] private float minRearrangeBoxesTime;
    [SerializeField] private float maxRearrangeBoxesTime;
    [SerializeField] private TMP_Text roundTimer;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private int roundTime;
    [SerializeField] private PlayerInputManager playerInput;

    private WaitForSecondsRealtime celebrationWait;
    private WaitForSecondsRealtime prepareWait;
    private WaitForSecondsRealtime indicateWait;

    private WaitForSecondsRealtime aSecondWait = new WaitForSecondsRealtime(1);
    IEnumerator rearrangeRoutine;
    IEnumerator roundTimerRoutine;

    int levelCounter = 0;

    protected override void Awake()
    {
        celebrationWait = new WaitForSecondsRealtime(celebrationDelay);
        prepareWait = new WaitForSecondsRealtime(prepareDelay);
        indicateWait = new WaitForSecondsRealtime(indicateDelay);

        Instance = this;
    }

    protected override void Start()
    {
        foreach (Player p in FindObjectsOfType<Player>())
        {
            p.SetRespawn(p.transform.position, p.transform.rotation);
        }

        player2.gameObject.SetActive(false);
        StartCoroutine(WinCoroutine(false));


    }

    public override void Win()
    {
        StartCoroutine(WinCoroutine());
    }




    IEnumerator RearrangeBoxesRoutine(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        RearrangeBoxes();
        rearrangeRoutine = RearrangeBoxesRoutine(Random.Range(minRearrangeBoxesTime, maxRearrangeBoxesTime));
        StartCoroutine(rearrangeRoutine);
    }
    void RearrangeBoxes()
    {
        OptionManager.Instance.DestroyCurrentOptionBoxes();
        OptionManager.Instance.CreateOptions();
    }
    IEnumerator<WaitForSecondsRealtime> RoundTimeRoutine() 
    {
        yield return aSecondWait;
        playerInput.enabled = true;

        for (int i = roundTime; i > 0; i--)
        {
            roundTimer.text = "" + i;
            yield return aSecondWait;
        }
        StartCoroutine(LoseCoroutine());
    }
    IEnumerator<WaitForSecondsRealtime> LoseCoroutine()
    {
        /*if (rearrangeRoutine != null)
            StopCoroutine(rearrangeRoutine);

        rearrangeRoutine = null;*/
        playerInput.enabled = false;
        if (roundTimerRoutine != null)
            StopCoroutine(roundTimerRoutine);

        roundTimer.text = "....";
        levelText.text = "_______";
        roundTimerRoutine = null;

        yield return celebrationWait;



        player2.gameObject.SetActive(false);


        player2Anim.SetActive(true);

        player2Anim.transform.position = player2.position + Vector3.up * yDifferenceAnim;



        player2Anim.GetComponent<Animation>().Play("lose");


        Destroy(
                ParticleFXManager.Instance.CreateParticleFX("SadEmoji", player2.transform.position + Vector3.up * 4.2f).GetComponent<AudioSource>());




        yield return prepareWait;

        ParticleFXManager.Instance.CreateParticleFX("HidePlayer", player2Anim.transform.position + Vector3.back);

        player2Anim.SetActive(false);



        OptionManager.Instance.DestroyCurrentOptionBoxes();


        player2.gameObject.SetActive(true);

        foreach (Player p in FindObjectsOfType<Player>())
        {
            p.Respawn();
        }

        ParticleFXManager.Instance.CreateParticleFX("ResetPlayer", player2.transform.position + Vector3.back);

        yield return indicateWait;

        OptionManager.Instance.CreateOptions();
        levelText.text = "Seviye " + levelCounter;
        

        yield return indicateWait;

        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            ParticleFXManager.Instance.CreateParticleFX("EnemyIndicator", enemy.transform.Find("FirePoint"), Vector3.zero);
        }

        foreach (Breakable box in FindObjectsOfType<Breakable>())
        {
            ParticleFXManager.Instance.CreateParticleFX("BoxIndicator", box.transform, Vector3.forward);
        }

        /*rearrangeRoutine = RearrangeBoxesRoutine(Random.Range(minRearrangeBoxesTime, maxRearrangeBoxesTime));
        StartCoroutine(rearrangeRoutine);*/
        roundTimerRoutine = RoundTimeRoutine();
        StartCoroutine(roundTimerRoutine);
    }

    protected override IEnumerator<WaitForSecondsRealtime> WinCoroutine(bool celebrate = true)
    {
        /*if (rearrangeRoutine != null)
            StopCoroutine(rearrangeRoutine);

        rearrangeRoutine = null;*/

        playerInput.enabled = false;
        if (roundTimerRoutine != null)
            StopCoroutine(roundTimerRoutine);
        
        roundTimer.text = "....";
        levelText.text = "_______";
        roundTimerRoutine = null;

        yield return celebrationWait;

        if (celebrate)
        {

            player2.gameObject.SetActive(false);


            player2Anim.SetActive(true);

            player2Anim.transform.position = player2.position + Vector3.up * yDifferenceAnim;


            Transform breaker = OptionManager.Instance.GetCorrectBox().GetComponent<Breakable>().GetBreaker();



            player2Anim.GetComponent<Animation>().Play("win");


            ParticleFXManager.Instance.CreateParticleFX("Confetti", player2.transform.position);
            ParticleFXManager.Instance.CreateParticleFX("HappyEmoji", player2.transform.position + Vector3.up * 4.2f);




            yield return prepareWait;

            ParticleFXManager.Instance.CreateParticleFX("HidePlayer", player2Anim.transform.position + Vector3.back);

            player2Anim.SetActive(false);
        }


        OptionManager.Instance.DestroyCurrentOptionBoxes();
        QuestionManager.Instance.MoveNextQuestion();

        levelText.text = "Seviye " + (++levelCounter);
        player2.gameObject.SetActive(true);

        foreach (Player p in FindObjectsOfType<Player>())
        {
            p.Respawn();
        }

        ParticleFXManager.Instance.CreateParticleFX("ResetPlayer", player2.transform.position + Vector3.back);

        yield return indicateWait;

        OptionManager.Instance.CreateOptions();

        yield return indicateWait;

        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            ParticleFXManager.Instance.CreateParticleFX("EnemyIndicator", enemy.transform.Find("FirePoint"), Vector3.zero);
        }

        foreach (Breakable box in FindObjectsOfType<Breakable>())
        {
            ParticleFXManager.Instance.CreateParticleFX("BoxIndicator", box.transform, Vector3.forward);
        }

        //rearrangeRoutine = RearrangeBoxesRoutine(Random.Range(minRearrangeBoxesTime, maxRearrangeBoxesTime));
        //StartCoroutine(rearrangeRoutine);
        roundTimerRoutine = RoundTimeRoutine();
        StartCoroutine(roundTimerRoutine);
    }

}
