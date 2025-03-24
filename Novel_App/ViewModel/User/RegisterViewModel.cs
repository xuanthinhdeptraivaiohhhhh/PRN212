using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Novel_App.Model;
using Novel_App.Utilities;
using Novel_App.ViewModel;
using System.Windows.Input;
using System.Windows;
using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;


namespace Novel_App.ViewModel.User
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly NovelAppContext _context = new NovelAppContext();

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string NumberPhone { get; set; } = string.Empty;  
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; } 

        public ICommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(async _ => await Register());
        }

        private async Task Register()
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(UserName)) errors.Add("Tên đăng nhập không được để trống.");
            if (string.IsNullOrWhiteSpace(Password)) errors.Add("Mật khẩu không được để trống.");
            if (Password != ConfirmPassword) errors.Add("Mật khẩu xác nhận không khớp.");
            if (!IsValidPassword(Password)) errors.Add("Mật khẩu phải có ít nhất 8 ký tự, bao gồm cả chữ cái và số.");
            if (string.IsNullOrWhiteSpace(Email) || !IsValidEmail(Email)) errors.Add("Email không hợp lệ.");
            if (!string.IsNullOrWhiteSpace(NumberPhone) && !IsValidPhoneNumber(NumberPhone)) errors.Add("Số điện thoại không hợp lệ. Vui lòng nhập 10-11 chữ số.");

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (await _context.UserAccounts.AnyAsync(u => u.UserName == UserName))
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newUser = new UserAccount
            {
                UserName = UserName,
                Password = HashPassword(Password),
                Email = Email,
                FullName = FullName,
                NumberPhone = NumberPhone,  // Lưu số điện thoại
                DateOfBirth = DateOfBirth,
                Gender = Gender,
                CreationDate = DateTime.Now,
                Status = true,
                Type = "normal"
            };

            _context.UserAccounts.Add(newUser);
            await _context.SaveChangesAsync();

            MessageBox.Show("Đăng ký thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidPhoneNumber(string number)
        {
            return Regex.IsMatch(number, @"^\d{10,11}$");
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 8 &&
                   password.Any(char.IsLetter) &&
                   password.Any(char.IsDigit);
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            byte[] hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            return Convert.ToBase64String(hashBytes);
        }
    }
}