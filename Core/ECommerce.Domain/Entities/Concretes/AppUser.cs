using ECommerce.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Domain.Entities.Concretes;

public class AppUser : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public string? UserName { get; set; }
    public string? Role { get; set; }
    public bool EmailComfirm { get; set; } = false;
    // -------------------------------------
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpireTime { get; set; }
    public DateTime? RefreshTokenCreateTime { get; set; } = DateTime.Now;
    // -------------------------------------
    public string? EmailComfirmToken { get; set; }
    public DateTime? EmailComfirmExpireTime { get; set; }
    public DateTime? EmailComfirmCreateTime { get; set; } = DateTime.Now;
    // -------------------------------------
    public string? RePasswordToken { get; set; }
    public DateTime? RePasswordTokenExpireTime { get; set; }
    public DateTime? RePasswordTokenCreateTime { get; set; } = DateTime.Now;
    // -------------------------------------

    // Navigation Property
    public virtual ICollection<Order> Orders { get; set; }
}
