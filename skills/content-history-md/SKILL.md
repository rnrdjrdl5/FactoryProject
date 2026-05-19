---
name: content-history-md
description: Append Korean, human-readable project history under a target Content folder. Use when the user asks to record, update, append, or maintain history for a Content/module folder while keeping history out of normal LLM-readable context.
---

# Content History MD

## Goal

Append human-readable Korean history under each target Content folder without treating that history as normal LLM context.

## Critical Rules

- Write all history entries in Korean.
- Store history under the target Content folder, not under this skill folder.
- Use this path shape: `.human-history/YYYY/MM/YYYY-MM-DD.md`.
- Create `.human-history/AGENTS.md` when missing.
- Do not read, search, summarize, or analyze existing `.human-history/` date files unless the user explicitly asks to read history.
- When appending history, append to today's file without reading existing history.
- If today's file does not exist, create it without inspecting older date files.
- Use only minimal filesystem inspection to identify the target Content folder and check whether the target directory/file exists.
- Prefer the target folder's `README.md` or `AGENTS.md` for LLM-readable context. Do not use `.human-history/` as context.

## Access Guard

Create `.human-history/AGENTS.md` with this content if it is missing:

```md
# Human History Access Rule

This directory contains human-readable project history.

Rules for AI agents:
- Do not read, search, summarize, or analyze history files in this directory unless the user explicitly asks to read history.
- Appending a new history entry is allowed without reading existing history.
- When appending, write only to `YYYY/MM/YYYY-MM-DD.md`.
- Do not inspect older date files for context, deduplication, or cleanup unless explicitly requested.
- Prefer the parent folder's `README.md` or `AGENTS.md` for LLM-readable context.
```

## Append Workflow

1. Resolve the target Content folder from the user's request or current task.
2. Create `.human-history/`, `.human-history/YYYY/`, and `.human-history/YYYY/MM/` as needed.
3. Ensure `.human-history/AGENTS.md` exists; write the access guard if missing.
4. Append a new entry to `.human-history/YYYY/MM/YYYY-MM-DD.md` without reading that file.
5. Keep entries factual and concise. Mention changed systems, decisions, and important consequences.

## Entry Format

Use this shape:

```md

## HH:mm - Short Korean title

- Korean bullet describing what changed.
- Korean bullet describing why it matters or the resulting behavior.
```

For larger work, use 3-5 bullets. Do not include exhaustive diffs, logs, or implementation dumps.
