using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactBookApi.Migrations
{
    public partial class SeedStates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
             table: "states",
             columns: new[] { "StateName", "CountryId"},
             values: new object[,]
             {
                   { "Gujarat", 1 },
        { "Maharashtra", 1 },
        { "Punjab", 1 },
        { "Karnataka", 1 },
         { "California", 2 },
        { "Texas", 2 },
        { "New York", 2 },
        { "Florida", 2 },
         { "Auvergne-Rhône-Alpes", 3 },
        { "Bourgogne-Franche-Comté", 3 },
        { "Bretagne", 3 },
        { "Centre-Val de Loire", 3 },
         { "England", 4 },
        { "Scotland", 4 },
        { "Wales", 4 },
        { "Northern Ireland", 4 },
         { "Basque Country", 5 },
        { "Canary Islands", 5 },
        { "Cantabria", 5 },
        { "Castile and León", 5 }

             });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
              table: "states",
              keyColumn: "StateId",
              keyValues: new object[]
              {
                    1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20
              });

        }
    }
}
