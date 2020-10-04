using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            GetComponent<Animator>().enabled = true;
            StartCoroutine(DisableMe());
        }
    }

    IEnumerator DisableMe()
    {
        yield return new WaitForSeconds(1.7f);
        this.gameObject.SetActive(false);
    }
}
