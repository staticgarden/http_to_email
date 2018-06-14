.PHONY: deploy
deploy:
	./build.sh
	sls deploy

.PHONY: logs
logs:
	sls logs --function hello --tail
