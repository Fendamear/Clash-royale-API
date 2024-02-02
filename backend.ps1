cd ./ClashRoyaleApi/ClashRoyaleApi

dotnet clean

docker buildx build --platform linux/amd64,linux/arm64 -t fendamear/clashroyaleapi --push .