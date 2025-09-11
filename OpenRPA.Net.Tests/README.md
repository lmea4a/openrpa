# OpenRPA.Net.Tests

This project contains unit and integration tests for OpenRPA networking.

## Test Types
- Unit: offline/pending-queue behavior without any external dependency.
- Integration (Mock): in-process WebSocket mock server to simulate disconnect/reconnect.
- Integration (External): connects to a real OpenCore instance and validates reconnect + pending flush end-to-end.

## External Integration Test Configuration
Set the following environment variables before running the test `Connect_OpenCore_Drop_During_Update_Flush_On_Reconnect`:

- `OPENRPA_WS_URL`: WebSocket endpoint (e.g., `wss://app.openiap.io/ws`).
- `OPENRPA_JWT`: JWT token with permission to pop/update workitems.
- `OPENRPA_WIQ` or `OPENRPA_WIQID`: Queue name or id to pop from.
- `OPENRPA_ADD_WORKITEM` (optional): `true` to enqueue a test workitem prior to popping.

If these variables are not set, the test is marked as Inconclusive and skipped.

## Running Tests
- Visual Studio: Test Explorer → Run All (Windows recommended).
- CLI: `dotnet test` (ensure .NET Framework targeting pack is installed on Windows).

## CI (GitHub Actions) Example
Add a workflow job that only runs external integration tests when secrets are present:

```yaml
jobs:
  tests:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4
      - name: Setup MSBuild
        uses: microsoft/setup-msbuild@v2
      - name: Set env for external IT
        if: secrets.OPENRPA_WS_URL && secrets.OPENRPA_JWT && vars.OPENRPA_WIQ
        run: |
          echo OPENRPA_WS_URL=${{ secrets.OPENRPA_WS_URL }} >> $Env:GITHUB_ENV
          echo OPENRPA_JWT=${{ secrets.OPENRPA_JWT }} >> $Env:GITHUB_ENV
          echo OPENRPA_WIQ=${{ vars.OPENRPA_WIQ }} >> $Env:GITHUB_ENV
      - name: Restore & Test
        run: |
          msbuild OpenRPA.sln /t:Restore
          dotnet test OpenRPA.Net.Tests/OpenRPA.Net.Tests.csproj --logger trx
```

Tip: You can also filter by MSTest category if you prefer:
- Include external tests: `dotnet test --filter TestCategory=External`
- Exclude external tests: `dotnet test --filter TestCategory!=External`

