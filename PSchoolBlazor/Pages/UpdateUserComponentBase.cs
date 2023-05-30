using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using PSchoolBlazor.Dto;
using PSchoolBlazor.Enum;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using static System.Net.WebRequestMethods;

namespace PSchoolBlazor.Pages
{
    public class UpdateUserComponentBase : ComponentBase
    {
        [Inject] public IJSRuntime JsRuntime { get; set; }
        [Inject] public NavigationManager NavManager { get; set; }
        [Parameter] public int UserId { get; set; }
        public UserDto UserMasterVM { get; set; } = new UserDto();
        protected override async Task OnInitializedAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                var response = await httpClient.GetAsync($"{Const.ApiUrl}/Users{UserId}");
                var result = await response.Content.ReadAsStringAsync();
                var Out = await response.Content.ReadFromJsonAsync<UserDto>();
                UserMasterVM = Out;
            };
        }
            protected async void FormSubmitted()
        {
            bool confirmed = true; //await JsRuntime.InvokeAsync<bool>("confirm", "Are you sure You want to Submit Form?");
            if (confirmed)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    var body = new StringContent(JsonConvert.SerializeObject(UserMasterVM), Encoding.UTF8, "application/json");
                    var response = await httpClient.PutAsync($"{Const.ApiUrl}/User{UserId}", body);
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