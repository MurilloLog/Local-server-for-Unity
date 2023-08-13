using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearServerSettings : MonoBehaviour
{
    public static ServerSettings serverSettings;

    void Awake()
    {
        if(serverSettings==null || serverSettings!=this)
            Destroy(gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
