# 로프액션 기술 구현 프로젝트 

## 개요 

- 특정 로프액션 게임을 보고 만들어보고 싶다는 생각에 만들어보게 되었습니다.
- 로프액션에 필요한 기능을 중점으로 구현하였습니다.

## 개발 스택 

- Unity 2022.3.26f1
- Visual Studio 2022

## 개발 기간

- 2024.05.17 ~ 2024.06.04

## 구현 기능 

### 로프 이동 

<img src="https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Image/로프이동및반동.gif" width="50%" height="50%" />
<img src="https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Image/로프가속.gif" width="50%" height="50%" />

- 지형 오브젝트에 마우스로 로프를 발사한 후 방향키를 사용하여 반동을 줄 수 있도록 구현했습니다.
- 방향키를 누르고 Shift를 누르면 Addforce()를 사용하여 순간 가속을 하여 좀더 멀리 날아가도록 구현했습니다.
- 순간 가속 이후 등장하게 되는 잔상 효과는 파티클로 구현하였습니다. [잔상 효과 Code](https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Assets/Script/AfterImage.cs)
- 로프를 걸고 윗방향키와 Shift를 누르면 위로 순간적으로 이동할 수 있도록 구현하였습니다.
- 로프를 걸었을 때 아래 방향키로 로프의 거리를 조절 할 수 있도록 구현하였습니다. 

<img src="https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Image/이동오브젝트로프.gif" width="50%" height="50%" />

- 이동하는 오브젝트에 로프를 걸었을 시 같이 이동할 수 있도록 구현하였습니다.

문제점 해결 
- 로프의 길이가 늘어나는 문제와 움직이는 오브젝트를 따라가지않는 문제점이 발생하였습니다. 그래서 이를 해결하고자
- 로프의 길이는 Spring joint 2D를 사용하여 해결하였습니다. 매달렸을 때 더이상 길이가 증가되지 않도록 Spring joint 2D 사용하여 자연스러움을 주면서 길이가 증가되지 않도록 하였습니다.
- 두번째도 오브젝트를 따라가지 않는 문제는 Hook의 부모를 오브젝트로 설정하면서 Hook이 오브젝트를 따라가도록 하여 해결하였습니다.

- [훅 Code](https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Assets/Script/Hooking.cs)

### 로프 공격 

- [플레이어 Code](https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Assets/Script/Player.cs)

<img src="https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Image/로프공격.gif" width="50%" height="50%" />

- 로프가 발사되는 방향에 적이 있다면 Line Renderer의 색을 빨간색으로 바꿔 적이라는 것을 알려주고
- 적에게 로프가 발사될 경우 순간적으로 이동하도록 구현하였습니다.
- 또한 이동하였을 때 일정 시간 동안 적의 이동을 플레이어가 직접 제어할 수 있도록 구현하였습니다. 
- 이후 마우스 우클릭 또는 일정 시간뒤에 마우스 반대 방향으로 플레이어가 날아가게 되며 적은 사라지도록 구현하였습니다. 

### 벽타기 

<img src="https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Image/벽타기.gif" width="50%" height="50%" />

- 플레이어가 바라보는 방향으로 하여 콜라이더를 생성하여 해당 콜라이더에 벽이 충돌하면 중력값을 조절하여 벽을 올라갈 수 있도록 구현하였습니다.
- 벽을 탈 때 반대방향을 보거나 점프를 할 시 중력값을 원래상태로 바꾸며 벽타기가 안되도록 구현하였습니다. 

### 로프 구현 

<img src="https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Image/조인트.png" width="30%" height="30%" />

- 플레이어가 로프를 발사할 때의 로프의 inspactor 화면입니다.
- Distance Joint와 Spring Joint를 사용였습니다.
- Distance Joint는 벽과 훅을 이어주기위해 사용하였고
- Spring Joint는 플레이어가 훅을 걸었을 때도 이동할 수 있도록 즉, 로프의 길이가 조절 될 수 있도록 구현하기 위해 두개의 joint를 사용하였습니다.
- [훅 Code](https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Assets/Script/Hooking.cs)

### 카메라 

<img src="https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Image/시네머신.png" width="30%" height="30%" />

- 시네머신 카메라의 확장기능으로 Cinemachine Confiner 를 사용하여 카메라의 범위를 구현하였습니다.
- Cinemachine Confiner 같은 경우 Ploygon Collider를 사용하여 외곽 테두리를 그려주게 되면 카메라의 반경이 해당 테두리를 벗어나지 못하도록 해주는 효과가 있습니다.
- 따라서 Ploygon Collider를 사용하여 외곽 테두리를 그려주고 카메라가 해당 범위를 벗어나지 못하도록 하기 위해 Cinemachine Confiner 컴포넌트를 사용하였습니다. 

