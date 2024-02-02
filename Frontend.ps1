cd React-frontend\clashroyaleapi

docker buildx build --platform linux/amd64,linux/arm64 -t fendamear/clashroyalefrontend --push .