using Common.Core.Models.Person;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Common.Core.Helpers
{
    public static class CustomValidator
    {
        public static bool IsPhoneNumberValid(string number)
        {
            return Regex.Match(number, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}").Success;
        }

        public static (string message, bool isvalid) ValidatePerson(PersonRequest model)
        {
            string response = "";
            bool isvalid = true;

            model.Active = model.Active.ToLower();
            model.Sex = model.Sex.ToLower();

            //Todo : we can move gender and status to Enums
            List<string> gender = new List<string>(){"m", "f"};

            List<string> status = new List<string>(){"true", "false"};

            if (!CustomValidator.IsPhoneNumberValid(model.Mobile))
            {
                response = $"sorry mobile number {model.Mobile} is not valid.";
                isvalid = false;
            }
            else if (!status.Contains(model.Active))
            {
                response = $"sorry {model.Active} is not valid, you can only enter true or false.";
                isvalid = false;
            }
            else if (!gender.Contains(model.Sex))
            {
                response = $"sorry {model.Sex} is not valid, you can only enter M or F.";
                isvalid = false;
            }
            return (response, isvalid);
        }

    }
}
