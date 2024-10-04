# Emotions Tracking Website
Semester Project
## Components
### JavaDAO 
- This DAO which interacts with postgres DB, using Java. It also has a gRPC server.
### C# API
- Consists of multiple Projects:
	- Server/Api - C# Api Project
	- Shared/Entities  - Has the Entity classes of the domain
	- Protobuf - Protobuf client to connect to JavaDAO
### ProtoBuf
- Has .proto files
