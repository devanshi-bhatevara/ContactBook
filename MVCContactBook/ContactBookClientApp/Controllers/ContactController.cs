using ContactBookClientApp.Infrastructure;
using ContactBookClientApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Xml.XPath;

namespace ContactBookClientApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private string endPoint;
        private readonly IImageUpload _imageUpload;


        public ContactController(IHttpClientService _httpClientService, IConfiguration _configuration,  IImageUpload imageUpload)
        {
            this._httpClientService = _httpClientService;
            this._configuration = _configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
            _imageUpload = imageUpload;
        }
        
        public IActionResult GetContactsBasedOnBirthdayMonth(int? month)
        {
            if(month.HasValue)
            {
                ViewBag.Month = month;
                var apiUrl = $"{endPoint}Contact/GetContactsBasedOnBirthdayMonth/?month=" + month;
                var response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactSPViewModel>>>
            (apiUrl, HttpMethod.Get, HttpContext.Request);


                if (response.Success)
                {
                    return View(response.Data);
                }
                return View(new List<ContactSPViewModel>());


            }
            else
            {
                return View();

            }

        }

        public IActionResult GetContactByState(int? stateId)
        {
            ViewBag.Countries = GetCountry();
            ViewBag.States = GetState();
            if (stateId.HasValue)
            {
                ViewBag.State = stateId;
                var apiUrl = $"{endPoint}Contact/GetContactByState/?state=" + stateId;
                var response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactSPViewModel>>>
            (apiUrl, HttpMethod.Get, HttpContext.Request);


                if (response.Success)
                {
                    return View(response.Data);
                }
                return View(new List<ContactSPViewModel>());


            }
            else
            {
                return View();

            }
        }

        public IActionResult GetContactsCountBasedOnCountry(int? countryId)
        {
            ViewBag.Countries = GetCountry();
            if (countryId.HasValue)
            {
                ViewBag.Country = countryId;
                var apiUrl = $"{endPoint}Contact/GetContactsCountBasedOnCountry/?countryId=" + countryId;
                var response = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
            (apiUrl, HttpMethod.Get, HttpContext.Request);


                if (response.Success)
                {
                    ViewBag.Count = response.Data; 
                    return View(response.Data);
                }
                ViewBag.Count = response.Data;
                return View();


            }
            else
            {
                return View();

            }

        }
         public IActionResult GetContactsCountBasedOnGender()
        {
           
                var apiUrlMale = $"{endPoint}Contact/GetContactsCountBasedOnGender/?gender=M";
                var responseMale = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
            (apiUrlMale, HttpMethod.Get, HttpContext.Request);

            var apiUrlFemale = $"{endPoint}Contact/GetContactsCountBasedOnGender/?gender=F";
            var responseFemale = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
        (apiUrlFemale, HttpMethod.Get, HttpContext.Request);


            if (responseMale.Success && responseFemale.Success)

            {
                ViewBag.FemaleCount = responseFemale.Data;
                ViewBag.MaleCount = responseMale.Data;
                return View();
            }
            return View();

        }




        public IActionResult Index(char? letter,string? search, int page = 1, int pageSize = 2, string sortOrder = "asc")
        {
            var apiGetContactsUrl = "";

            var apiGetCountUrl = "";

            var apiGetLettersUrl = $"{endPoint}Contact/GetAllContacts";

            ViewBag.Search = search;

            if (letter != null && search!=null)
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetAllContactsByPagination/?letter={letter}&search={search}&page={page}&pageSize={pageSize}&sortOrder={sortOrder}";
                apiGetCountUrl = $"{endPoint}Contact/GetContactsCount/?letter={letter}&search={search}";
            }
            else if (letter != null && search == null)
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetAllContactsByPagination/?letter={letter}&page={page}&pageSize={pageSize}&sortOrder={sortOrder}";
                apiGetCountUrl = $"{endPoint}Contact/GetContactsCount/?letter={letter}";
            }
            else if (letter == null && search!=null)
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetAllContactsByPagination/?search={search}&page={page}&pageSize={pageSize}&sortOrder={sortOrder}";
                apiGetCountUrl = $"{endPoint}Contact/GetContactsCount/?search={search}";
            }
            else
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetAllContactsByPagination/?page={page}&pageSize={pageSize}&sortOrder={sortOrder}";
                apiGetCountUrl = $"{endPoint}Contact/GetContactsCount";

            }
            ServiceResponse<int> countOfContact = new ServiceResponse<int>();

            countOfContact = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                (apiGetCountUrl, HttpMethod.Get, HttpContext.Request);

            int totalCount = countOfContact.Data;

            if (totalCount == 0)
            {
                // Return an empty view
                return View(new List<ContactViewModel>());
            }
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);


            if (page > totalPages)
            {
                // Redirect to the first page with the new page size
                return RedirectToAction("Index", new { letter, page = 1, pageSize });
            }
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.Letter = letter;
            ViewBag.SortOrder = sortOrder;
            ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetContactsUrl, HttpMethod.Get, HttpContext.Request);

            ServiceResponse<IEnumerable<ContactViewModel>>? getLetters = new ServiceResponse<IEnumerable<ContactViewModel>>();

            getLetters = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetLettersUrl, HttpMethod.Get, HttpContext.Request);

            if (getLetters.Success)
            {
                var distinctLetters = getLetters.Data.Select(contact => char.ToUpper(contact.FirstName.FirstOrDefault()))
                                                            .Where(firstLetter => firstLetter != default(char))
                                                            .Distinct()
                                                             .OrderBy(letter => letter)
                                                            .ToList();
                ViewBag.DistinctLetters = distinctLetters;

            }
     


            if (response.Success)
            {
                return View(response.Data);
            }

            return View(new List<ContactViewModel>());
        }
        
        public IActionResult Favourites(char? letter, int page = 1, int pageSize = 2, string sortOrder = "asc")
        {
            var apiGetContactsUrl = "";
            var apiGetLettersUrl = $"{endPoint}Contact/GetAllFavouriteContacts";
            var apiGetCountUrl = "";
            if (letter != null)
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetAllFavouriteContactsByPagination/?letter={letter}&page={page}&pageSize={pageSize}&sortOrder={sortOrder}";
                apiGetCountUrl = $"{endPoint}Contact/GetFavouriteContactsCount/?letter={letter}";
            }
            else
            {
                apiGetContactsUrl = $"{endPoint}Contact/GetAllFavouriteContactsByPagination/?page={page}&pageSize={pageSize}&sortOrder={sortOrder}";
                apiGetCountUrl = $"{endPoint}Contact/GetFavouriteContactsCount";

            }
            ServiceResponse<int> countOfContact = new ServiceResponse<int>();

            countOfContact = _httpClientService.ExecuteApiRequest<ServiceResponse<int>>
                (apiGetCountUrl, HttpMethod.Get, HttpContext.Request);

            int totalCount = countOfContact.Data;

            if (totalCount == 0)
            {
                // Return an empty view
                return View(new List<ContactViewModel>());
            }
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);


            if (page > totalPages)
            {
                // Redirect to the first page with the new page size
                return RedirectToAction("Favourites", new { letter, page = 1, pageSize });
            }
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.Letter = letter;
            ViewBag.SortOrder = sortOrder;
            ServiceResponse<IEnumerable<ContactViewModel>> response = new ServiceResponse<IEnumerable<ContactViewModel>>();

            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetContactsUrl, HttpMethod.Get, HttpContext.Request);
            ServiceResponse<IEnumerable<ContactViewModel>>? getLetters = new ServiceResponse<IEnumerable<ContactViewModel>>();

            getLetters = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<ContactViewModel>>>
                (apiGetLettersUrl, HttpMethod.Get, HttpContext.Request);

            if (getLetters.Success)
            {
                var distinctLetters = getLetters.Data.Select(contact => char.ToUpper(contact.FirstName.FirstOrDefault()))
                                                            .Where(firstLetter => firstLetter != default(char))
                                                            .Distinct()
                                                             .OrderBy(letter => letter)
                                                            .ToList();
                ViewBag.DistinctLetters = distinctLetters;

            }


            if (response.Success)
            {
                return View(response.Data);
            }

            return View(new List<ContactViewModel>());
        }

        [Authorize]

        public IActionResult Create()
        {
            AddContactViewModel viewModel = new AddContactViewModel();
            viewModel.States = GetState();
            viewModel.Country = GetCountry();
            return View(viewModel);
        }

        [Authorize]

        [HttpPost]
        public IActionResult Create(AddContactViewModel contactViewModel)
        {
            contactViewModel.States = GetState();
            contactViewModel.Country = GetCountry();
            if (ModelState.IsValid)
            {   
                if (contactViewModel.File != null && contactViewModel.File.Length > 0)
                {
                    var fileName = Path.GetFileName(contactViewModel.File.FileName);
                    var fileExtension = Path.GetExtension(fileName).ToLower();

                    if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                    {
                        TempData["ErrorMessage"] = "Invalid file type. Only .jpg, .jpeg, and .png files are allowed.";
                        return View(contactViewModel);
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        contactViewModel.File.CopyTo(memoryStream);
                        contactViewModel.ImageByte = memoryStream.ToArray();
                    }
                    fileName = _imageUpload.AddImageFileToPath(contactViewModel.File);

                    contactViewModel.FileName = fileName;
                }

                var apiUrl = $"{endPoint}Contact/Create";
                var response = _httpClientService.PostHttpResponseMessage<AddContactViewModel>(apiUrl, contactViewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successMessage = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successMessage);

                    TempData["successMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    string errorMessage = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorMessage);
                    if (errorResponse != null)
                    {
                        TempData["errorMessage"] = errorResponse.Message;

                    }
                    else
                    {
                        TempData["errorMessage"] = "Something went wrong. Please try after sometime";
                    }
                   

                }

            }

            return View(contactViewModel);
        }
        public IActionResult Details(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UpdateContactViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateContactViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["errorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["errorMessage"] = errorResponse.Message;

                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong. Please try after sometime";
                }
                return RedirectToAction("Index");

            }

        }

        [Authorize]

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UpdateContactViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateContactViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    UpdateContactViewModel viewModel = serviceResponse.Data;

                    viewModel.States = GetState();
                    viewModel.Countries = GetCountry();
                    return View(viewModel);
                }
                else
                {
                    TempData["errorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["errorMessage"] = errorResponse.Message;

                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong. Please try after sometime";
                }
                return RedirectToAction("Index");

            }

        }

        [Authorize]

        [HttpPost]

        public IActionResult Edit(UpdateContactViewModel updateContactView)
        {
            updateContactView.Countries = GetCountry();
            updateContactView.States = GetState();

            if (ModelState.IsValid)
            {
                if (updateContactView.File != null && updateContactView.File.Length > 0)
                {
                    var fileName = Path.GetFileName(updateContactView.File.FileName);
                    var fileExtension = Path.GetExtension(fileName).ToLower();

                    if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                    {
                        TempData["ErrorMessage"] = "Invalid file type. Only .jpg, .jpeg, and .png files are allowed.";
                        return View(updateContactView);
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        updateContactView.File.CopyTo(memoryStream);
                        updateContactView.ImageByte = memoryStream.ToArray();
                    }

                    fileName = _imageUpload.AddImageFileToPath(updateContactView.File);
                    updateContactView.FileName = fileName;
                }

                else
                {
                    updateContactView.FileName = updateContactView.FileName;
                    updateContactView.ImageByte = updateContactView.ImageByte;
                }

                if (updateContactView.RemoveImage)
                {
                    updateContactView.FileName = null;
                    updateContactView.ImageByte = null;// Set FileName to null to remove the image
                }

                var apiUrl = $"{endPoint}Contact/ModifyContact";

                HttpResponseMessage response = _httpClientService.PutHttpResponseMessage(apiUrl, updateContactView, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string successMessage = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successMessage);

                    TempData["successMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    string errorMessage = response.Content.ReadAsStringAsync().Result;
                    var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorMessage);
                    if (errorResponse != null)
                    {
                        TempData["errorMessage"] = errorResponse.Message;

                    }
                    else
                    {
                        TempData["errorMessage"] = "Something went wrong. Please try after sometime";
                    }
                    return RedirectToAction("Index");

                }
            }
        
            return View(updateContactView);
        }

        [Authorize]

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var apiUrl = $"{endPoint}Contact/GetContactById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<ContactViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["errorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<ContactViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["errorMessage"] = errorResponse.Message;

                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong. Please try after sometime";
                }
                return RedirectToAction("Index");

            }
        }

        [Authorize]

        [HttpPost]
        public IActionResult DeleteConfirm(int contactId)
        {
            var apiUrl = $"{endPoint}Contact/Remove/" + contactId;
            var response = _httpClientService.ExecuteApiRequest<ServiceResponse<string>>($"{apiUrl}", HttpMethod.Delete, HttpContext.Request);
            if (response.Success)
            {
                TempData["successMessage"] = response.Message;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["errorMessage"] = response.Message;
                return RedirectToAction("Index");
            }

        }

        [ExcludeFromCodeCoverage]
        private List<StateViewModel> GetState()
        {
            ServiceResponse<IEnumerable<StateViewModel>> response = new ServiceResponse<IEnumerable<StateViewModel>>();
            string endPoint = _configuration["EndPoint:CivicaApi"];
            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<StateViewModel>>>
                ($"{endPoint}State/GetAllStates", HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return response.Data.ToList();
            }
            return new List<StateViewModel>();
        }

        [ExcludeFromCodeCoverage]
        private List<CountryViewModel> GetCountry()
        {
            ServiceResponse<IEnumerable<CountryViewModel>> response = new ServiceResponse<IEnumerable<CountryViewModel>>();
            string endPoint = _configuration["EndPoint:CivicaApi"];
            response = _httpClientService.ExecuteApiRequest<ServiceResponse<IEnumerable<CountryViewModel>>>
                ($"{endPoint}Country/GetAllCountries", HttpMethod.Get, HttpContext.Request);

            if (response.Success)
            {
                return response.Data.ToList();
            }
            return new List<CountryViewModel>();
        }

    }
}
