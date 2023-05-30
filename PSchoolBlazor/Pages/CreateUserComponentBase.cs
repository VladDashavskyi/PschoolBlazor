using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using PSchoolBlazor.Dto;
using PSchoolBlazor.Enum;
using System;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using static System.Net.WebRequestMethods;



namespace PSchoolBlazor.Pages
{
    public class CreateUserComponentBase : ComponentBase
    {
        [Inject] public IJSRuntime JsRuntime { get; set; }
        [Inject] public NavigationManager NavManager { get; set; }
        [Parameter] public int RoleId { get; set; } = 1;
        public string Pid { get; set; }

        public UserDto UserMasterVM { get; set; } = new UserDto();
        public List<UserDto> Users { get; set; } 

        protected override async Task OnInitializedAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                var response = await httpClient.GetAsync($"https://localhost:7080/Users?role=1");
                var result = await response.Content.ReadAsStringAsync();
                var Out = await response.Content.ReadFromJsonAsync<List<UserDto>>();
                Users = Out;
            };
        }
        protected async void FormSubmitted()
        {
            int b = 0;
            if (!string.IsNullOrEmpty(Pid))
            {
                int.TryParse(Pid, out b);
            }

            bool confirmed = true; /*await JsRuntime.InvokeAsync<bool>("confirm", "Are you sure You want to Submit Form?");*/
            if (confirmed)
            {
                var user = new UserDto()
                {
                    FirstName = UserMasterVM.FirstName,
                    LastName = UserMasterVM.LastName,
                    Email = UserMasterVM.Email,
                    DateOfBirth = UserMasterVM.DateOfBirth,
                    Role = (Role)RoleId,
                    ParentId = b,
                    UserId = 0


                };  
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    var body = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("https://localhost:7080/User", body);
                    var result = await response.Content.ReadAsStringAsync();

                };

            }
            else
            {
                await JsRuntime.InvokeVoidAsync("Alert", "Cancel");
            }
            StateHasChanged();
            NavManager.NavigateTo("/", false);

        }
    }

   
}