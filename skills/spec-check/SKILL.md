---
name: spec-check
description: Extract implementation requirements from the current conversation context, read referenced project files and nearby directly relevant code, and judge whether each requirement is satisfied with scenario-based Pass/Fail/Warning evidence. Use when the user asks to run a spec check, verify implementation against conversation requirements, audit whether requested behavior is implemented, or produce scenario-based validation from discussed specs and code.
---

# Spec Check

Extract implementation requirements from the current conversation context, read the relevant code, derive testable scenarios, and judge whether the implementation satisfies each scenario.

## Workflow

### 1. Extract Requirements

Analyze the available conversation context and collect implementation requirements expressed by the user.

- Include requirements phrased as requests, constraints, expected behavior, edge cases, or corrections.
- Number each requirement.
- If conversation context appears incomplete because of summarization or missing earlier messages, explicitly mark uncertain requirements as inferred or unavailable instead of inventing details.
- Do not include assistant suggestions unless the user accepted or requested them.

### 2. Collect Code Evidence

Read the files referenced in the conversation.

- Read every explicitly mentioned project file path that is relevant to the requirements.
- If a referenced file depends on nearby code needed to verify the behavior, read the minimum directly related files needed to trace the implementation.
- Prefer fast local search tools such as `rg` to locate symbols, references, and call sites.
- If a file cannot be found or read, report it as a limitation in the result.
- Preserve existing user changes. Do not modify files while running this check.

### 3. Derive Scenarios

For each requirement, derive 1-3 verifiable scenarios.

- Write each scenario in the form: "When [condition/action], [expected result] should happen."
- Include edge cases when relevant, such as timing, state transitions, missing data, invalid input, repeated calls, or disabled state.
- Keep scenarios concrete enough to trace in code.

### 4. Check Implementation

Trace each scenario through the actual code.

Use these judgments:

- `✅ Pass`: Clearly implemented in code.
- `❌ Fail`: Missing, incomplete, unreachable, or implemented contrary to the requirement.
- `⚠️ Warning`: Implemented, but with a plausible risk, ambiguity, brittle behavior, or missing coverage.

For each judgment:

- Cite file path and line number evidence where possible.
- Use absolute file paths for file references when reporting in Codex.
- Explain the code path briefly enough that the result is auditable.
- Do not rely only on naming or comments if runtime behavior is not implemented.
- If tests exist and are relevant, consider them supporting evidence, not a replacement for checking implementation code.

### 5. Output Format

Return the result in Korean unless the user asks for another language.

Use this structure:

```markdown
## 명세 항목
1. [명세 내용]
2. [명세 내용]

## 시나리오 체크
| 시나리오 | 결과 | 근거 |
|----------|------|------|
| [When ... should ...] | ✅ Pass | [파일:라인 근거와 짧은 설명] |
| [When ... should ...] | ❌ Fail | [누락/오동작 이유] |
| [When ... should ...] | ⚠️ Warning | [잠재 문제] |

## Fail / Warning 수정 방향
### [시나리오명]
- 문제: [구체적 문제점]
- 수정 방향: [코드 수준의 수정 방법]
```

If there are no Fail or Warning results, end with:

```text
모든 명세가 구현되어 있습니다.
```

## Constraints

- This skill is for inspection only. Do not edit files unless the user explicitly asks for fixes after the check.
- Do not broaden the review into unrelated refactoring or style feedback.
- If evidence is insufficient, mark the scenario as `⚠️ Warning` and state what code or context is missing.
