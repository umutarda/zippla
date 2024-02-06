using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PLAYERTWO.PlatformerProject;
using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField] protected Transform player1;
    [SerializeField] protected Transform player2;

    [SerializeField] protected GameObject player1Anim;
    [SerializeField] protected GameObject player2Anim;
    [SerializeField] protected GameObject debugButton;

    [SerializeField] protected float celebrationDelay = 2f;
    [SerializeField] protected float prepareDelay = 2f;
    [SerializeField] protected float indicateDelay = 2f;

    [SerializeField] protected float yDifferenceAnim = -2f;

    


    private WaitForSecondsRealtime celebrationWait;
    private WaitForSecondsRealtime prepareWait;
    private WaitForSecondsRealtime indicateWait;
    public static GameManager Instance;

    
    protected virtual void Awake()
    {
        celebrationWait = new WaitForSecondsRealtime(celebrationDelay);
        prepareWait = new WaitForSecondsRealtime(prepareDelay);
        indicateWait = new WaitForSecondsRealtime(indicateDelay);

        Instance = this;
    }

    protected virtual void Start()
    {
        foreach (Player p in FindObjectsOfType<Player>())
        {
            p.SetRespawn(p.transform.position, p.transform.rotation);
        }

        player1.gameObject.SetActive(false);
        player2.gameObject.SetActive(false);
        StartCoroutine(WinCoroutine(false));


    }

    public virtual void Win()
    {
        StartCoroutine(WinCoroutine());
    }

    public void CheckForTotalDeath()
    {
        foreach (Player p in FindObjectsOfType<Player>())
        {
            if (p.isAlive) return;
        }

        StartCoroutine(WinCoroutine(false));

    }


    protected virtual IEnumerator<WaitForSecondsRealtime> WinCoroutine(bool celebrate = true)
    {
        yield return celebrationWait;

        if (celebrate)
        {
            player1.gameObject.SetActive(false);
            player2.gameObject.SetActive(false);

            player1Anim.SetActive(true);
            player2Anim.SetActive(true);

            player1Anim.transform.position = player1.position + Vector3.up * yDifferenceAnim;
            player2Anim.transform.position = player2.position + Vector3.up * yDifferenceAnim;


            Transform breaker = OptionManager.Instance.GetCorrectBox().GetComponent<Breakable>().GetBreaker();
            if (breaker == player1.transform)
            {
                player1Anim.GetComponent<Animation>().Play("win");
                player2Anim.GetComponent<Animation>().Play("lose");

                ParticleFXManager.Instance.CreateParticleFX("Confetti", player1.transform.position);
                ParticleFXManager.Instance.CreateParticleFX("HappyEmoji", player1.transform.position + Vector3.up * 4.2f);

                Destroy(
                        ParticleFXManager.Instance.CreateParticleFX("SadEmoji", player2.transform.position + Vector3.up * 4.2f).GetComponent<AudioSource>());
            }

            else
            {
                player2Anim.GetComponent<Animation>().Play("win");
                player1Anim.GetComponent<Animation>().Play("lose");

                ParticleFXManager.Instance.CreateParticleFX("Confetti", player2.transform.position);
                ParticleFXManager.Instance.CreateParticleFX("HappyEmoji", player2.transform.position + Vector3.up * 4.2f);

                Destroy(
                 ParticleFXManager.Instance.CreateParticleFX("SadEmoji", player1.transform.position + Vector3.up * 4.2f).GetComponent<AudioSource>());

            }

            yield return prepareWait;

            ParticleFXManager.Instance.CreateParticleFX("HidePlayer", player1Anim.transform.position + Vector3.back);
            ParticleFXManager.Instance.CreateParticleFX("HidePlayer", player2Anim.transform.position + Vector3.back);

            player1Anim.SetActive(false);
            player2Anim.SetActive(false);
        }


        OptionManager.Instance.DestroyCurrentOptionBoxes();
        QuestionManager.Instance.MoveNextQuestion(debugButton.activeInHierarchy);

        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);

        foreach (Player p in FindObjectsOfType<Player>())
        {
            p.Respawn();
        }

        ParticleFXManager.Instance.CreateParticleFX("ResetPlayer", player1.transform.position + Vector3.back);
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

    }

}
