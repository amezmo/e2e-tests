## Dev Requirements:

- [Dotnet SDK](https://dotnet.microsoft.com/en-us/download)
  - `>=8.0` required

## Run Tests (Local / Dotnet):

From the root directory:
`dotnet test Amezmo.Tests.E2E`

## Build Container Image:

Run the below in the repository root directory
`docker build --tag "<tag>" -f Amezmo.Tests.E2E/Dockerfile .`

Run the image / tests (only would work headless for now)
`docker run "<tag_previous>"`

## Issues

- Tests fail in headless mode
- Tests will not run in container (because of headless mode)

##  Settings file

In the root directory of Amezmo.Tests.E2E a settings file should be added:

```json
{
  "username" : "<user email>",
  "password" : "<user password>>",
  "baseUrl" : "https://amezmo.com/",
  "headless" : "true"
}
```

This will be replaced by secret ops

