namespace PishgamanTask.Maui.Dtos
{
    public class ServiceResponse
    {
        public record class RegisterResponse(bool flag, string Message);
        public record class LoginResponse(bool flag, string token, string Message);
    }
}
