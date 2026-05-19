# Input Structure

## Summary

Input is collected into a single `InputContext` and routed through a priority layer stack.

```text
InputCollectorAbility
 -> InputContext
 -> InputProcessorAbility
 -> BaseInputLayerProcessor.ProcessInput
```

Framework owns input collection, the input context data shape, and the layer stack. Contents own all input meaning.

## Framework Layout

```text
Assets/DoughFramework/Framework/Script/Input
├─ InputContext.cs
├─ InputStateType.cs
├─ InputCollectorAbility.cs
├─ InputProcessorAbility.cs
└─ BaseInputLayerProcessor.cs
```

`InputContext` contains:

| Field | Meaning |
| --- | --- |
| `KeyCode` | Source key or button. `KeyCode.None` is used for axis input. |
| `StateType` | `Pressed`, `Held`, or `Released`. |
| `Axis` | Current horizontal/vertical axis value. |
| `ScreenPosition` | Current mouse position. |

## Content Layout

```text
Assets/Contents/InputRealm/Script/Realm
├─ InputRealm.cs
└─ InputRealmProcessorAbility.cs
```

```text
Assets/Contents/Player/Script/Input
└─ PlayerInputLayerProcessor.cs
```

```text
Assets/Contents/Team/Script/Input
├─ TeamInputLayerProcessor.cs
└─ TeamPopupInputLayerProcessor.cs
```

```text
Assets/Contents/Equipment/Script/Input
├─ EquipmentInputLayerProcessor.cs
└─ EquipmentPopupInputLayerProcessor.cs
```

```text
Assets/Contents/Inventory/Script/Input
├─ InventoryInputLayerProcessor.cs
└─ InventoryPopupInputLayerProcessor.cs
```

## Content Input Ownership

- Player movement uses axis input from `KeyCode.None`.
- Player pick uses `KeyCode.Z`.
- Player skills use skill bindings stored by `KeyCode`.
- Team open uses `KeyCode.F1`.
- Equipment open uses `KeyCode.F2`.
- Inventory open uses `KeyCode.I`.
- Popup close uses `KeyCode.Escape` or the popup's own menu key.

Layers return `Pass` to continue routing, `Consume` after handling input, and `Block` to stop lower-priority layers.
