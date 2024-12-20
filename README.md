## Dev Requirements:

- [Dotnet SDK](https://dotnet.microsoft.com/en-us/download)
  - `>=8.0` required
- Docker

## Run Tests (Local / Dotnet):

From the root directory:
`dotnet test -e DOTNET_ENVIRONMENT=Development`

## Run Tests (Docker):

From the root directory
1. Build container image (below)
2. Run container w/ credentials of a valid user
`docker run -e "Credentials__username=<email>" -e "Credentials__password=<password>" <image name>`

## Build Container Image:

Run the below in the repository root directory
`docker build --tag "<image name>" -f Amezmo.Tests.E2E/. .`

Run the image / tests (only would work headless for now)
`docker run -e "Credentials__username=<email>" -e "Credentials__password=<password>" <image name>`

##  Settings

- Local Development: .env file and Dotnet User Secrets file
- Container (available environment args):
 - `Credentials__username=`
 - `Credentials__password=`
 - `BASE_URL=`

This will be replaced by secret ops

