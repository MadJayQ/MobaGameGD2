using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

class CustomNetworkTransform : NetworkBehaviour {
    private void FixedUpdateClient() {

    }

    private void FixedUpdateServer() {

    }

    void FixedUpdate() {
        if(isServer) {
            FixedUpdateServer();
        }
        if(isClient) {
            FixedUpdateClient();
        }
    }
}