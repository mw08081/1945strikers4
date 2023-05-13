## 개요
1. [InGame](https://github.com/mw08081/1945strikers4/blob/main/README.md#scene---main_title--choose_player)
2. [What I learned](https://github.com/mw08081/1945strikers4/blob/main/README.md#what-i-learned)
3. [What i lacking](https://github.com/mw08081/1945strikers4/blob/main/README.md#what-is-lacking)

### SCENE - MAIN_TITLE & CHOOSE_PLAYER  
![PlayerChoice](https://user-images.githubusercontent.com/58582985/146001339-09db4837-d7b8-4bef-9b3c-674d56f4add1.gif) 
### SCENE - INGAME(PLAYER)
![INGAMEF5U](https://user-images.githubusercontent.com/58582985/146014710-b768be8b-2d64-45c2-94e5-06e45a3077e0.gif)
![BFINGAME](https://user-images.githubusercontent.com/58582985/146015963-713f31a4-924e-481f-b827-e1ec514e6cf1.gif)
### SCENE - INGAME(BOSE_INFO)
![ST1BOS](https://user-images.githubusercontent.com/58582985/146017173-20ce57b6-f049-46b0-bd41-43de21532149.gif)  
![M1](https://user-images.githubusercontent.com/58582985/146018235-bba7aedf-0787-451d-8df2-ed2c65206e3f.gif)
![M23](https://user-images.githubusercontent.com/58582985/146018269-641d2726-1d8d-4953-a637-cab19a966b63.gif)
![M4](https://user-images.githubusercontent.com/58582985/146018826-7b6ab667-d9ee-44fb-bda1-ba971fc4b3e4.gif)
### SCENE - GAMEOVER
![ENDING](https://user-images.githubusercontent.com/58582985/146020109-766c90f4-5dbb-40e7-a6d6-abae45515308.gif)
![ENDING2](https://user-images.githubusercontent.com/58582985/146020115-a512c57a-0ddd-47dc-9b24-051ac527471d.gif)
　  
　  
## What I learned  
- Linguistic
  - [Scene Load](https://github.com/mw08081/1945strikers4/blob/main/README.md#1-sceneload)
  - [ViewPort Position](https://github.com/mw08081/1945strikers4/blob/main/README.md#2-viewport-position)
  - [Resources](https://github.com/mw08081/1945strikers4/blob/main/README.md#3-resources)
  - [BackGroundImage Offset Scrolling](https://github.com/mw08081/1945strikers4/blob/main/README.md#4-backgroundimage-offset-scrolling)
  - [PlayerPrefs](https://github.com/mw08081/1945strikers4/blob/main/README.md#5-playerprefs)
  - [For Two People (feat. AddComponent<>())](https://github.com/mw08081/1945strikers4/blob/main/README.md#6-for-two-people)
- MatheMatical
  - [Angle between A, B Vector](https://github.com/mw08081/1945strikers4/blob/main/README.md#7-angle-between-a-b-vector)
  - [Parabolic motion](https://github.com/mw08081/1945strikers4/blob/main/README.md#8-parabolic-motion)
- Some Tips
  - [Resources Prefab Cache](https://github.com/mw08081/1945strikers4/blob/main/README.md#9-resources-object-cache)
  - [Serializiable Class](https://github.com/mw08081/1945strikers4/blob/main/README.md#10-serializiable-class)
  - [Time Pause](https://github.com/mw08081/1945strikers4/blob/main/Assets/Resources/cs/UI/OptionPanel.cs)
  
### 1. SceneLoad
씬을 로드하는 방식으로 비동기로드와 동기로드, 두가지 방식을 이용하였다  
  
씬의 동기로드의 경우 하나의 작업을 요청한 후에 작업이 완료되어야만 다음 작업을 시작하는 방식이다 
그러니 동기로드는 작업이 길어질 경우, 사용자로부터 불편함을 야기한다 그래서 비동기 로드를 사용하는 것이다  
  
다음 씬의 리소스가 무거울경우 씬을 비동기적으로 호출하여 다음 씬 호출 도중에도 다른 작업을 할 수 있게 된다  
예를 들면 진행률을 보여주거나 게임내 미니게임을 하는 등의 작업을 할 수 있다  
  
각 씬에서 곧 바로 다음 씬을 비동기적 호출하여도 좋으나 해당 프로젝트에서는 LoadScene을 만들어서  
LoadScene을 동기로드하고, 다음 씬을 LoadScene에서 비동기적 호출하는 방식을 사용했다  
  
각각의 로드에 사용되는 함수를 살펴보자
```C#
using UnityEngine.SceneManagement;
...
//동기로드
SceneManager.LoadScene(string sceneName);

//비동기로드
...
      StartCoroutine("AsyncLoadSceneCoroutine");
...

IEnumerator AsyncLoadSceneCoroutine()
{
      AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode);

      while(!asyncOperation.isDone)
      {
            yield return null;
      }
      Debug.Log("Scene Load Coplete!! : " + NextSceneName);
}
```
그렇다면 비동기로드에 사용되는 AsyncOperatioin의 변수에 대해 살펴보자  
- allowSceneActivation : 씬 전환을 허용한다
- isDone : 씬 로드 완료 여부(읽기전용)
- priority : 우선순위를 설정
- progress : 씬 로드 진행상황을 0~1값으로 표현한다(읽기전용)  

한 가지 주의할 점은 allowSceneActivation은 단순히 씬 전환을 허용하는 것이 아닌, 완료여부도 같이 결정한다  
다시 말해, 씬로드가 완료되어도 allowSceneActivation이 false라면 isDone 또한 false인 셈이다  
따라서 아래와 같이 !asyncOperation.isDone의 반복 안에서 입력을 통해 allowSceneActivation을 허용해줘야한다  
```C#
IEnumerator AsyncLoadSceneCoroutine()
{
    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode);
    asyncOperation.allowSceneActivation = false;            //씬 전환 false

    while (!asyncOperation.isDone)
    {
        if (asyncOperation.progress >= 0.9f)                //로드 완료
        {
            Debug.Log("AsyncLoadScene is complete!  " + nextSceneName + "  Press Space to NextScene");
            if (Input.GetKeyDown(KeyCode.Space))
                asyncOperation.allowSceneActivation = true;
        }
        yield return null;
    }
}
```
이때 progress가 0.9이상일 때를 씬 로드가 끝난 시점으로 하는데 그것에 대해서는 정확하게 잘 모르겠다  
또 progress는 로딩씬 임의의 Slider의 value값으로 설정해준다면 진행상황을 sliderBar로 확인해볼 수 있다 
  
+++ 자세한 SceneLoad Project link  
(링크)
　   
　   
### 2. ViewPort Position
ViewPort Position이란 유니티상의 3d world가 2d 출력화면에 출력될 때, 해당 화면에서의 좌표를 의미한다  
좌하단을 (0, 0), 우상단을 (1.0, 1.0)으로 인식하는 것이다  
  
사용하는 방법은 다음과 같다
```C#
Vector3 worldPosition;
Vector3 viewportPosition;

worldPosition = transform.position;
viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);

viewportPosition = Camera.main.ViewportToWorldPoint(viewportPosition);
```
이때 viewport Position은 z값이 카메라와의 거리를 의미하므로 2d좌표가 3d좌표로 변환이 가능한 것이며 애초에 viewportPosition도 3d좌표인 점을 기억하자  
  
이번 프로젝트에서는 두가지 부분에서 viewPort Position을 활용할 수 있었다
- Player의 이동가능 범위 제한 - void LimitPlayerPosition()  
https://github.com/mw08081/1945strikers4/blob/main/Assets/Resources/cs/Actor/Player/Player.cs
- Enemy의 status 전환 - protected override void Updating()  
https://github.com/mw08081/1945strikers4/blob/main/Assets/Resources/cs/Actor/Enemy/GroundEnemy/GroundEnemy.cs
　  
　  
### 3. Resources
Resources는 미리 오브젝트(reference)를 에디터상에서 할당해두지 않더라도 유동적으로 원하는 오브젝트를 할당할 수 있다  
이는 고정적인 오브젝트 할당이 아닌 사용자의 선택에 따른 오브젝트 할당과 같이 유동적인 오브젝트 할당을 수행한다  
  
활용하는 방법은 다음과 같다
```C#
GameObject go = Resources.Load<T>(string path);
```
단 Asset의 경로는 Assets/Resources라는 폴더를 만든 후 Resources 디렉토리의 경로까지 생략한 다음부터 사용해주면 된다  
`ej) Prefab/Bullet/...`
　  
　  
### 4. BackGroundImage Offset Scrolling
동일한 배경 세개 배치하여 배경을 스크롤하는 방법도 있다  
그러나 배경이 단순한 이미지일 경우 이미지의 offset을 활용하여 한 개의 배경만으로도 스크롤할 수 있다  
  
방법은 스크롤 하고자 하는 이미지를 메테리얼로 준비하여 Plane/Quad의 MeshRenderer로 적용한 후 MeshRenderer의 imgOffset을 이용하는 것이다  
```C#
[SerializeField] MeshRenderer bg;
[SerializeField] float speed;
float offsetY;

void Update()
{
    BackGroundScrolling();
}

void BackGroundScrolling()
{
    offsetY += (float)speed * Time.deltaTime;             //이미지가 가로인지 세로인지에 따라, offsetX / offsetY를 활용
    offsetY = offsetY % 1.0f;

    Vector2 offset = new Vector2(0, offsetY);             //offsetX라면 Vector2의 x값을, offsetY라면 Vector2의 y값을 변경

    bg.material.SetTextureOffset("_MainTex", offset);
}
```
단 주의할 점은 이미지의 Texture Type이 Sprite(2D and UI)와 같은 것이 아닌 Default로 설정해둬야 한다  
  
+++ bgImg Scrolling  
https://github.com/mw08081/1945strikers4/blob/main/Assets/Resources/cs/BG/BackGroundImgScrolling.cs
　  
　  
### 5. PlayerPrefs
PlayerPrefs는 기본적인 데이터 저장에 용이하다  
다만 보안성이 굉장이 낮으므로 옵션값과 같은 데이터를 저장하는 것이 좋다  
  
해당 프로젝트에서는 PlayerPrefs를 활용해보는 차원에서 플레이어의 점수를 저장했다  
활용방법은 다음과 같다  
```C#
PlayerPrefs.SetString(string key, string val);
PlayerPrefs.SetInt(string key, int val);
PlayerPrefs.SetFloat(string key, float val);

string strTmp = PlayerPrefs.GetString(string key);
int iTmp = PlayerPrefs.GetInt(string key);
float fTmp = PlayerPrefs.GetFloat(string key);
```
+++ PlayerPrefs 활용  
https://github.com/mw08081/1945strikers4/blob/main/Assets/Resources/cs/UI/BestStrikersPanel.cs  
　  
   
### 6. For Two People
한 키보드에서 두 명의 플레이어가 게임하는 방법에 대해 고민해봤다  
한 명은 방향키, 또 다른 한 명은 w,a,s,d로 게임하는 방법이다  
  
player1는 방향키를 입력했을 때, player의 이동벡터를 변경해주고
player2는 w,a,s,d를 입력했을 때, player2의 이동벡터를 변경해주는 방식이다
해당 방식은 `moveDir = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));`을 활용을 하지 못하고 직접 이동벡터를 만들어서 활용해야한다  
  
+++ Input.GetAxix()를 활용하지 않고 이동벡터를 만드는 방법  
https://github.com/mw08081/1945strikers4/blob/main/Assets/Resources/cs/Actor/Player/Player1Controller.cs  

다만 player Object가 정해져 있지않고 나중에 Instantiate된다면 아래와 같이 AddComponent<>()를 통해 controller.cs를 붙여줘야한다  
```C#
GameObject playerGameObject = Instantiate(PlayerPrefab[SystemManager.Instance.Player1PrefabIndex]);
playerGameObject.AddComponent<Player1Controller>();
``` 
   
　  
### 7. Angle between A, B Vector  
두 벡터가 이루는 사이각을 구하는 방법은 벡터의 내적을 활용하는 것이다   
두 벡터의 성분값을 알면 사잇각을 계산할 수 있다  
  
두 벡터의 내적은 `a · b = a1*b1 + a2*b2 = |a||b| * cosΘ` 이므로  
a = (1, 0), b = (0.5, -0.5) 의 내적을 계산하면 0.5이다   
  
따라서 a, b벡터가 단위벡터이면, |a|, |b|는 1이고 cosΘ = 0.5, Θ = 𝝿 / 4(45°)  
  
이를 C#에서는 다음과 같이 표현할 수 있다.
```C#
Vector3 a = new Vector3(1, 0, 0);
Vector3 b = new Vector3(0.5f, 0, -0.5f);

float betweenAngle = Mathf.Acos(Vector3.Dot(a, b)) * Mathf.RadToDeg;
```
사잇각(betweenAngle)은 내적결과를 Acos함수를 통해 각도를 얻을 수 있다  
이때 반드시 Acos값에 Mathf.RadToDeg값을 곱해줘야지 라디안 각도를 얻을 수 있다  

다음은 Acos에 대한 C# Docs이다  
<img width="695" alt="스크린샷 2021-12-04 오후 4 17 49" src="https://user-images.githubusercontent.com/58582985/144701396-3b9d3f10-2c72-4916-a8b7-1e7c50ccda5c.png">  
　 
  
### 8. Parabolic motion
포물선운동의 수학적 계산을 활용하면 미리 원하는 비행거리를 위한 각도와 힘을 계산할 수 있다  
각도가 정해져 있는 경우라면 힘만을 조절하면되고, 힘이 정해진 경우라면 각도를 조절하면 될 듯하다  
  
프로젝트에서는 각도가 45도일때 힘을 계산했다  
`비행거리 = Sin(2 * θ) * V^2 / g`이므로 Sin(2 * θ)는 1이 되고, 따라서 `비행거리 = V^2 / g`인 셈이다  

+++ 포물선 운동의 최대높이와 최대거리 공식  
https://github.com/mw08081/MathNPhysics2D#1-%ED%9E%98%EA%B3%BC-%EC%9A%B4%EB%8F%99force--motion  
+++ 비행거리에 따른 힘 계산 - IEnumerator attackModel2()  
https://github.com/mw08081/1945strikers4/blob/main/Assets/Resources/cs/Actor/Enemy/GroundEnemy/GroundEnemy.cs
　  
　  
### 9. Resources Object Cache
앞써 배운 Resources를 동일한 Object에 대해서 지속적으로 호출한다면 생각보다 무거운 프로그램이 된다  
그래서 에디터에서 직접 할당한 Object의 경우에는 별도의 Cache가 필요하지 않을 듯하나 Resources Object에 대해서 Cache작업을 해두면 좋을 듯하다  
  
활용하는 방법은 다음과 같다
```C#
//using Dictionary<T, T>
using System.Collections.Generic;

Dictionary<string, GameObject> prefabPathCacheDic = new Dictionary<string, GameObject>();      //패스캐시 딕셔너리
List<Queue<GameObject>> objectPoolingList = new List<Queue<GameObject>>();                  //오브젝트풀링 리스트

void Start()
{
    ...
    SetObjectPooling();
    ...
}

//prefabPath Cache Function
GameObject PrefabLoad(string resourcePath)
{
    GameObject go = null;
    if(prefabPathCacheDic.ContainsKey(resourcePath))                //해당 패스로 이미 prefab이 캐시되어있을 경우
    {
        go = prefabPathCacheDic[resourcePath];
    }
    else                                                            //해당 패스로 이미 prefab이 캐시되어있지 않을 경우
    {
        go = Resources.Load<GameObject>(resourcePath);
        
        if(!go)
        {
            Debug.LogError("Load Error");
            return null;
        }
        prefabPathCacheDic.Add(resourcePath, go);
    }
    GameObejct instantiateGo = Instantiate(go, transform);
    
    return instantiateGo;
}

//ObjectPooling Function
void SetObjectPooling()
{
    GameObejct go = null;
    for(int i = 0; i < N; i++)                              //N = 생성할 objectPoolingList 개수
    {
        objectPoolingList.add(new Queue<GameObject>());
        for(int j = 0; j < 10; j++)                         //임의적으로 모든 오브젝트를 10씩 생성하여 Queue에 삽입
        {
              go = PrefabLoad(prefabPath);
              
              go.SetActive(false);                          //ObjectPooling 비활성화
              objectPoolingList[i].Enqueue(go);             //ObejctPooling Queue에 삽입
        }
    }
}
```
　  
### 10. Serializiable Class
유니티 상에 클래스를 통째로 표시하는 방법이다  
  
방법은 다음과 같다  
```C#
[System.Serializable]
class AttackModel
{
    public string className;
    public int fireCnt;
    public float fireInterval;
}

...
[SerializeField] AttackModel[] attackModels;
```
### 11. Time Pause(Option Menu)
보통 게임에서 모서리에 있는 옵션을 열 경우, 게임은 일시 정지하게 된다. 해당 기능을 구현하는 방법은 다음과 같다.
```c#
public override void ShowPanel()
{
    base.ShowPanel();
    Time.timeScale = 0f;
}

public override void ClosePanel()
{
    base.ClosePanel();
    Time.timeScale = 1f;
}
```
option을 열었을 때, ShowPanel()함수를 통해 option창이 열린다.  
이때, 동시에 `Time.timeScale`을 0f로 설정해주면 된다.  
해제할때에는 1f로 설정해주면 되는 듯하다  

+++ 참고) https://github.com/mw08081/1945strikers4/blob/main/Assets/Resources/cs/UI/OptionPanel.cs  

## What is lacking
- Linguistic
  - NetworkManager
  - Resolution
- MatheMatical
  - Vector3Angle vs Quaternion
  - Vector3 Move(to be added)
- On Editor
  - BGM & EffectSound(to be added)
  - Terrain
  - ParticleSystem

### NetworkManager
### Resolution
### Vector3Angle vs Quaternion
### Vector3 Move
### BGM & EffectSound
### Terrain
### ParticleSystem

### Panel System
