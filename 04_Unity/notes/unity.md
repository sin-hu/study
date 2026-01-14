# 유니티
- 프로젝트 1개 = 게임 1개
- templete에서 2D, 3D 선택 가능
- 위쪽 |> 세모 버튼으로 게임 실행, 중지
- 오브젝트 위치 잃어버렸을 때 --> Hierarchy에서 오브젝트 더블클릭
- 코드(스크립트)는 vs code에서 작성  
: edit - preferences - external tools - external script editor - vs code

# 기능
## Hierarchy
- 오브젝트  
예시)  
MySquare  
-Circle  :MySquare의 구성요소 ex) mysquare의 팔    
  
- delete: 오브젝트 제거
- duplicate: 오브젝트 복사


## view
- scene view  
-- 게임 만드는 과정 볼 수 있음  
- game view  
-- 인게임 화면  
-- 배경색은 Main Camera 색 -->insepector - background에서 변경  
-- free aspect에서 화면 비율 설정 가능  
--> 모바일 게임에서 가로모드, 세로모드 설정 중요


## tools
- view tool: 화면 전체 이동
- move tool:  x, y, 자유 방향 이동
- rotate tool: x,y,z 방향 회전
- scale tool: x,,y,z 크기 조정
- rect tool: 자유 크기 조절
- transform tool: 모든 기능 합쳐짐  
--> 리셋: inspector - transform - 점 3개 - reset  
--> 컨트롤 제트도 가능


## inspector
:컴퍼넌트(오브젝트의 속성=옵션), 정보 표시, 값 변경
- transform: 모든 오브젝트 기본 속성
- sprite renderer: 사용자가 생성한 오브젝트 입맛대로 바꿀 수 있는 속성
- add component로 컴퍼넌트 추가
- remove component로 컴퍼넌트 제거  
ex) rigidbody 2D: 오브젝트에 물리법칙 적용


## project
:게임이 가지고 있는 파일들
### assets
:주로 scenes, scripts, sprites 폴더 생성  
- assets - create - folder : 새 폴더 생성  
-- 파일 탐색기에서 파일 붙여넣기 가능
- scripts(생성) 파일 - create - MonoBehaviour Script  
: 코드 일부 작성되어 있음  
또는  
scripts(생성) 파일 - create - scripting - empty c# script  
: 아예 빈 코드  
  
-->스크립트 더블클릭시 vscode로 넘어가서 수정 가능  
- vs code에서 수정, 저장하면 유니티에 반영 됨  
- vs code 옆쪽에 프로젝트 구조 보임

### console
- 스크립트 출력문, 로그, 에러 확인창


# 유니티 실습
## 이미지 구하기
- 무료 이미지 제공 사이트  
character sprite site:opengameart.org 검색  
% site:opengameart.org --> %부분에 찾는 이미지 적으면 opengameart.org 사이트에서 검색 됨  

배경 만들기  
:sprites파일에 넣어둔 배경 이미지 hierarchy나 scene뷰에 끌어와 쓰기

크기조정
- shift누른채로 조절시, 비율 고정
- shift + alt 시 중앙 고정
-inspector의 pixels per unit 조정
(여기서 unit은 scene뷰의 작은 네모 하나)  
** inspector의 position 값도 unit 단위 (x값: 10 = unit 10칸)  
** 위치는 중앙 기준 (x값:10 = 중앙으로부터 10 떨어진 객체의 중앙)

움직이는 배경 만들기
- 같은 배경 붙이기 -> 같이 이동 -> 한개 위로 올리기 -> 반복 (컨베이너 벨트)
- b1, b2에 같은 코드 넣기 --> 한쪽 코드라도 수정시 모든 코드 바뀜


(레이어 관련)
- order in layer: 레이어 순서 결정값(inspector에 있음)
- sorting layer: 그룹 묶기  

**스프라이트 != 오브젝트
-->오브젝트에 스프라이트 설정하는 것