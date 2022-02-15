﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameInfo;

public class AgoraController : MonoBehaviour
{
    private static AgoraController agoraController = null; // 싱글톤

    public CamObject camObject;
    static WebAgoraUnityVideo app;      // 비디오용

    private CanvasGroup camCanvasGroup;

    void Awake()
    {
        // 싱글톤
        if (agoraController == null)
        {
            agoraController = this;
            
            DontDestroyOnLoad(this.gameObject);
            if (ReferenceEquals(app, null) && !ReferenceEquals(camObject, null))
            {
                app = WebAgoraUnityVideo.GetTestHelloUnityVideoInstance();
                Debug.Log("캠 인스턴스 생성");
                app.loadEngine("539d6dace7d74d6f8c7c9a86e6c79f68");
                app.SetCamObject(camObject); // create app
            }
            camCanvasGroup = camObject.gameObject.GetComponent<CanvasGroup>();

            SceneManager.sceneLoaded += OnSceneLoadedAgora;
        }
        else
        {
            Debug.Log("AgoraController 삭제");
            Destroy(this.gameObject);
        }
    }

    public static AgoraController GetAgoraControllerInstance()
    {
        return agoraController;
    }

    // Start is called before the first frame update
    void Start()
    { 
        
    }

    void CamShow()
    {
        camCanvasGroup.alpha = 1;
        camCanvasGroup.interactable = true;
        camCanvasGroup.blocksRaycasts = true;
    }
    void CamHide()
    {
        camCanvasGroup.alpha = 0;
        camCanvasGroup.interactable = false;
        camCanvasGroup.blocksRaycasts = false;
    }

    private void OnSceneLoadedAgora(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name + "아고라로 접속합니다.");
        if(app != null)
        {
            if (GameRoomInfo.agoraVideoScene.Contains(scene.name))
            {
                Debug.Log(scene.name + "agora 비디오 연결");
                JoinAgoraRoom(GameRoomInfo.roomNo + scene.name, true, GameRoomInfo.userNoByRoom);
                CamShow();
            }
            else if (GameRoomInfo.agoraKeepScene.Contains(scene.name))
            {
                Debug.Log(scene.name + "agora 상태 유지");
                //빈블록: 그냥 아고라의 이전 상태를 유지하기 위함임
            }
            else
            {
                Debug.Log(scene.name + "agora 나가기");
                app.leave();
                CamHide();
            }
        }
        
    }

    public void SetVolumeByUserId(int userNo, float volume)
    {
        if(userNo != 0 && app != null)
            app.SetUserVolumeByUserId((uint)userNo, volume);
    }

    public void SetMyVolume(float volume)
    {
        if(app != null)
        {
            app.SetMyVolume(volume);
        }
    }

    public void JoinAgoraRoom(string joinRoomID, bool audioFlag, int userNo)
    {
        Debug.Log("아고라에 가입 요청 UID: " + userNo);
        if (userNo != 0 && app != null)
            app.join(joinRoomID, audioFlag, (uint)userNo);
    }

    //아고라 나가는 메서드
    public void LeaveAgoraRoom()
    {
        // 아고라 나가기 테스트
        if(app != null)
            app.leave(); // leave channel
    }

    public void LoadNewAgoraInstance()
    {
        app.leave();
        app.unloadEngine();
        app.loadEngine("b16baf20b1fc49e99bd375ad30d5e340");
    }

    
}
