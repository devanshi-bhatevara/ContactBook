using ContactBookApi.Dtos;
using ContactBookApi.Models;
using ContactBookApi.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet("GetAllContacts")]
        public IActionResult GetAllContacts(char? letter)
        {
            var response = _contactService.GetContacts(letter);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        } 
        
        [HttpGet("GetAllFavouriteContacts")]
        public IActionResult GetAllFavouriteContacts(char? letter)
        {
            var response = _contactService.GetFavouriteContacts(letter);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllContactsByPagination")]
        public IActionResult GetPaginatedContacts(char? letter,string? search,int page = 1, int pageSize = 4, string sortOrder = "asc")
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
      
                response = _contactService.GetPaginatedContacts(page, pageSize, letter,search, sortOrder);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        
        [HttpGet("GetAllContactsByPaginationSP")]
        public IActionResult GetPaginatedContactsSP(char? letter,string? search,int page = 1, int pageSize = 4, string sortOrder = "asc")
        {
            var response = new ServiceResponse<IEnumerable<ContactSPDto>>();
      
                response = _contactService.GetPaginatedContactsSP(page, pageSize, letter,search, sortOrder);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetContactsBasedOnBirthdayMonth")]
        public IActionResult GetContactsBasedOnBirthdayMonth(int month)
        {
            var response = new ServiceResponse<IEnumerable<ContactSPDto>>();
      
                response = _contactService.GetContactsBasedOnBirthdayMonth(month);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetContactByState")]

        public IActionResult GetContactByState(int state)
        {
            var response = new ServiceResponse<IEnumerable<ContactSPDto>>();
      
                response = _contactService.GetContactByState(state);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetContactsCountBasedOnCountry")]

        public IActionResult GetContactsCountBasedOnCountry(int countryId)
        {
            var response = new ServiceResponse<int>();
      
                response = _contactService.GetContactsCountBasedOnCountry(countryId);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetContactsCountBasedOnGender")]

        public IActionResult GetContactsCountBasedOnGender(string gender)
        {
            var response = new ServiceResponse<int>();
      
                response = _contactService.GetContactsCountBasedOnGender(gender);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        } 
        
        [HttpGet("GetAllFavouriteContactsByPagination")]
        public IActionResult GetFavouritePaginatedContacts(char? letter, string sortOrder="asc",int page = 1, int pageSize = 4)
        {
            var response = new ServiceResponse<IEnumerable<ContactDto>>();
      
                response = _contactService.GetFavouritePaginatedContacts(page, pageSize, letter, sortOrder);
         
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetContactsCount")]
        public IActionResult GetTotalCountOfContacts(char? letter,string? search)
        {
            var response = _contactService.TotalContacts(letter, search);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }    
        [HttpGet("GetFavouriteContactsCount")]
        public IActionResult GetTotalCountOfFavouriteContacts(char? letter)
        {
            var response = _contactService.TotalFavouriteContacts(letter);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetContactById/{id}")]

        public IActionResult GetContactById(int id)
        {
            var response = _contactService.GetContact(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("Create")]
        [Authorize]
        public IActionResult AddContact(AddContactDto contactDto)
        {

            var contact = new ContactBook()

            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                Address = contactDto.Address,
                Phone = contactDto.Phone,
                Email = contactDto.Email,
                FileName = contactDto.FileName,
                Gender = contactDto.Gender,
                IsFavourite = contactDto.IsFavourite,
                CountryId = contactDto.CountryId,
                StateId = contactDto.StateId,
                ImageByte = contactDto.ImageByte,
                birthDate = contactDto.birthDate
            };

            var result = _contactService.AddContact(contact);
            return !result.Success ? BadRequest(result) : Ok(result);

        }

        [HttpPut("ModifyContact")]
        [Authorize]

        public IActionResult UpdateContact(UpdateContactDto contactDto)
        {
            var contact = new ContactBook()

            {
                ContactId = contactDto.ContactId,
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                Address = contactDto.Address,
                Phone = contactDto.Phone,
                Email = contactDto.Email,
                FileName = contactDto.FileName,
                ImageByte = contactDto.ImageByte,
                Gender = contactDto.Gender,
                IsFavourite = contactDto.IsFavourite,
                CountryId = contactDto.CountryId,
                StateId = contactDto.StateId,
                birthDate = contactDto.birthDate

            };

            var response = _contactService.ModifyContact(contact);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }

        }

        [HttpDelete("Remove/{id}")]
        //[Authorize]

        public IActionResult RemoveContact(int id)
        {
            if (id > 0)
            {
                var response = _contactService.RemoveContact(id);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            else
            {
                var response = new ServiceResponse<string>();
                response.Success = false;
                response.Message = "Enter correct data please";
                return BadRequest(response);
            }

        }

    }
}
