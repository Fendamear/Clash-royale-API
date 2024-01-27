docker buildx create --use --name buildx_instance

Start-Sleep -Seconds 5

cd ./ClashRoyaleApi/ClashRoyaleApi

docker buildx build --platform linux/amd64,linux/arm64 -t fendamear/clashroyaleapi --push .