# 3d Idle Game
3d 방치형 RPG입니다.

## 유니티 숙련 개인과제
<img width="1090" height="610" alt="Image" src="https://github.com/user-attachments/assets/7a0fb7a9-bc2c-494c-ab37-ad997f9d3b07" />

## 1. 프로젝트 소개
유니티에서의 데이터 관리와 플레이어를 구현하는 방법 중 하나인 FSM에 중점을 두고 학습하기 위한 프로젝트입니다.

---

### **개발 환경**
- **Engine**: Unity 2022.3.62f2
- **Language**: C# 
- **IDE**: Visual Studio


## 2. 프로젝트 구현 내용
 # 필수 과제

**기본 UI 구현** 

- 게임 화면에 HP, MP, 경험치 바, 현재 스테이지, 골드 및 재화 등의 정보를 표시합니다.

<aside>

**플레이어 AI 시스템** 

- 플레이어가 직접 조작하지 않아도 앞을 향해 나아가며, 적을 발견하면 일정 시간마다 자동으로 적을 공격합니다.
</aside>

<aside>

**아이템 및 업그레이드 시스템**

- 사용자가 아이템을 구매하고 업그레이드 할 수 있는 시스템입니다.
- 특정 아이템을 사용하여 플레이어의 스탯을 일시적으로 강화.
예시 : 체력 포션을 사용하면 HP가 즉시 회복되고, 공격력 스크롤을 사용하면 30초 동안 공격력이 10% 증가함.
</aside>

<aside>

**게임 내 통화 시스템**

- 게임 내에서 사용할 수 있는 가상의 통화 시스템입니다. 이 통화는 클릭이나 게임 내 활동을 통해 얻을 수 있습니다.
</aside>

<aside>

**아이템 및 장비 창 UI 구현**

- 화면의 버튼을 클릭하면 인벤토리 창이 열리고, 여기서 아이템을 장착하거나 사용할 수 있습니다.
</aside>

<aside>

**스테이지 시스템**

- 다양한 스테이지를 구성하고, 플레이어가 원하는 스테이지를 선택하여 입장하는 기능입니다.
</aside>

---
  
## 3. 사용 기술

### State Machine Pattern (FSM)
- State Pattern을 활용하여 각 상태(Move, Attack 등)를 독립적인 클래스로 분리</br>
- 상태별 Enter/Exit로 명확한 생명주기 관리 및 상태 전환 로직 구현</br>
- Physics 업데이트와 일반 업데이트를 분리하여 처리</br>
- 장애물 감지 기반 스마트 타겟팅으로 실전적인 AI 동작 구현
  <img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/c595cabc-a340-4153-95cb-1f694053ee1f" />
  <img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/0a3a7e5e-52dc-4e91-b8eb-4f8c82253bad" />


### ScriptableObject
- Player,Stage,Enemy,Item을 ScriptableObject로 구현하여 데이터와 로직 분리
- Inspector에서 직접 수정, 추가 가능한 유연한 아이템 시스템 구축

---

## 4. 인게임 스크린샷
<img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/9ee70e05-ffaf-4b6f-be63-07ac361a493a" />
<img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/7d9a1cd6-225a-4cca-8bcd-97ee54ad705b" />
<img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/8f2d89ad-a633-4466-be0b-daec94190f86" />
<img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/70079836-5dc3-4ea1-8eba-29d3b33564ac" />
<img width="1000" height="600" alt="Image" src="https://github.com/user-attachments/assets/247183ff-3123-40ff-8e03-c384a9f57cae" />
