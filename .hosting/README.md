# DevFoundry hosting reference

Reference-only preparation, 2026-09-10. The inert `manifest.json` is agent-intake metadata, not application configuration. No runtime, data, domain, name, account or deployment trigger changes here. Existing workflows may still run after a future merge; inspect their effects first.

Keep the toolkit local. Its Vue frontend uses a .NET API; uploading the frontend to static hosting does not make those API-backed tools work. DF1 inventories that execution boundary and gives a future static site honest local-run instructions. No hosted backend is needed for a landing page.

DF2 prepares only synthetic, reviewed public examples. Do not publish real tool inputs, JWTs, credentials, local paths or generated private output. DF3 preserves the current CLI/API/package interfaces and local invocation. A later branding change must distinguish display strings from technical identifiers and configuration; no candidate name is selected here.

Read the current README and any applicable repository instructions before implementation. Root AGENTS.md and CLAUDE.md returned 404 during this pass; no replacement harness is introduced. Syntax: `python -m json.tool .hosting/manifest.json`. Follow-on code, build or publication work requires the actual .NET/frontend checks and public-output review; JSON validation establishes none of those results.

Retain the previous static build and existing local invocation as rollback. A new domain does not authorize exposing the local API, collecting user inputs or introducing shared identity infrastructure. Private operational receipts and unregistered naming candidates remain outside this public repository.
