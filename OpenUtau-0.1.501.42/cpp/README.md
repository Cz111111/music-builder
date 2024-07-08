# Building

1. Install Bazelisk
2. `cd` to `cpp` folder
3. Run `bazelisk build //worldline` to build `dll` on Windows, `dylib` on macOS or `so` on Linux.
4. You can also run `bazelisk build //worldline:main` to build a executable version, though curve expressions won't be available.

Notes:
- On Windows omits `//` in commands.
- Recommends Visual Studio Code to leverage IntelliSense.
- If Bazel ever freezes on Windows, open up Task Manager and kill the Java process.
- `bazelisk clean` cleans up build cache.
- `bazelisk clean --expunge` cleans up build cache and dependencies.
