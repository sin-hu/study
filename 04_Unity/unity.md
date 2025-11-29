# 유니티
- 프로젝트 1개 = 게임 1개
- templete에서 2D, 3D 선택 가능
- 위쪽 |> 세모 버튼으로 게임 실행, 중지
- 오브젝트 위치 잃어버렸을 때 --> Hierarchy에서 오브젝트 더블클릭
- 코드(스크립트)는 vs code에서 작성
: edit - preferences - external tools - external script editor - vs code

**Hierarchy**
- 오브젝트
- MySquare
-- Circle #MySquare의 구성요소 ex) 로봇의 팔
- delete: 오브젝트 제거
- duplicate: 오브젝트 복사

**view**
- scene view
-- 게임 만드는 과정 볼 수 있음
- game view
-- 인게임 화면
-- 배경색은 Main Camera 색 -->insepector - background에서 변경
-- free aspect에서 화면 비율 설정 가능
-->모바일 게임에서 가로모드, 세로모드 설정 중요

**tools**
- view tool: 화면 전체 이동
- move tool:  x, y, 자유 방향 이동
- rotate tool: x,y,z 방향 회전
- scale tool: x,,y,z 크기 조정
- rect tool: 자유 크기 조절
- transform tool: 모든 기능 합쳐짐
--> 리셋: inspector - transform - 점 3개 - reset
--> 컨트롤 제트도 가능

**inspector**
- (컴퍼넌트)속성, 정보 표시, 값 변경
- transform: 모든 오브젝트 기본 속성
- sprite renderer: 사용자 생성 오브젝트만 가지는 속성
- add component로 컴퍼넌트 추가
- remove component로 컴퍼넌트 제거
ex) rigidbody 2D: 오브젝트에 물리법칙 적용

**project**
- 게임이 필요한, 가지고 있는 파일들
- assets - create - folder : 새 폴더 생성
-- 파일 탐색기에서 파일 붙여넣기 가능
- scripts(생성) 파일 - create - MonoBehaviour Script
: 코드 일부 작성되어 있음
또는
scripts(생성) 파일 - create - scripting - empty c# script
: 아예 빈 코드
-->스크립트 더블클릭시 vscode로 넘어가서 수정 가능
--> vs code에서 수정, 저장하면 유니티에 반영 됨

**console**
- 스크립트 출력문, 로그, 에러 확인창
