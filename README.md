# RedisApp

## Description

Redis is an in-memory data structure store, used as a database, cache and message broker.
It supports data structures such as strings, hashes, lists, sets, sorted sets with range
queries, bitmaps, hyperloglogs, geospatial indexes with radius queries and streams.

## Commands 

The application supports the following commands:

- SET <VARNAME> <VALUE>: Set VARNAME to VALUE
- GET <VARNAME>: Retrieve and return value of VARNAME
- INCR <VARNAME> [COUNT]: Increment VARNAME by 1 or COUNT
- DECR <VARNAME> [COUNT]: Decrement VARNAME by 1 or COUNT
- RPUSH <VARNAME> <VALUE>: Push VALUE to the right of list VARNAME
- RPOP <VARNAME>: Pop right value from list VARNAME
- LPUSH <VARNAME> <VALUE>: Push VALUE to the lift of list VARNAME
- LPOP <VARNAME> <VALUE>: Pop left value from list VARNAME
- LINDEX <VARNAME> <INDEX>: Get value at index INDEX from LIST VARNAME
- EXPIRES <VARNAME> <TIME>: Expire value of VARNAME after TIME seconds
- LOGOUT : Exit the current session

## Deployment

### Service

The service can be launched in two ways:

#### Docker

The latest build is deployed as a docker image on docker hub under :  [fareseid/redisserver](https://hub.docker.com/repository/docker/fareseid/redisserver) 

```bash
docker run -d -p [SERVER_PORT]:5050 fareseid/redisserver
```

#### Command Prompt

To build & Launch the server manually:
```bash
cd ./RedisServer
dotnet build
dotnet run 
```
The  [SERVER_PORT] is 5050.

### Client

#### Deployment
In order to run the application, you need to configure first the port of the server.
To do so, replace the RedisServerHost value in [APP_SETTINGS](./RedisClient/appsettings.json) by [SERVER_PORT]. 

The client can only be run manually:
```bash
cd ./RedisClient
dotnet build
dotnet run 
```
#### Default Credentials
In order to login you can use the below credentials:
> username : User1
>
> password : Password1