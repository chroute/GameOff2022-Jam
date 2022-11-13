using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace GO22
{
    public class cinemachine_setup : MonoBehaviour
    {
        CinemachineVirtualCamera followedObject;
        private const string ROCK = "Rock";


        void Awake()
        {
            followedObject = GetComponent<CinemachineVirtualCamera>();
            followedObject.m_Follow = GameObject.FindGameObjectsWithTag(ROCK)[0].transform;
        }

    }
}
