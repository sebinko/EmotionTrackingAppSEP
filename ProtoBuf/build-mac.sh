protoc --plugin=./bin/protoc-gen-grpc-java -I=./proto/ --java_out=../JavaDAO/src/main/java/ --grpc-java_out=../JavaDAO/src/main/java/ ./proto/*.proto

