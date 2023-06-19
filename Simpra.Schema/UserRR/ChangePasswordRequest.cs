namespace Simpra.Schema.UserRR;

public class ChangePasswordRequest
{
    public string OldPassword { get; set; }
    public string Password { get; set; }
}
