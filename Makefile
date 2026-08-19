test:
	dotnet test

coverage:
	rm -rf */TestResults coveragereport
	dotnet test
	reportgenerator \
		-reports:"**/TestResults/**/coverage.cobertura.xml" \
		-targetdir:"coveragereport" \
		-reporttypes:"Html;MarkdownSummaryGithub;Badges"
