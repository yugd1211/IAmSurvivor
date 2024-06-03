# IAmSurvivor

# MainScene

## 캐릭터 선택

캐릭터 선택은 OnClick 이벤트로 진행되며, OnClick 이벤트를 호출하는 객체의 ScriptableObject를 GameManager에게 넘겨주게 되며, GameManager는 해당 ScriptableObject를 기반으로 Player를 초기화한다.
![캐릭터선택창](./.image/캐릭터선택창.gif)

## 업적 열람

플레이어가 달성한 업적 열람이 가능합니다.
![업적열람](./.image/업적열람.gif)

## 통계 열람

플레이어가 플레이한 플레이 정보를 볼 수 있는 통계 패널의 열람이 가능합니다.
![통계열람](./.image/통계열람.gif)

## 업적&캐릭터 해금 알림창

업적과 업적에 따른 캐릭터가 해금될때 플레이어에게 이를 알린다.
알림창은 수 초후 사라지고, 여러 알림이 와도 중첩되며 아래에 줄지어 생긴다.
![알림](./.image/알림.gif)
![알림중첩](./.image/알림중첩.gif)
