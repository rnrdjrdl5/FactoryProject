# FactoryProject

A Unity game development project based on the framework.

## Project Overview



## Key Features



## Getting Started

### Requirements

- **Unity**: 6000.0.51f1 or later
- **C#**: .NET Framework compatible

### Installation



## Architecture Overview

### Entry-Universe-Module Pattern
```
Entry (MonoBehaviour) - Game entry point
  └── Universe (Singleton) - Game world manager
      └── Environment - Module container
          └── Modules[] (ObjectPoolModule, StageModule, LocalDataModule, etc.)
```

### Actor-Trait Composition System
```
Actor (MonoBehaviour) - Base game object
  ├── ActorDatas[] - Custom data
  ├── Traits[] - Feature components
  └── Children - Child actors
```

### Core Systems

- **State machine**: State management via `State<OwnerType>` inheritance
- **Flow system**: Hierarchical flow control
- **BehaviourTree**: AI behavior tree
- **Processer system**: Attribute-based auto-registration logic handlers
- **Panel (UI)**: UI system extending `Panel : Actor`

## Project Structure

```
Assets/
├── DoughFramework/          # Framework core
│   ├── Framework/           # Core code and generated code
│   ├── AI/                  # AI docs and conventions
│   │   ├── FrameworkContext.md     # Framework guide
│   │   ├── Convention/             # Coding conventions
│   │   └── Addressable/            # Asset management
│   └── ...
├── Scenes/                  # Game scenes
├── Settings/                # Project settings
└── ...
```

## Coding Guide

When writing code, refer to:
- `Assets/DoughFramework/AI/FrameworkContext.md` - Framework overview
- `Assets/DoughFramework/AI/Convention/` - Coding conventions

### Core Patterns

1. **Lifecycle separation**: `Initialize()` (setup) → `Ready()` (start)
2. **Generic owner**: Explicit owner types in `State<T>`, `Flow<T>`, etc.
3. **Attribute-based registration**: Use the auto-registration system
4. **Object pooling**: Use ObjectPoolModule to optimize GC

## Build & Development

- **Unity**: Open in Unity Editor (auto-compile)
- **Solution**: `DoughFramework.sln` (Visual Studio/Rider)
- **Tests**: Unity Test Framework (Window > General > Test Runner)

### Assembly Definitions

- `DoughFramework.Generated.asmdef` - Auto-generated code
- Other project-specific asmdef files

## Dependencies

- **UniTask**: Async processing (async/await)
- **DOTween/DOTweenPro**: Tween animations
- **Addressables**: Asset loading
- **Input System**: Input processing
- **URP**: Universal Render Pipeline

## Contributing



## License



## Contact
