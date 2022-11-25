using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraScript : MonoBehaviour
{
    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
    public static CinemachineVirtualCamera ActiveCamera = null;

    public static bool isActiveCam(CinemachineVirtualCamera camera){
         return camera == ActiveCamera; 
    }
    public static void SwitchCam(CinemachineVirtualCamera camera){
        camera.Priority = 10;
        ActiveCamera = camera;

        foreach (CinemachineVirtualCamera c in cameras)
        {
            if (c != camera && c.Priority!=0)
            {
                c.Priority=0;
            }
        }
    }
    public static void Register(CinemachineVirtualCamera camera){

        cameras.Add(camera);
        Debug.Log("added");
    }

    public static void Unregister(CinemachineVirtualCamera camera){
        cameras.Remove(camera);
        Debug.Log("removed");

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
