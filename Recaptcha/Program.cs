using Recaptcha;

using System.Drawing.Text;

var captcha = new CustomCaptcha();

// Display the CAPTCHA image to the user (e.g., in a Windows Forms or WPF application)
Console.WriteLine($"Please open the CAPTCHA image from the following path: {captcha.CaptchaImageFilePath}");

// Get user input
Console.Write("Enter the text from the CAPTCHA image: ");

var userInput = Console.ReadLine();

// Calculate the hash of the user's input
var inputHash = captcha.CalculateSha256(userInput);

// Verify the user's input
if (inputHash == captcha.AnswerHash)
{
    Console.WriteLine("Success! You entered the correct CAPTCHA.");
}
else
{
    Console.WriteLine("Incorrect CAPTCHA.");
}