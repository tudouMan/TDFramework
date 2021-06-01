using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TDFramework;
using System.Text;
using System.Net;
using TDFramework.EventSystem;
using TDFramework.Extention;


public class TempTest : MonoBehaviour
{

    public readonly int mFixedFrameTime = 10;
    public int mCurFrameIndexTime;
    public int mCurFrameIndex;
    private Dictionary<int, TestFrameProto> mAllFrameOperations = new Dictionary<int, TestFrameProto>();

    TestFrameProto mLastFrame=new TestFrameProto {FrameIndex=0,mDirect=-1,ID=1 };
    private PlayeyCtr playerCtr;

    bool isStart = false;


    ushort protoType = 30000;
    private void Awake()
    {
        playerCtr = GetComponent<PlayeyCtr>();

    }

    private void AcceptMsg(byte[] args)
    {
        TestFrameProto acceptProto=TestFrameProto.GetProto(args);
        if (acceptProto.FrameIndex == -1)
        {
            //无效帧
            //直接恢复上一帧
            playerCtr.SetDirect(mLastFrame.mDirect);
            Debug.Log($"玩家{acceptProto.ID} 收到无效帧 执行上次帧{mLastFrame.FrameIndex} 方向为{mLastFrame.mDirect}");

        }
        else
        {
            playerCtr.SetDirect(acceptProto.mDirect);
            mLastFrame = acceptProto;
            Debug.Log($"玩家{acceptProto.ID} 收到{acceptProto.FrameIndex}帧 执行 方向为{acceptProto.mDirect}");
        }
    }




    private void OnDestroy()
    {
       // EventCenter.RemoveListener<Byte[]>(protoType, AcceptMsg);
    }

    public void OnGUI()
    {
        if(GUI.Button(new Rect(100, 100, 100, 100), "connect"))
        {
            NetWorkSocket.Instance.Connect("192.168.2.188", 1015);
        }

        if (GUI.Button(new Rect(100, 200, 100, 100), "start"))
        {
            isStart = true;
        }
    }


    // 0 帧开始 
    private void FixedUpdate()
    {
        if (isStart)
        {
            if (mCurFrameIndexTime >= mFixedFrameTime)
            {

                //Send CurFrame Operation
                if (mAllFrameOperations.ContainsKey(mCurFrameIndex))
                {
                    NetWorkSocket.Instance.SendMsg(mAllFrameOperations[mCurFrameIndex].ToArray());
                }
                else
                {
                    //Send CurFrame Operation null frame
                    NetWorkSocket.Instance.SendMsg(mLastFrame.ToArray());
                }
               
              
                mCurFrameIndexTime = 0;
                mCurFrameIndex++;
            }
            else
            {
                mCurFrameIndexTime++;
            }
        }
       
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

            TestFrameProto operation = new TestFrameProto { FrameIndex= mCurFrameIndex ,mDirect=0,ID=1};
            if (!mAllFrameOperations.ContainsKey(mCurFrameIndex))
            {
                mAllFrameOperations.Add(mCurFrameIndex, operation);
            }
            mAllFrameOperations[mCurFrameIndex] = operation;

        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            TestFrameProto operation = new TestFrameProto { FrameIndex = mCurFrameIndex, mDirect = 1,ID=1 };
            if (!mAllFrameOperations.ContainsKey(mCurFrameIndex))
            {
                mAllFrameOperations.Add(mCurFrameIndex, operation);
            }
            mAllFrameOperations[mCurFrameIndex] = operation;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {

            TestFrameProto operation = new TestFrameProto { FrameIndex = mCurFrameIndex, mDirect = -1,ID=1 };
            if (!mAllFrameOperations.ContainsKey(mCurFrameIndex))
            {
                mAllFrameOperations.Add(mCurFrameIndex, operation);
            }
            mAllFrameOperations[mCurFrameIndex] = operation;


        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            TestFrameProto operation = new TestFrameProto { FrameIndex = mCurFrameIndex, mDirect = -1,ID=1 };
            if (!mAllFrameOperations.ContainsKey(mCurFrameIndex))
            {
                mAllFrameOperations.Add(mCurFrameIndex, operation);
            }
            mAllFrameOperations[mCurFrameIndex] = operation;
        }
    }
}


