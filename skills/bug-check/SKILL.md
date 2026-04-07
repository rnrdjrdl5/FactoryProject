---
name: bug-check
description: Inspect game content code discussed or changed in the current conversation, read referenced files and direct dependencies, and report likely bugs in a concise checklist. Use when the user asks to run a bug check, review content-level game code such as Inventory, Equipment, City, Shop, Gacha, or Mission, or identify likely defects after implementation work without making fixes.
---

# Bug Check

Inspect the game content code discussed in the current conversation and report likely bugs concisely. Focus on content-level behavior rather than broad architecture or style review.

## Workflow

### 1. Identify Content Scope

Analyze the available conversation context and identify the game content units that were worked on or directly discussed.

- Treat content units such as Inventory, Equipment, City, Shop, Gacha, Mission, and similar gameplay content modules as in scope.
- List the files that were directly edited, referenced, or named in the conversation.
- If conversation context appears incomplete because of summarization or missing earlier messages, state the limitation instead of inventing missing files or requirements.

### 2. Collect Code Evidence

Read the relevant files before judging bugs.

- Read every conversation-mentioned file that is relevant to the identified content units.
- Read directly dependent files needed to understand the code path, including imports and references through patterns such as `RequireModule`, `GetModule`, or equivalent project APIs.
- Use fast local search tools such as `rg` to locate symbols, module lookups, event definitions, call sites, and matching lifecycle methods.
- Keep the read scope narrow: include direct dependencies needed for bug analysis, but do not expand into unrelated systems.
- Preserve existing user changes. Do not edit files while running this check.

### 3. Analyze Likely Bugs

Check the collected code against these categories:

- **Null or uninitialized references**: nullable references used without checks or required setup paths that can be skipped.
- **Event subscription cleanup**: events subscribed in `Begin()`, `OnEnable()`, constructors, or similar lifecycle methods without corresponding cleanup in `End()`, `OnDisable()`, disposal, or equivalent teardown.
- **UniTask / UniTaskScope misuse**: missing cancellation tokens, tasks not attached to an appropriate scope, missing `scope.Clear().RunTask()` or the project-equivalent pattern, or fire-and-forget async flow that can outlive the content lifecycle.
- **State transition errors**: wrong ordering, duplicate transitions, reentrancy risks, stale state after cancellation or failure, or transitions that bypass required cleanup.
- **Edge cases**: empty collection access, negative value handling, index range risks, missing data, repeated calls, disabled state, timing races, or invalid input.
- **Project pattern violations**: project-specific issues visible in code, such as using `GetModule` where `RequireModule` is required, or using global `App.XxxModule` access inside Controllers when the surrounding project pattern avoids it.

Keep each finding brief. Save deeper code-flow explanation for follow-up questions such as "자세히 설명해줘" or "이거 왜 버그야?"

### 4. Classify Results

Use these severities:

- `❌ 버그`: A concrete likely defect, missing cleanup, unsafe access, or behavior that can fail in a plausible runtime path.
- `⚠️ 주의`: A plausible risk, brittle assumption, missing guard, unclear lifecycle ownership, or insufficient evidence to prove safety.
- `✅ 양호`: A checked area that appears safe and has useful evidence. Include these sparingly; do not pad the table with obvious non-issues.

For each row:

- Cite file path and line number evidence where possible.
- Use absolute file paths for file references when reporting in Codex.
- Explain the issue in one concise sentence.
- Avoid broad style comments unless they create a plausible bug.

### 5. Output Format

Return the result in Korean unless the user asks for another language.

Use this structure:

```markdown
## 대상 컨텐츠
[컨텐츠 단위명] - [관련 파일 목록]

## 버그 가능성 체크
| 위치 | 설명 | 심각도 |
|------|------|--------|
| [파일명:라인] | [간략 설명] | ❌ 버그 |
| [파일명:라인] | [간략 설명] | ⚠️ 주의 |
| [파일명:라인] | [간략 설명] | ✅ 양호 |

## 요약
- 버그(❌): N건
- 주의(⚠️): N건
```

If there are no bug or warning findings, end with:

```text
버그 가능성이 발견되지 않았습니다.
```

## Constraints

- This skill is for inspection only. Do not edit files unless the user explicitly asks for fixes after the check.
- Do not broaden the result into a full code review, architecture critique, or unrelated refactoring list.
- If evidence is insufficient, mark the item as `⚠️ 주의` and state what code or context is missing.
