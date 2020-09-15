using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeyCtr : MonoBehaviour
{
    public int mDirect=-1;

    public void SetDirect(int direct)
    {
        mDirect = direct;
    }

    private void Update()
    {
        if (mDirect == 0)
            transform.Translate(Vector3.left * Time.deltaTime * 2);
        else if (mDirect == 1)
            transform.Translate(Vector3.right * Time.deltaTime * 2);
        else
        {

        }

    }

}
