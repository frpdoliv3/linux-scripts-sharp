test:
	dotnet test

coverage:
	rm -rf */TestResults docs/coveragereport
	dotnet test
	reportgenerator \
		-reports:"**/TestResults/**/coverage.cobertura.xml" \
		-targetdir:"docs/coveragereport" \
		-reporttypes:"Html;MarkdownSummaryGithub;Badges"
