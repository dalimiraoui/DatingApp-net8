
# Entity Framework local:
Make Sure to install dotnet-ef:
`dotnet tool install --global dotnet-ef`

To Add migrations:
`dotnet ef migrations add [MigrationName] -o Data/Migrations`

To remove migrations
`dotnet ef migrations remove -c EAssessmentDbContext -s ./EAssessment.WebApi/ -p ./EAssessment.Data/`

To update migrations
`dotnet ef database update -c EAssessmentDbContext -s ./EAssessment.WebApi/ -p ./EAssessment.Data/`

To Revert to Specific migration
`dotnet ef database update [MigrationNameYouWantToRevertTo] -c EAssessmentDbContext -s ./EAssessment.WebApi/ -p ./EAssessment.Data/`

To Revert to 0
`dotnet ef database update 0 -c EAssessmentDbContext -s ./EAssessment.WebApi/ -p ./EAssessment.Data/ ; dotnet ef migrations remove -c EAssessmentDbContext -s ./EAssessment.WebApi/ -p ./EAssessment.Data/`
