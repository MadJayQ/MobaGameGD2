using System;

using UnityEngine;

public class WaypointGenerator : MonoBehaviour {

    public bool ShouldGenerate = true;

    public bool PlotX = true;
    public bool PlotY = true;
    public bool MirrorX = true;
    public bool MirrorY = false;

    public Vector2 StartPoint = new Vector2(1, 0);
    public Vector2 EndPoint = new Vector2(-1, 0);
    public Vector2 Center = new Vector2(0, 0);
    public float Radius = 15f;

    public Transform[] transformsX;
    public Transform[] transformsY;

    public GameObject ParentTransform;

    void Update() {
        if(ShouldGenerate) {
            Generate();
            ShouldGenerate = false;
        }
    }

    void Generate() {
        var startPoint = new Vector2(1, 0);
        var endPoint = new Vector2(-1, 0);
        var numPoints = 20;
        var point = startPoint;
        var difference = startPoint - endPoint;
        for(var i = 1; i <= numPoints; i++) {
            if(PlotX) {
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                var posX = Radius * Mathf.Cos(Mathf.Acos(point.x));
                var posZ = Radius * Mathf.Sin(Mathf.Acos(point.x));
                sphere.name = "Sphere_x " + i;
                sphere.transform.parent = ParentTransform.transform;
                sphere.transform.position = new Vector3(posX, 0f, posZ);
                sphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                if(MirrorX) {
                    var sphereMirror = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphereMirror.name = "Mirror_Sphere_x " + i;
                    var posXMirror = Radius * Mathf.Cos(Mathf.Acos(point.x));
                    var posZMirror = Radius * Mathf.Sin(Mathf.Acos(point.x) + Mathf.PI);
                    sphereMirror.transform.parent = ParentTransform.transform;
                    sphereMirror.transform.position = new Vector3(posXMirror, 0f, posZMirror);
                    sphereMirror.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                }
            }
            if(PlotY) {
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                var posZ = Radius * Mathf.Cos(Mathf.Acos(point.x));
                var posY = Radius * Mathf.Sin(Mathf.Acos(point.x));
                sphere.name = "Sphere_y " + i;
                sphere.transform.parent = ParentTransform.transform;
                sphere.transform.position = new Vector3(0f, posY, posZ);
                sphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                if(MirrorY) {
                    var sphereMirror = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphereMirror.name = "Mirror_Sphere_y " + i;
                    var posZMirror = Radius * Mathf.Cos(Mathf.Acos(point.x));
                    var posYMirror = Radius * Mathf.Sin(Mathf.Acos(point.x) + Mathf.PI);
                    sphereMirror.transform.parent = ParentTransform.transform;
                    sphereMirror.transform.position = new Vector3(0f, posYMirror, posZMirror);
                    sphereMirror.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                }
            }
            point = point - (difference / numPoints);
        }
    }
}