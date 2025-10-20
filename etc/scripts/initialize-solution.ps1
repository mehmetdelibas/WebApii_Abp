abp install-libs

cd WebApii_Abp && dotnet run --migrate-database && cd -

cd WebApii_Abp.Blazor && dotnet dev-certs https -v -ep openiddict.pfx -p daeaa3aa-6156-4c21-a2fc-0a622d55b99c




exit 0