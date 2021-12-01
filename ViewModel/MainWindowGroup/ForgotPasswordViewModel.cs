using MailKit.Net.Smtp;
using MimeKit;
using MailKit;
using Model;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Input;
using ViewModel.Helper;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;

namespace ViewModel
{
    public class ForgotPasswordViewModel : Navigation.ViewModelBase
    {
        private const string EmailUsername = "EzHRM.Demo@gmail.com";
        private const string EmailPassword = "jqrpwaypbglldcvi";

        private static ForgotPasswordViewModel _instance;
        public static ForgotPasswordViewModel Instance { get => _instance; }

        public const int ConfirmationStringLength = 7;
        public int WHATISTHIS { get => ConfirmationStringLength; }

        public const int AllowTry = 5;

        private string _confirmationString;
        private int _wrongStringCounter = 0;

        public string InputComfirmStr { get; set; }
        public bool WaitingForString { get; set; }
        public int WrongStringCounter { get => _wrongStringCounter; set => _wrongStringCounter = value; }

        public bool IsWrongString { get; set; }

        public bool NewPasswordError { get; set; }
        public string NewPasswordErrorString { get; set; }

        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public string AccountName { get; set; }

        protected RelayCommand<object> _goBackToLoginCommand;
        public ICommand GoBackToLoginCommand => _goBackToLoginCommand ??= new RelayCommand<object>(param => { MainViewModel.Instance.ToLogin.Execute(param); });

        protected RelayCommand<object> _confirmStringCommand;
        public ICommand ConfirmStringCommand => _confirmStringCommand ??= new RelayCommand<object>(param => {
            if (_confirmationString == InputComfirmStr)
            {
                WaitingForString = false;
            }
            else
            {
                IsWrongString = true;
                WrongStringCounter--;
                if (_wrongStringCounter == 0)
                {
                    MainViewModel.Instance.ToLogin.Execute(param);
                }
            }
        });

        protected RelayCommand<object> _changePasswordCommand;
        public ICommand ChangePasswordCommand => _changePasswordCommand ??= new RelayCommand<object>(param => {
            if (NewPassword.Length == 0)
            {
                NewPasswordErrorString = "Password can't be empty.";
                NewPasswordError = true;
            }
            else if (NewPassword.Length > AccountModel.MaxPasswordLength)
            {
                NewPasswordErrorString = $"Your password is too long (Max is {AccountModel.MaxPasswordLength}).";
                NewPasswordError = true;
            }
            else if (NewPassword.Length < AccountModel.MinPasswordLength)
            {
                NewPasswordErrorString = $"Your password is too short (Min is {AccountModel.MinPasswordLength}).";
                NewPasswordError = true;
            }
            else if (NewPassword != ConfirmPassword)
            {
                NewPasswordErrorString = "Confirm password and new password doesn't match up.";
                NewPasswordError = true;
            }
            else
            {
                SaveToNewPassword();
                MainViewModel.Instance.ToLogin.Execute(param);
            }
        });

        public ForgotPasswordViewModel()
        {
            _instance = this;
        }

        public override void OnGetTo()
        {
            base.OnGetTo();
            WaitingForString = true;
            IsWrongString = false;
            InputComfirmStr = "";
            NewPasswordError = true;
            AccountName = LoginViewModel.Instance.UserName;
            SendEmail();
        }

        protected void SendEmail()
        {
            _wrongStringCounter = AllowTry;
            _confirmationString = "";
            Random random = new Random();
            for (int i = 0; i < ConfirmationStringLength; i++)
            {
                _confirmationString += (char)random.Next('0', 'z');
            }

            EmployeeModel employee = EmployeeModel.GetEmployeeFromAccount(AccountName);
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("EzHRMApp", EmailUsername));
            message.To.Add(MailboxAddress.Parse(employee.EmailVanPhong));
            message.Subject = "Security code";
            message.Body = new TextPart("html")
            {
                //Text = $"Your security code is: {_confirmationString}"
                Text = ConstructEmail(_confirmationString)
            };

            Task.Factory.StartNew(() =>
            {
                SmtpClient client = new SmtpClient();
                try
                {
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate(EmailUsername, EmailPassword);
                    client.Send(message);
                    Debug.WriteLine("Email sent");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            });
        }

        protected void SaveToNewPassword()
        {
            AccountModel account = AccountModel.GetAccount(AccountName);
            account.ChangePassword(NewPassword);
        }

