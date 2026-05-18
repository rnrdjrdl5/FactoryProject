# Input Structure

## Summary

Input is split into `Raw -> Token -> Content`.

```text
RawInputContext
 -> TokenInputContext
    -> ContentInputContext<TInputType>
```

Framework owns raw collection, token mapping, token routing, and base layer types. Each content module owns its own content input type and input layer.

## Framework Layout

```text
Assets/DoughFramework/Framework/Script/Input
├─ Raw
│  ├─ InputCollectorAbility.cs
│  ├─ RawInputContext.cs
│  ├─ RawInputRequest.cs
│  ├─ RawInputStateData.cs
│  └─ RawInputType.cs
├─ Token
│  ├─ TokenInputBindingData.cs
│  ├─ TokenInputContext.cs
│  ├─ TokenInputMapperProcessor.cs
│  ├─ TokenInputProcessorAbility.cs
│  ├─ TokenInputRouterProcessor.cs
│  └─ TokenInputType.cs
├─ Layer
│  ├─ BaseTokenInputLayerProcessor.cs
│  └─ BasePopupTokenInputLayerProcessor.cs
├─ Content
│  ├─ ContentInputContext.cs
│  └─ BaseContentInputLayerProcessor.cs
└─ Realm
   └─ FrameworkInputRealm.cs
```

## Content Layout

```text
Assets/Contents/InputRealm/Script/Realm
├─ InputRealm.cs
└─ InputRealmProcessorAbility.cs
```

```text
Assets/Contents/Player/Script/Input
├─ PlayerInputType.cs
└─ PlayerInputLayerProcessor.cs
```

```text
Assets/Contents/Team/Script/Input
├─ TeamInputType.cs
├─ TeamInputLayerProcessor.cs
└─ TeamPopupInputLayerProcessor.cs
```

```text
Assets/Contents/Equipment/Script/Input
├─ EquipmentInputType.cs
├─ EquipmentInputLayerProcessor.cs
└─ EquipmentPopupInputLayerProcessor.cs
```

```text
Assets/Contents/Inventory/Script/Input
├─ InventoryInputType.cs
├─ InventoryInputLayerProcessor.cs
└─ InventoryPopupInputLayerProcessor.cs
```

## Default Token Bindings

`TokenInputBindingData` maps platform keys to neutral framework token input.

| KeyCode | TokenInputType |
| --- | --- |
| `Mouse0` | `PointerPrimary` |
| `Mouse1` | `PointerSecondary` |
| `Z` | `Action1` |
| `Q` | `Action2` |
| `E` | `Action3` |
| `R` | `Action4` |
| `Space` | `Action5` |
| `LeftShift` | `Action6` |
| `F1` | `Menu1` |
| `F2` | `Menu2` |
| `I` | `Menu3` |
| `Escape` | `Cancel` |

Axis input is emitted as `TokenInputType.MoveAxis`.

## Content Input Ownership

- Player input is handled by `PlayerInputLayerProcessor`.
- Team open input is handled by `TeamInputLayerProcessor`.
- Equipment open input is handled by `EquipmentInputLayerProcessor`.
- Inventory open input is handled by `InventoryInputLayerProcessor`.
- Popup close input is handled by each popup's input layer.

`InputRealm` only provides the token input layer stack. It does not own content input mapping.
