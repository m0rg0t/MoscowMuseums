# Portable source regression tests

Invalidate cached Phone images when their path changes, handle an empty path, and notify direct-image assignments in both Phone model copies.

Run with .NET SDK 10.0.401:

```sh
dotnet run --project tests/Portable.Tests/Portable.Tests.csproj --configuration Release
```

Also run `dotnet run --project tests/Legacy.Tests/Legacy.Tests.csproj --configuration Release` for the older Phone model copy.

The harness compiles the actual listed production source with C# 5. It uses synthetic fixtures and minimal explicit platform adapters, with network/storage/service access throwing if exercised. No package sources or external test dependencies are used. Image adapters retain URIs only and do not download images. The HTML adapter is identity-only: native HTML conversion is not covered.

This does not build or verify the original Windows/Windows Phone applications, UI, devices, native libraries, JSON services, geographic services, or image rendering. Historical application project targets, vendor dependencies and assets remain unchanged. Dependency migration requires matching historical SDKs/references or a separately scoped supported-platform migration; no exhaustive dependency vulnerability audit is claimed.
