# Migration DB


```
 dotnet ef migrations add InitialIdentity -c AppDbContext --project .\DatPhongNhanh.Infrastructure\DatPhongNhanh.Infrastructure.csproj --startup-project .\DatPhongNhanh.WebApi\DatPhongNhanh.WebApi.csproj -o Data/Migrations/App
```

```
dotnet ef database update -c AppDbContext --project .\DatPhongNhanh.Infrastructure\DatPhongNhanh.Infrastructure.csproj --startup-project .\DatPhongNhanh.WebApi\DatPhongNhanh.WebApi.csproj
```

```
dotnet ef migrations remove -c AppDbContext --project .\DatPhongNhanh.Infrastructure\DatPhongNhanh.Infrastructure.csproj --startup-project .\DatPhongNhanh.WebApi\DatPhongNhanh.WebApi.csproj
```

```
dotnet ef database update --verbose --project .\DatPhongNhanh.Infrastructure\DatPhongNhanh.Infrastructure.csproj --startup-project .\DatPhongNhanh.WebApi\DatPhongNhanh.WebApi.csproj
```