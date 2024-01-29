using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private GameObject obj;
    [SerializeField] private float dishJudgeDelay = 3f;
    private Collider2D _collider;
    private bool Clicking{get{return GameManager.Clicking;} set{GameManager.Clicking = value;}}

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;
    }
    private void Update() 
    {
        if(Clicking)
        {
            _collider.enabled = false;
        }
        else
        {
            _collider.enabled = true;
        }
    }

    IEnumerator JudgeItem()
    {
        yield return new WaitForSeconds(dishJudgeDelay);
        GameManager.StartEndSequence(obj);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.name);
        if(other.gameObject.CompareTag("Skillet"))
        {
            var skillet = other.gameObject.GetComponent<Skillet>(); 
            if(skillet.HasObject)
            {
                obj = skillet.ChangeObjectOwner(this);
                skillet.ResetPosition();
                GameManager.IsStoveOn = false;
                StartCoroutine(JudgeItem());
            }
        }
    }
}
