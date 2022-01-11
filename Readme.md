## 오늘 진행 내용


### 01.10
### 1. 사용 프로그램 설치 및 환경 설정
### 2. 프로젝트 기획
- 프로젝트 미팅
    - kotlin server
    - 음성 서버 채팅
        - 필요하다면 채팅을 버리겠다
    - 환경을 만들 때 힘들다..
        - 문화유산 asset(무료 사용 가능)

    - 프로젝트 사이즈를 많이 줄여라
        - 유니티 -> 예찬님 -> 유니티 팀을 데려가라
        - 유니티를 웹에 어떻게 띄우는지 -> 멀티가 되는 환경을 어떻게 구현할 것인지 생각해야 한다.
            - >> 멀티로 소통을 하는 방식이 아닌 증거를 찾는 시간은 개인, 서로 조사 상황을 토론 하는 시간에만 멀티로 구현 하면 되지 않을까
    - 서버를 어떻게 해야할지 -> 무료 서버를 찾아서 세팅해라
    - 음성 / 대화 여러 유저 활동 프로토타입을 만들어서 올려라
        - 유저가 가입 접속 들어가서 방에 6명이든 2명이든 들어가서 활동 상호활동이 가능한지 확인
    - 스토리 2개 작은 거(우리 거) + 큰거 (차용)
        - 시간이 부족할 경우를 대비해 스토리를 작은 축소판으로 구현할 생각을 해야한다.
    - 상호작용 눌렀을 때 쪽지 형태로 나온다. (양피지st 이렇게 줄수 있는 거 / 이런 식으로 크기를 줄일 수 있다.)
    - 인벤토리 -> 이력 관리 (급하진 않음)
    - 게임이 돌아갈 수 있는 환경 갖추기
    - UNITY/SERVER/스토리 팀이 필요 (그만큼 최대한 빠른 진행을 해야한다.)

### 01.11
### 1. git flow 공부
- 깃이 동작하는 과정을 알아보았다.
### 2. 프로젝트 화면 구성을 위한 Asset 조사 및 구매
- 프론트의 화면을 담당할 Unity에서 사용할 Asset을 조사
    - 프로젝트에 알맞은 asset을 구매
### 3. DB 설계
- DB를 구성하기 위해 속성을 정리
        - 로그인 / 회원가입 (user)
            - 아이디 - 필수
            - 비밀번호 - 필수
            - 이름
            - 연락처 (전화번호) - 있어도 되고 없어도 됨
            - 주소 - 있어도 되고 없어도 됨
            - 뺀거(없어도되는거)
        - 역할 (각 시나리오 별 있을 역할)
            - 역할이 포함된 스토리 번호 (fk - 스토리)
            - 역할 id (pk)
            - 역할 설명 >> 이 안에 해당 유저의 스토리를 다 담는다..
            - 역할 이미지 (캠 화면에 띄워줄 이미지)
        - 스토리
            - 스토리 번호 (pk / 어떤 스토리인지 확인하는 용도)
            - 스토리 이름 (ㅇㅇㅇ 살인사건 같은 이름)
        - 맵
            - 맵 번호 (pk) → 이거에 따라 보여주는 맵이 바뀜
            - 스토리 번호 (fk)
        - 각 방 (room) >> 스토리번호 + 방번호
            - 스토리 번호 (fk)
            - 방 no or id → 유저가 있는 방과 증거가 있는 곳의 위치를 구분하기 위함
            - 방 이름
        - 증거
            - 증거 번호 or id (pk) -> 구분하기 위한 것
            - 증거가 있는 방 번호 or id (외래키) >> 어디 있는지 알기 위한 번호 (스토리번호 + 방번호)
        - 인게임 (session 번호) - 이건 휘발성 데이터 저장공간 / 게임이 종료되면 날려 줘야 함
            - Session 번호 (이 번호를 통해서 게임 종료시 clear시키기 / auto increase를 사용해도 괜찮을 것 같음)
            - 플레이중인 스토리 아이디
            - 플레이중인 유저 아이디
            - 역할 id - 유저가 받을 역할에 대한 id → 이걸로 플레이어가 원할 때 설명 보여줘야함
            - 해당 사람이 찾은 증거물 (db로 구성해야한다면 증거물의 개수를 딱 정해주는게 나을 것 같다는 생각이 듦 /   ex) 최대 증거 보유 가능 7개 >> 추가 증거를 보유하기 위해선 이전의 증거물을 제거하는 식 / 이렇게 하면 스토리가 작아져도 게임을 조금 더 어렵게 만들어 주는 수단이 될 수도 있다고 생각함)
            - 1차 투표 개수 (중간 투표 결과)
            - 2차 투표 개수 (최종 투표 결과)
            - 플레이 타임
            - 게임 진행 상태 (1차 조사 / 중간 투표 / 2차 조사 / 최종 조사 등)
        - 대기실
            - 대기실 번호 (== session 번호)
            - 대기실 명
            - 대기실 비밀번호 (default = null / 지정 시 고정)
            - 방장 id
            - 대기실에 있는 유저 id (1 ~ 6)
            - 게임의 상태 (진행 여부 - 대기모드 / 진행중)
            - 스토리 id (방장이 선택한 스토리)
### 4. 유니티를 통한 WebRTC 구현 해보기
- WebRTC의 구현에는 실패
