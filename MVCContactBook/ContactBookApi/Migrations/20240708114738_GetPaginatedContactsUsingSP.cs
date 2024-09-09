using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactBookApi.Migrations
{
    public partial class GetPaginatedContactsUsingSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"CREATE or ALTER PROCEDURE GetAllContactsSPWithCodeFirst
(
  @letter CHAR(1) = null,
  @Search NVARCHAR(50) = null,
  @page INT,
  @pageSize INT,
  @sortOrder NVARCHAR(50)
)
AS
BEGIN

 DECLARE @skip INT = (@page -1) * (@pageSize);
 SELECT c.ContactId,c.FirstName, c.LastName,c.Phone,c.Address,c.Email,c.Gender,
	c.IsFavourite,ct.CountryName,s.StateName,c.FileName,c.ImageByte,c.birthDate
	from ContactBook c
	inner join countries ct
	on c.CountryId = ct.CountryId
	inner join states s
	on c.StateId = s.StateId
	WHERE 
	(@letter IS NULL OR c.FirstName LIKE @letter + '%') 
AND (@Search IS NULL OR c.FirstName LIKE '%' + @Search + '%' OR c.LastName LIKE '%' + @Search + '%')	
	ORDER BY 
    CASE WHEN @sortOrder = 'asc' THEN FirstName END ASC ,
    CASE WHEN @sortOrder = 'desc' THEN FirstName END DESC,
	CASE WHEN @sortOrder = 'asc' THEN LastName END ASC ,
    CASE WHEN @sortOrder = 'desc' THEN LastName END DESC
	OFFSET @skip ROWS
  FETCH NEXT @pageSize ROWS ONLY
END;");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS GetAllContactsSPWithCodeFirst");

        }
    }
}
