---
name: folder-context-md
description: Create or update very short folder-level Markdown context files that help Codex analyze a codebase. Use when the user asks for README.md, AGENTS.md, or another MD note that summarizes a folder, module, content pack, feature area, pipeline, ownership boundary, or "what Codex should know before editing here."
---

# Folder Context MD

## Goal

Write a short, accurate folder map for Codex. Prefer a compact `README.md` for explanation and `AGENTS.md` only for instructions/rules.

## Workflow

1. Inspect the target folder before writing: list direct children, read key entry files, and follow only direct dependencies needed to understand the main flow.
2. Identify the folder's purpose, main path, subfolder roles, and edit boundary.
3. Draft in English unless the user explicitly asks otherwise.
4. Keep it short: usually 6-12 lines, under 120 words.
5. Use exact class, file, and folder names from the code.
6. Avoid speculative design intent, long architecture notes, and details likely to go stale.

## Format

Use this shape by default:

```md
# FolderName

One-line purpose.

Main path:
`EntryA` -> `MapperB` -> `RouterC` -> `HandlerD`.

Folders:
`Raw/` does X. `Token/` does Y. `Layer/` does Z.

Rule:
Keep feature/content-specific behavior outside this folder unless explicitly changing this module.
```

## Quality Bar

- Prefer a useful map over a complete explanation.
- Name the first files Codex should read.
- State boundaries with "Rule:" only when the boundary is clear from project structure or user instruction.
- If the folder is content-specific, mention what is safe to modify inside it.
- If the folder is framework/core code, mention that feature/content behavior should live elsewhere.
