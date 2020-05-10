# MapEditor-Project-I

## 사용법
이미지에 있는거처럼 기능별로 Tilemap을 나눠줍니다.<br>
Ex) GroundTilemap, BackgroundTilemap, EnemyTilemap<br>

타일맵을 선택한뒤 Brush로 그려줘야 그 타일맵에 저장되니 주의해주세요.<br>

시작 버튼을 누르면 이러한 화면이 나오는데<br>
타일맵을 다 그려주고 난뒤 FileName은 Json으로 저장될 파일 이름 MapIndex는 방번호를 적어주세요<br>
EX) Map_1, 1, Map_1-2, 1-2<br>

플레이어가 방에 입장한뒤 생성되는 위치는 오른쪽위에 PlayerStartPositionSettingMdde를 클릭한뒤,<br>
생선된 깃발을 원하는 위치에 꽂아주시면됩니다<br>

플레이어가 방에 되돌아왔을때의 위치 역시 동일하게 PlayerEndPositionSettingMode를 클릭한뒤 진행해주세요.<br>

그다음에 CreateJson버튼을 누르시면 됩니다.<br>

LoadTilemap은 맵로드가 잘되나 테스트하기위한 버튼입니다.<br>

## Prefab Brush
함정이나 몬스터를 타일맵에 배치하기 위해서 Prefab Brush가 필요합니다.<br>

사진처럼 Prefab Brsuh를 만들어주시고<br>

프리팹을 넣어줍니다.<br>

이렇게 Brush를 바꿔주시고 Tilemap을 선택해 그려주시기만 하면됩니다.<br>
