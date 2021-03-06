
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.InteropServices;
using agora_gaming_rtc;

namespace Project
{
    public class NewWork_Meeting : MonoBehaviour
    {

        // 자기 자신을 instance로 선언
	    public static NewWork_Meeting instance;
        //  ':' 로 분리할 것
	    static private readonly char[] Delimiter = new char[] {':'};
            
        Text RoleDesc;
        Text StoryName;
        Text StoryDesc;

        [Header("오브젝트 :")]
        public GameObject btn_exitMeeting;
        public GameObject btn_closeVideo;

        public Text phase3desc;
        public Text phase5desc;

        public GameObject myVideo;
        public VideoPlayer vp;
        private AudioSource musicPlayer;

        // Start is called before the first frame update
        void Start()
        {
            // 싱글 톤 코드
            if (instance == null) {
                // 
                vp.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Video_Clue.mp4");
                myVideo.gameObject.SetActive(false);
                btn_closeVideo.gameObject.SetActive(false);
                // OnPlayVideo();
                if (Client.meeting_num == 0)
                {
                    // 미팅 룸으로 들어오기
                    phase3desc.gameObject.SetActive(true);
                    phase5desc.gameObject.SetActive(false);
                    Client.meeting_num += 1;
                } else if (Client.meeting_num == 1)
                {
                    phase3desc.gameObject.SetActive(false);
                    phase5desc.gameObject.SetActive(true);
                }




            }
            else {
			    //it destroys the class if already other class exists
			    Destroy(this.gameObject);
		    }
        
        }

        void OnEnable(){
            // Debug.Log("OnEnable");
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void emitExitMeeting(){
            Debug.Log("[system] 미팅에서 나갑니다. ");
            btn_exitMeeting.SetActive(false);
            if (GameInfo.GameRoomInfo.roomReadyPlayer == 5 && GameInfo.GameRoomInfo.roomReadyPlayer == 6)
            {
                Debug.Log("[system] 플레이어가 모두 나가고 싶어 합니다. : " + Client.room);
                Application.ExternalCall("socket.emit", "EXIT_MEETING", Client.room);
            }
            else
            {
                Client.ready = true;
                Application.ExternalCall("socket.emit", "READY_CRIMESCENE", Client.room);
            }

            // 키 밸류 데이터 
      //      Dictionary<string, string> data = new Dictionary<string, string>();
		    //// 보낼 정보 저장
		    //data["callback_name"] = "EXIT_MEETING";
      //      data["name"] = Client.name;

      //      // JSON으로 묶어서 보냄 
		    //Application.ExternalCall("socket.emit", data["callback_name"], new JSONObject(data));
        }
        public void onRefreshReadyPlayer(int readyPlayer)
        {
            Debug.Log("[system] 준비된 플레이어 : " + readyPlayer);
            GameInfo.GameRoomInfo.roomReadyPlayer = readyPlayer;
            if ( GameInfo.GameRoomInfo.roomReadyPlayer >= 6 )
            {
                Application.ExternalCall("socket.emit", "EXIT_MEETING", Client.room);
            }

        }


        public void onExitMeeting(){
            Debug.Log("[system] 게임으로 갑니다. ");

            Client.ready = false;
            GameInfo.GameRoomInfo.roomReadyPlayer = 0;
            SceneManager.LoadScene("Map", LoadSceneMode.Single);
        }

        public void OnPlayVideo() {
            Debug.Log("[system] 비디오를 재생합니다." );
            musicPlayer = GetComponent<AudioSource>();
            musicPlayer.Play();
            myVideo.gameObject.SetActive(true);
            btn_closeVideo.gameObject.SetActive(true);
            vp.Play();
        }

        public void CloseVideo(){
            Debug.Log("[system] 비디오를 닫습니다." );
            vp.Stop();
            myVideo.gameObject.SetActive(false);
            btn_closeVideo.gameObject.SetActive(false);
        }

    }
}

