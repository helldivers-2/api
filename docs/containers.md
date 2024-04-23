# Running in a container

It is possible to self-host the API by either running it directly or by running it as a container.
This allows you to access the API in secured networks (although you'll still need to
be able to access the ArrowHead API endpoints of course), or to run an instance
with modified settings (such as rate limits).

### Building the container
First make sure you have cloned the repository:
```shell
git clone --recurse-submodules https://github.com/helldivers-2/api.git
```

Then build the container so it's ready for local use (run this *inside* the cloned folder):
```shell
docker build -f ./src/Helldivers-2-API/Dockerfile -t helldivers2-api .
```

and finally run the image:
```shell
docker run -p 8080:8080 helldivers2-api
```

### Building the container with OpenAPI
By default the OpenAPI specifications aren't bundled in the container image,
you can enable OpenAPI support by building the container with these flags:
```shell
docker build --build-arg="OPENAPI=true" -f ./src/Helldivers-2-API/Dockerfile -t helldivers2-api .
```

### Customizing the runtime of the API
Other settings, such as rate limits, features, ... can be customized at runtime
and don't need to be defined at build time of the container.

For a **full** list of options that can be customized, please inspect
[appsettings.json](https://github.com/helldivers-2/api/blob/master/src/Helldivers-2-API/appsettings.json) as all these settings can be overriden.

For example, the `appsettings.json` configuration contains a key like this:
```json
{
  // omitted for brevity
  "Helldivers": {
    "Synchronization": {
      "IntervalSeconds": 20,
      // omitted for brevity
  }
}
```
The `IntervalSeconds` defines the rate at which the API will synchronize with ArrowHead's
API endpoints, in this example it will synchronize every `20` seconds.
Let's say we want to update this in our own container to run every `10` seconds instead.

We can do this by defining an environment variable that overrides the configuration defined
in `appsettings.json`.
in this case the property we're overriding is set under
`Helldivers:Synchronization:IntervalSeconds`, so we need to declare a variable called
`Helldivers__Synchronization__IntervalSeconds` with value `10`

You can define environment variables for your container like this:
```shell
docker run -p 8080:8080 -e "Helldivers__Synchronization__IntervalSeconds=10" helldivers2-api
```

You can read more about using environment variables to override configuration [here](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-8.0#naming-of-environment-variables)

### Overriding rate limits
You can override the rate limits by overriding the following configuration:
```json
{
  "Helldivers": {
    "API": {
      "RateLimit": 5,
      "RateLimitWindow": 10
    }
  }
}
```

The `RateLimit` (overridable with `-e Helldivers__API__RateLimit`) is how many requests can be made in the timeframe,
the `RateLimitWindow` (overridable with `-e Helldivers__API__RateLimitWindow`) is how many seconds before the `RateLimit`
resets again.

Increasing the `RateLimit`, decreasing the `RateLimitWindow` or both will effectively increase how many requests you can
make to the application.

Alternatively, if you use the hosted versions you can request an API key that allows for higher rate limits
by sponsoring this project! (if you self host you can generate your own keys too!).