        protected string ConstructEmail(string code)
        {
            return "<!DOCTYPE html>\n\n<html lang=\"en\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:v=\"urn:schemas-microsoft-com:vml\">\n<head>\n<title></title>\n<meta charset=\"utf-8\"/>\n<meta content=\"width=device-width, initial-scale=1.0\" name=\"viewport\"/>\n<!--[if mso]><xml><o:OfficeDocumentSettings><o:PixelsPerInch>96</o:PixelsPerInch><o:AllowPNG/></o:OfficeDocumentSettings></xml><![endif]-->\n<style>\n\t\t* {\n\t\t\tbox-sizing: border-box;\n\t\t}\n\n\t\tbody {\n\t\t\tmargin: 0;\n\t\t\tpadding: 0;\n\t\t}\n\n\t\t/*th.column{\n\tpadding:0\n}*/\n\n\t\ta[x-apple-data-detectors] {\n\t\t\tcolor: inherit !important;\n\t\t\ttext-decoration: inherit !important;\n\t\t}\n\n\t\t#MessageViewBody a {\n\t\t\tcolor: inherit;\n\t\t\ttext-decoration: none;\n\t\t}\n\n\t\tp {\n\t\t\tline-height: inherit\n\t\t}\n\n\t\t@media (max-width:520px) {\n\t\t\t.row-content {\n\t\t\t\twidth: 100% !important;\n\t\t\t}\n\n\t\t\t.stack .column {\n\t\t\t\twidth: 100%;\n\t\t\t\tdisplay: block;\n\t\t\t}\n\t\t}\n\t</style>\n</head>\n<body style=\"background-color: #000000; margin: 0; padding: 0; -webkit-text-size-adjust: none; text-size-adjust: none;\">\n<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"nl-container\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; background-color: #000000;\" width=\"100%\">\n<tbody>\n<tr>\n<td>\n<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row row-1\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" width=\"100%\">\n<tbody>\n<tr>\n<td>\n<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"row-content stack\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; color: #000000;\" width=\"500\">\n<tbody>\n<tr>\n<td class=\"column\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; font-weight: 400; text-align: left; vertical-align: top; padding-top: 5px; padding-bottom: 5px; border-top: 0px; border-right: 0px; border-bottom: 0px; border-left: 0px;\" width=\"100%\">\n<table border=\"0\" cellpadding=\"10\" cellspacing=\"0\" class=\"text_block\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\" width=\"100%\">\n<tr>\n<td>\n<div style=\"font-family: Tahoma, Verdana, sans-serif\">\n<div style=\"font-size: 12px; font-family: Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 14.399999999999999px; color: #393d47; line-height: 1.2;\">\n<p style=\"margin: 0; font-size: 12px; text-align: center;\"><span style=\"font-size:30px;color:#076ae3;\"><strong>SECURITY CODE</strong></span></p>\n</div>\n</div>\n</td>\n</tr>\n</table>\n<table border=\"0\" cellpadding=\"10\" cellspacing=\"0\" class=\"text_block\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\" width=\"100%\">\n<tr>\n<td>\n<div style=\"font-family: sans-serif\">\n<div style=\"font-size: 12px; mso-line-height-alt: 14.399999999999999px; color: #393d47; line-height: 1.2; font-family: Arial, Helvetica Neue, Helvetica, sans-serif;\">\n<p style=\"margin: 0; font-size: 12px; text-align: center;\"><span style=\"font-size:22px;color:#ffffff;\">Your security code for resetting your password is</span></p>\n</div>\n</div>\n</td>\n</tr>\n</table>\n<table border=\"0\" cellpadding=\"30\" cellspacing=\"0\" class=\"text_block\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\" width=\"100%\">\n<tr>\n<td>\n<div style=\"font-family: Tahoma, Verdana, sans-serif\">\n<div style=\"font-size: 12px; font-family: Tahoma, Verdana, Segoe, sans-serif; mso-line-height-alt: 24px; color: #393d47; line-height: 2;\">\n<p style=\"margin: 0; font-size: 12px; text-align: center; letter-spacing: 5px;\"><strong><span style=\"font-size:46px;background-color:#000000;color:#ffffff;\">" 
                + code 
                + "</span></strong></p>\n</div>\n</div>\n</td>\n</tr>\n</table>\n<table border=\"0\" cellpadding=\"10\" cellspacing=\"0\" class=\"text_block\" role=\"presentation\" style=\"mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;\" width=\"100%\">\n<tr>\n<td>\n<div style=\"font-family: sans-serif\">\n<div style=\"font-size: 14px; mso-line-height-alt: 16.8px; color: #393d47; line-height: 1.2; font-family: Arial, Helvetica Neue, Helvetica, sans-serif;\">\n<p style=\"margin: 0; font-size: 14px; text-align: center;\"><span style=\"color:#ffffff;\">If this isn't intentional then just ignore this.</span></p>\n</div>\n</div>\n</td>\n</tr>\n</table>\n</td>\n</tr>\n</tbody>\n</table>\n</td>\n</tr>\n</tbody>\n</table>\n</td>\n</tr>\n</tbody>\n</table><!-- End -->\n</body>\n</html>";
        }
    }
}
