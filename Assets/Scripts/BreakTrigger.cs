using UnityEngine;
using PLAYERTWO.PlatformerProject;
public class BreakTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Breakable breakableObj = other.GetComponent<Breakable>();
        if (breakableObj != null)
        {
            breakableObj.Break(transform.root);

            if (breakableObj.gameObject != OptionManager.Instance.GetCorrectBox())  
            {
                ParticleFXManager.Instance.CreateParticleFX("SadEmoji",transform.root,Vector3.up*4.2f);
                transform.root.GetComponent<Player>().Hurt();
            }
                
        
        }
                    
    }
}
