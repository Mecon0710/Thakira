using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class capsule : MonoBehaviour
{
    CapsuleCollider col; 
    public int minx = 15;
    public int miny = 10;
    public int minz = 25;

    public int maxx = -15;
    public int maxy = 15;
    public int maxz = -5;
    // Start is called before the first frame update
    void Start()
    {
    

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int getMinx(){

        return minx;
    }
    public int getMiny(){

        return miny;
    }
    public int getMinz(){

        return minz;
    }
    public int getMaxx(){

        return maxx;
    }
    public int getMaxy(){

        return maxy;
    }
    public int getMaxz(){

        return maxz;
    }
}
