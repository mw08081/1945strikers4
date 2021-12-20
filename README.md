## What I learned  
- Linguistic
  - Scene Load
  - ViewPort Position
  - Resources
  - BackGroundImage Offset Scrolling
  - PlayerPrefs
  - For Two People (feat. AddComponent<>())
- MatheMatical
  - Angle between A, B Vector  
  - Parabolic motion
  - Circle Moving
- Some Tips
  - Serializiable Class
  - FindObjectOfType<>() / FindObjectsOfType<>()
  - Renderer
  - Layer
  
### SceneLoad
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
    asyncOperation.allowSceneActivation = false;

    while (!asyncOperation.isDone)
    {
        if (asyncOperation.progress >= 0.9f)
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

+++ 자세한 SceneLoad Project link : 


### ViewPort Position
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


### Resources
### BackGroundImage Offset Scrolling
### PlayerPrefs
### For Two People
### Angle between A, B Vector  
### Parabolic motion
### Circle Moving
### Serializiable Class
### FindObjectOfType<>() / FindObjectsOfType<>()
### Renderer
### Layer

## What is lacking
- Linguistic
  - NetworkManager
- MatheMatical
  - Vector3Angle vs Quaternion
  - Vector3 Move
- On Editor
  - BGM & EffectSound
  - Terrain
  - ParticleSystem

### NetworkManager
### Vector3Angle vs Quaternion
### Vector3 Move
### BGM & EffectSound
### Terrain
### ParticleSystem


## About Project
> 플랫폼　　　　　PC(오프라인)  
> 장르　　　　　　슈팅  
> 이용등급　　　　전체  
> 출시년도　　　　PC | 2021년 12월 15일  
> ![1945Strikers4MainTitle](https://user-images.githubusercontent.com/58582985/145998095-5d40821b-87b7-4261-9ba2-0c83b5307b37.PNG)  

게임소개 - 모작 : 1945Strikers 시리즈에 이은 네번째 작품으로 정해진 패턴이 아닌 알고리즘에 의한 게임 플레이가 핵심이다. 총 2가지 스테이지와 3가지 스트라이커스로 구성되어있다

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


