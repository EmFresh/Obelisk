using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingStatic : MonoBehaviour
{
    public GameObject object1, object2;
    [HideInInspector] public static TestingStatic inst;

    // Start is called before the first frame update

    void Awake()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(inst);
        }
        singletonVarInit(object1,inst.object1);
        singletonVarInit(object2,inst.object2);

        

    }

    void singletonVarInit(Object ob,Object instOb)
    {
        if (!ob)
            ob = instOb;
              
        if (ob)
        {
            if (instOb != ob)
                Destroy(instOb);
            DontDestroyOnLoad(ob);
        }
        
        instOb = ob;
    }
    void Start()
    {
        Vector3 tmp = Vector3.zero;
        if (object1)
        {
            tmp = object1.transform.position;
            print(tmp);
        }
        if (object2)
        {
            tmp = object2.transform.position;
            print(tmp);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}