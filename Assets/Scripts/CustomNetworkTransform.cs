using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

class CustomNetworkTransform : NetworkBehaviour {

    private Vector3 m_TargetPosition;
    private Vector3 m_PreviousPosition;

    private float m_LastClientSyncTime = 0f;

    private void FixedUpdateClient() {
        if(m_LastClientSyncTime == 0)
            return;
        if(!NetworkServer.active && !NetworkClient.active)
            return;
        if(!isServer && !isClient)
            return;
        if(hasAuthority)
            return;
        
        transform.position = m_TargetPosition;
    }
    
    private void Update() {
        if(isClient) {
            var elapsedTime = Time.time - m_LastClientSyncTime;
            Debug.Log(elapsedTime);
        }
    }

    private void FixedUpdateServer() {
        if(syncVarDirtyBits != 0)
            return;
        if(!NetworkServer.active)
            return;
        if(!isServer) 
            return;
        SetDirtyBit(1);
    }

    void FixedUpdate() {
        if(isServer) {
            FixedUpdateServer();
        }
        if(isClient) {
            FixedUpdateClient();
        }
    }

    public override void  OnDeserialize(NetworkReader reader, bool initialState) {
        if(isServer && NetworkServer.localClientActive) {
            return;
        }

        if(!initialState)
        {
            if(reader.ReadPackedUInt32() == 0) 
            return;
        }

        if(hasAuthority) {
            reader.ReadVector3();
        }

        if(isServer && !isClient) { 
            //Our server will not interpolate, only teleport
            transform.position = reader.ReadVector3();
        }

        m_TargetPosition = reader.ReadVector3();

        m_LastClientSyncTime = Time.time;
    }

    public override bool OnSerialize(NetworkWriter writer, bool initialState) {
        if(initialState) {

        }
        else if(syncVarDirtyBits == 0) {
            writer.WritePackedUInt32(0);
            return false;
        }
        else {
            writer.WritePackedUInt32(1);
        }
        writer.Write(transform.position);

        m_PreviousPosition = transform.position;
        return true;
    }
}