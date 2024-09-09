using ContactBookClientApp.Implementation;
using ContactBookClientApp.Infrastructure;
using ContactBookClientApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContactBookClientApp.Controllers
{

    [AllowAnonymous] //public access pages.
                     //can be added on action methods too
    public class AuthController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;
        private string endPoint;
        private readonly IImageUpload _imageUpload;


        public AuthController(IHttpClientService _httpClientService, IConfiguration _configuration, IImageUpload imageUpload)
        {
            this._httpClientService = _httpClientService;
            this._configuration = _configuration;
            endPoint = _configuration["EndPoint:CivicaApi"];
            _imageUpload = imageUpload;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                if (register.File != null && register.File.Length > 0)
                {
                    var fileName = Path.GetFileName(register.File.FileName);
                    var fileExtension = Path.GetExtension(fileName).ToLower();

                    if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                    {
                        TempData["ErrorMessage"] = "Invalid file type. Only .jpg, .jpeg, and .png files are allowed.";
                        return View(register);
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        register.File.CopyTo(memoryStream);
                        register.ImageByte = memoryStream.ToArray();
                    }
                    fileName = _imageUpload.AddImageFileToPath(register.File);

                    register.FileName = fileName;
                }
                var apiUrl = $"{endPoint}Auth/Register";
                var response = _httpClientService.PostHttpResponseMessage<RegisterViewModel>(apiUrl, register, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successMessage = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successMessage);

                    TempData["successMessage"] = serviceResponse.Message;
                    return RedirectToAction("RegisterSuccess");
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
            return View(register);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var apiUrl = $"{endPoint}Auth/GetUserById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UpdateUserViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateUserViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    UpdateUserViewModel viewModel = serviceResponse.Data;

                    return View(viewModel);
                }
                else
                {
                    TempData["errorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UpdateUserViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["errorMessage"] = errorResponse.Message;

                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong. Please try after sometime";
                }
                return RedirectToAction("Index", "Home");

            }

        }


        [HttpPost]

        public IActionResult Edit(UpdateUserViewModel updateUserView)
        {
            if (ModelState.IsValid)
            {
                if (updateUserView.File != null && updateUserView.File.Length > 0)
                {
                    var fileName = Path.GetFileName(updateUserView.File.FileName);
                    var fileExtension = Path.GetExtension(fileName).ToLower();

                    if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                    {
                        TempData["ErrorMessage"] = "Invalid file type. Only .jpg, .jpeg, and .png files are allowed.";
                        return View(updateUserView);
                    }
                    using (var memoryStream = new MemoryStream())
                    {
                        updateUserView.File.CopyTo(memoryStream);
                        updateUserView.ImageByte = memoryStream.ToArray();
                    }

                    fileName = _imageUpload.AddImageFileToPath(updateUserView.File);
                    updateUserView.FileName = fileName;
                }

                else
                {
                    updateUserView.FileName = updateUserView.FileName;
                    updateUserView.ImageByte = updateUserView.ImageByte;
                }

                //if (updateUserView.RemoveImage)
                //{
                //    updateUserView.FileName = null;
                //    updateUserView.ImageByte = null;// Set FileName to null to remove the image
                //}

                var apiUrl = $"{endPoint}Auth/ModifyUser";

                HttpResponseMessage response = _httpClientService.PutHttpResponseMessage(apiUrl, updateUserView, HttpContext.Request);

                if (response.IsSuccessStatusCode)
                {
                    string successMessage = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successMessage);

                    TempData["successMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index", "Home");
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
                    return RedirectToAction("Index", "Home");

                }
            }

            return View(updateUserView);
        }


        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel login)

        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/Login";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, login, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successMessage = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successMessage);
                    string token = serviceResponse.Data;
                    Response.Cookies.Append("jwtToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });
                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return RedirectToAction("Index", "Home");
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
            return View(login);
        }

        public IActionResult Details(string id)
        {
            var apiUrl = $"{endPoint}Auth/GetUserById/" + id;
            var response = _httpClientService.GetHttpResponseMessage<UserViewModel>(apiUrl, HttpContext.Request);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<UserViewModel>>(data);

                if (serviceResponse != null && serviceResponse.Success && serviceResponse.Data != null)
                {
                    return View(serviceResponse.Data);
                }
                else
                {
                    TempData["errorMessage"] = serviceResponse?.Message;
                    return RedirectToAction("Index","Home");
                }
            }
            else
            {
                string errorData = response.Content.ReadAsStringAsync().Result;
                var errorResponse = JsonConvert.DeserializeObject<ServiceResponse<UserViewModel>>(errorData);
                if (errorResponse != null)
                {
                    TempData["errorMessage"] = errorResponse.Message;

                }
                else
                {
                    TempData["errorMessage"] = "Something went wrong. Please try after sometime";
                }
                return RedirectToAction("Index", "Home");

            }

        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/ForgetPassword";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);

                    TempData["SuccessMessage"] = serviceResponse.Message;
                    return View("Login");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);
                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong. Please try again later.";
                    }
                }
            }
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            ForgetPasswordViewModel viewModel = new ForgetPasswordViewModel();
            viewModel.UserName = @User.Identity.Name;
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ChangePassword(ForgetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = $"{endPoint}Auth/ForgetPassword";
                var response = _httpClientService.PostHttpResponseMessage(apiUrl, viewModel, HttpContext.Request);
                if (response.IsSuccessStatusCode)
                {
                    string successResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(successResponse);

                    TempData["SuccessMessage"] = serviceResponse?.Message;
                    return View("Login");
                }
                else
                {
                    string errorResponse = response.Content.ReadAsStringAsync().Result;
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<string>>(errorResponse);
                    if (serviceResponse != null)
                    {
                        TempData["ErrorMessage"] = serviceResponse.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Something went wrong. Please try again later.";
                    }
                }
            }
            return View(viewModel);
        }

    }

}
