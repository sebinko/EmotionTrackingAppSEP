# Emotions Tracking Website
Semester Project
## Components
### JavaDAO 
- This DAO which interacts with postgres DB, using Java. It also has a gRPC server.
#### Generating Protobuf classes from proto file
```bash
 mvn clean install
```
#### Running migrations (runs SQL files)
```bash
mvn flyway:migrate
```
### C# API
- Consists of multiple Projects:
	- Server/Api - C# Api Project
	- Shared/Entities  - Has the Entity classes of the domain
	- Protobuf - Protobuf client to connect to JavaDAO
#### Generating Protobuf classes from proto file
- This is done automatically upon bulding the project :)
### ProtoBuf
- Has .proto files
### Client/Frontend
- Blazor SSR frontend
