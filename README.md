# 로프액션 기술 구현 프로젝트 

## 개요 

- 특정 로프액션 게임을 보고 만들어보고 싶다는 생각에 만들어보게 되었습니다.
- 게임을 처음부터 구현하지 않고 로프액션에 필요한 기능만 구현하였습니다.

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
- 로프를 걸고 윗방향키와 Shift를 누르면 위로 순간적으로 이동할 수 있도록 구현하였습니다.
- 로프를 걸었을 때 아래 방향키로 로프의 거리를 조절 할 수 있도록 구현하였습니다. 

<img src="https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Image/이동오브젝트로프.gif" width="50%" height="50%" />

- 이동하는 오브젝트에 로프를 걸었을 시 같이 이동할 수있도록 하였습니다. 

### 로프 공격 

<img src="https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Image/로프공격.gif" width="50%" height="50%" />

### 벽타기 

<img src="https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Image/벽타기.gif" width="50%" height="50%" />

### 로프 구현 

<img src="https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Image/조인트.png" width="30%" height="30%" />

### 카메라 

<img src="https://github.com/parkjun-0521/SANABI_Function_Practice-/blob/master/Image/시네머신.png" width="30%" height="30%" />
