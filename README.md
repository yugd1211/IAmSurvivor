# IAmSurvivor

<img width="700" alt="스크린샷 2024-07-24 15 48 30" src="https://github.com/user-attachments/assets/6bc7ca81-ba8b-43a9-b200-a924d9185477">

뱀서라이크류 2D 게임입니다.

엔진 : Unity  
버전 : 2021.3.35f1  

WebGL로 빌드후 unity play에 업로드하여 브라우저상에서 플레이 가능합니다. (PC O, Mobile X)    
https://play.unity.com/en/games/2c98ef4e-5b48-4c03-a13d-c4a8365ab91e/webgl-builds  
이동 : WASD or 방향키  
캐릭터, 아이템 선택 : 마우스 좌클릭  


## [주요 코드 설명](https://github.com/yugd1211/IAmSurvivor/wiki)

### 게임플레이


# MainScene

## 캐릭터 선택

캐릭터 선택은 OnClick 이벤트로 진행되며, OnClick 이벤트를 호출하는 객체의 ScriptableObject를 GameManager에게 넘겨주게 되며, GameManager는 해당 ScriptableObject를 기반으로 Player를 초기화합니다.

<details>
<summary><strong>gif</strong></summary>

![캐릭터선택창](./.image/캐릭터선택창.gif)

</details>

## 업적 열람

플레이어가 달성한 업적을 열람할 수 있습니다.

업적패널 UI는 스크롤이 가능하게 Scroll View를 사용했고 Grid Layout Group으로 정렬했습니다.

업적은 AchieveManager가 Dictionary<int, Achieve> 형태로 관리합니다.  
AchievementBook 객체는 AchieveManager의 UnlockAchieve 딕셔너리를 참조하여 업적을 가져옵니다.  
Achieve는 이름, 내용, 조건 등을 가지고 있으며, 이 중 이름과 내용을 패널에 보여줍니다.

<details>
<summary><strong>gif</strong></summary>

![업적열람](./.image/업적열람.gif)

</details>

## 통계 열람

플레이어가 플레이한 플레이 정보를 볼 수 있는 통계 패널의 열람이 가능합니다.

통계는 플레이어의 기록이 Json파일로 저장되는데 이를 불러와 패널에 보여줍니다.

<details>
<summary><strong>gif</strong></summary>

![통계열람](./.image/통계열람.gif)

</details>

## 업적&캐릭터 해금 알림창

업적과 업적에 따른 캐릭터가 해금될때 플레이어에게 이를 알립니다.  
알림창은 수 초후 사라지고, 여러 알림이 호출돼도 Virtical Layout group을 사용해 스택처럼 쌓이게 됩니다.

알림은 Notice 객체가 보여주게되는데 Notice는 추상 클래스로, 이를 상속받는 객체가 알림창의 내부를 수정하고 보여줍니다.

<details>
<summary><strong>gif</strong></summary>

![알림](./.image/알림.gif)
![알림중첩](./.image/알림중첩.gif)

</details>

## 아이템

레벨업시 아이템획득이 가능합니다.  
아이템은 ScriptableObject이며 Weapon, Armor, Potion으로 나뉘어져있고, 각각의 Data를 통해 설명 Text를 변경합니다.

아이템은 마우스 클릭과, 숫자키(1,2,3)으로 선택가능합니다.

<details>
<summary><strong>gif</strong></summary>

![아이템 선택](./.image/아이템선택.gif)

</details>
