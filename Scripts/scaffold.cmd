RUN dotnet ef dbcontext scaffold "Filename=../Database/Northwind.db" Microsoft.EntityFrameworkCore.Sqlite --namespace Packt.Shared --data-annotations