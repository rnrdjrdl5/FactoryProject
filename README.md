# FactoryProject

Unity 게임 개발 프레임워크 기반 프로젝트입니다.

## 프로젝트 소개



## 주요 기능



## 시작하기

### 요구사항

- **Unity**: 6000.0.51f1 이상
- **C#**: .NET Framework 호환

### 설치 방법



## 아키텍처 개요

### Entry-Universe-Module 패턴
```
Entry (MonoBehaviour) - 게임 시작점
  └── Universe (싱글톤) - 게임 월드 관리
      └── Environment - 모듈 컨테이너
          └── Modules[] (ObjectPoolModule, StageModule, LocalDataModule 등)
```

### Actor-Trait 컴포지션 시스템
```
Actor (MonoBehaviour) - 게임 객체 베이스
  ├── ActorDatas[] - 커스텀 데이터
  ├── Traits[] - 기능 컴포넌트
  └── Children - 자식 액터
```

### 핵심 시스템

- **State 머신**: `State<OwnerType>` 상속으로 상태 관리
- **Flow 시스템**: 계층적 흐름 제어
- **BehaviourTree**: AI용 행동 트리
- **Processer 시스템**: Attribute 기반 자동 등록 로직 처리기
- **Panel (UI)**: `Panel : Actor` 확장 UI 시스템

## 프로젝트 구조

```
Assets/
├── DoughFramework/          # 프레임워크 코어
│   ├── Framework/           # 핵심 코드 및 생성 코드
│   ├── AI/                  # AI 문서 및 컨벤션
│   │   ├── FrameworkContext.md     # 프레임워크 가이드
│   │   ├── Convention/             # 코딩 컨벤션
│   │   └── Addressable/            # 에셋 관리
│   └── ...
├── Scenes/                  # 게임 씬
├── Settings/                # 프로젝트 설정
└── ...
```

## 코딩 가이드

코드 작성 시 다음 문서를 참고하세요:
- `Assets/DoughFramework/AI/FrameworkContext.md` - 프레임워크 전체 개요
- `Assets/DoughFramework/AI/Convention/` - 코딩 컨벤션

### 핵심 패턴

1. **생명주기 분리**: `Initialize()` (설정) → `Ready()` (시작)
2. **제네릭 Owner**: `State<T>`, `Flow<T>` 등에서 Owner 타입 명시
3. **Attribute 기반 등록**: 자동 등록 시스템 활용
4. **오브젝트 풀링**: GC 최적화를 위해 ObjectPoolModule 사용

## 빌드 & 개발

- **Unity**: Unity Editor에서 직접 열기 (자동 컴파일)
- **솔루션**: `DoughFramework.sln` (Visual Studio/Rider)
- **테스트**: Unity Test Framework (Window > General > Test Runner)

### Assembly Definitions

- `DoughFramework.Generated.asmdef` - 자동 생성 코드
- 기타 프로젝트별 asmdef 파일들

## 의존성

- **UniTask**: 비동기 처리 (async/await)
- **DOTween/DOTweenPro**: 트윈 애니메이션
- **Addressables**: 에셋 로드
- **Input System**: 입력 처리
- **URP**: 유니버설 렌더 파이프라인

## 기여하기



## 라이선스



## 연락처

