# IAmSurvivor

# MainScene

## 캐릭터 선택

<details>
<summary><strong>내용</strong></summary>
캐릭터 선택은 OnClick 이벤트로 진행되며, OnClick 이벤트를 호출하는 객체의 ScriptableObject를 GameManager에게 넘겨주게 되며, GameManager는 해당 ScriptableObject를 기반으로 Player를 초기화합니다.

![캐릭터선택창](./.image/캐릭터선택창.gif)

</div>
</details>

## 업적 열람

<details>
<summary><strong>내용</strong></summary>
플레이어가 달성한 업적 열람이 가능합니다.

![업적열람](./.image/업적열람.gif)

</div>
</details>

## 통계 열람

<details>
<summary><strong>내용</strong></summary>
플레이어가 플레이한 플레이 정보를 볼 수 있는 통계 패널의 열람이 가능합니다.

![통계열람](./.image/통계열람.gif)

</div>
</details>

## 업적&캐릭터 해금 알림창

<details>
<summary><strong>내용</strong></summary>
업적과 업적에 따른 캐릭터가 해금될때 플레이어에게 이를 알립니다.
알림창은 수 초후 사라지고, 여러 알림이 와도 중첩되며 아래에 줄지어 생깁니다.

![알림](./.image/알림.gif)
![알림중첩](./.image/알림중첩.gif)

</div>
</details>

## 아이템

<details>
<summary><strong>내용</strong></summary>
레벨업시 아이템획득이 가능합니다.  
아이템은 ScriptableObject이며 Weapon, Armor, Potion으로 나뉘어져있고, 각각의 Data를 통해 설명 Text를 변경합니다.

아이템은 마우스 클릭과, 숫자키(1,2,3)으로 선택가능합니다.

![아이템 선택](./.image/아이템선택.gif)

</div>
</details>
