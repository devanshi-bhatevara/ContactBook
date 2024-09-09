using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactBookApi.Migrations
{
    public partial class AddStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE OR ALTER PROCEDURE GetContactsBasedOnBirthdayMonthCF
(
@month NVARCHAR(100)
)
AS

BEGIN
SELECT c.ContactId,c.FirstName,c.LastName,c.Phone,
c.Address,c.Email,c.Gender,c.IsFavourite,ct.CountryName,
s.StateName, c.FileName,c.ImageByte, c.birthDate
FROM ContactBook c
INNER JOIN countries ct ON c.CountryId = ct.CountryId
INNER JOIN states s ON c.StateId = s.StateId
Where MONTH(c.birthDate) = @month
END
            ");

            migrationBuilder.Sql(@"
            CREATE OR ALTER PROCEDURE GetContactsCountBasedOnCountryCF
(
@countryId INT
)
AS
BEGIN
SELECT COUNT(ContactId) as CountOfContacts
FROM ContactBook 
Where CountryId = @countryId
END
             ");

            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE GetContactByStateCF
(@STATE INT)
AS
BEGIN
SELECT C.ContactId,C.FIRSTNAME,C.LASTNAME,C.Birthdate,C.PHONE,C.ADDRESS,C.EMAIL,C.GENDER,C.ISFAVOURITE,D.STATENAME,E.COUNTRYNAME,C.FILENAME,C.IMAGEBYTE
FROM CONTACTBOOK C
JOIN STATES D
ON C.STATEID = D.STATEID
JOIN COUNTRIES E
ON C.COUNTRYID = E.COUNTRYID
 
WHERE C.StateId = @STATE
END
        
");

            migrationBuilder.Sql(@"
          CREATE OR ALTER PROCEDURE GetContactsCountBasedOnGenderCF
(
@gender NVARCHAR(1)
)
AS
BEGIN
SELECT COUNT(ContactId) as CountOfContacts
FROM ContactBook 
Where Gender = @gender
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS GetContactsBasedOnBirthdayMonthCF");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS GetContactsCountBasedOnCountryCF");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS GetContactByStateCF");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS GetContactsCountBasedOnGenderCF");
        }
    }
}
