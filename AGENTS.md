# Repository Guidelines

## Project Structure & Modules
- `OpenRPA/`: Main WPF application (`net462`). Output to `debug/` or `dist/`.
- `OpenRPA.*`: Feature plugins (Windows, IE, Java, SAP, Storage, etc.).
- `OpenRPA.Interfaces/`: Shared contracts and types used across plugins.
- `OpenRPA.SetupProject/`: WiX installer project (`.wixproj`, `.wxs`).
- `RDService.sln`: Windows service–related solution.
- `docs/`, `manifests/`, assets (e.g., `OpenRPA-logo.png`), signing and packaging scripts (`sign.ps1`, `openrpa.nsi`).

## Build, Run, and Packaging
- Restore/build: `dotnet build OpenRPA.sln -c Debug` (or `Release`).
- Run locally: launch `OpenRPA` as startup project in Visual Studio, or execute `debug/OpenRPA.exe` (Debug) or `dist/OpenRPA.exe` (Release).
- Build installer (WiX): `msbuild OpenRPA.SetupProject/OpenRPA.SetupProject.wixproj /p:Configuration=Release` (requires WiX Toolset on Windows).
- Optional NSIS: `makensis openrpa.nsi` (requires NSIS). Signing uses local keys via `sign.ps1` (not required for local testing).

## Coding Style & Naming
- C#: 4‑space indent; braces on new lines; one type per file.
- Naming: PascalCase for classes/methods/properties; camelCase for locals/params; `_camelCase` for private fields.
- Use `var` for obvious types; prefer async/await where applicable; follow surrounding style in each project.
- XAML: pair `*.xaml` and `*.xaml.cs`; place images in `Resources/` and embed via project settings.

## Testing Guidelines
- Current repo has limited unit tests; contributions adding tests are welcome.
- Create `OpenRPA.<Module>.Tests` (target `net462`) using MSTest/NUnit.
- Name tests `MethodName_State_Expected`; keep deterministic and isolated.
- Run tests: `dotnet test` (Windows recommended for UI-dependent code).
- For UI/features, include minimal repro steps and a sample workflow in the PR.

## Commit & Pull Requests
- Commits: short, imperative subject (e.g., “Fix logging attributes”); group related changes.
- Link issues: `Fixes #123` or `Refs #123`.
- PRs: clear description, scope, testing notes, screenshots for UI, and docs updates under `docs/` when relevant.
- Follow `CONTRIBUTING` and `CODE_OF_CONDUCT.md`. Keep changes focused and consistent with existing patterns.

## Security & Configuration
- Never commit secrets, signing material, or API keys. Use local env/config (`App.config`, user secrets) for development.
- Packaging/signing is optional for contributors; maintainers handle official releases.
