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

        if (!object1)
            object1 = inst.object1;
        if (!object2)
            object2 = inst.object2;

        if (object1)
        {
            if (inst.object1 != object1)
                Destroy(inst.object1);
            DontDestroyOnLoad(object1);
        }

        if (object2)
        {
            if (inst.object2 != object2)
                Destroy(inst.object2);
            DontDestroyOnLoad(object2);
        }

        inst.object1 = object1;
        inst.object2 = object2;

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