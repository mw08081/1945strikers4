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
SceneManager.LoadScene(String sceneName);

//비동기로드
IEnumerator AsyncSceneLoad()
{
      AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(String sceneName, LoadSceneMode loadSceneMode);

      while(asyncOperation.isDone)
      {
            yield return null;
      }
      Debug.Log("Scene Load Coplete!! : " + NextSceneName);
}
```

### ViewPort Position
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


