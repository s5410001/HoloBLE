using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class BeaconCallback
{
    public string Uuid;
    public ushort MajorId;
    public ushort MinorId;
    public UnityEvent Callback;
}

public class BeaconHandler : MonoBehaviour {

    public List<BeaconCallback> BeaconCallbacks;

	// Use this for initialization
	void Start () {
        GetComponent<BeaconReceiver>().BeaconReceived += BeaconReceived;
	}

    private void BeaconReceived(Beacon obj)
    {
        foreach(var bc in BeaconCallbacks)
        {
            if(bc.Uuid == obj.Uuid.ToString() && bc.MajorId == obj.MajorId && bc.MinorId == obj.MinorId)
            {
                bc.Callback.Invoke();
            }
        }
    }
	
}
