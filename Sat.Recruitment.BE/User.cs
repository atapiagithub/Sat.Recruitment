using System;

namespace Sat.Recruitment.BE
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string UserType { get; set; }
        public decimal Money { get; set; }

        public User(string name, string email, string adress, string phone, string userType, string money)
        {
            this.Name = name;
            this.Email = email;
            this.Address = adress;
            this.Phone = phone;
            this.UserType = userType;
            if (!string.IsNullOrEmpty(money))
            {
                if (decimal.TryParse(money, out decimal auxmoney))
                {
                    this.Money = auxmoney;
                }
            }

            //Estrategias segun tipo de usuario (se establecen al crearlo)
            switch (this.UserType)
            {
                case "Normal":
                    if (this.Money > 100)
                    {
                        this.Money += this.Money * Convert.ToDecimal(0.12);
                    }
                    if (this.Money < 100 && this.Money > 10)
                    {
                        this.Money += this.Money * Convert.ToDecimal(0.8);
                    }
                    break;
                case "SuperUser":
                    if (this.Money > 100)
                    {
                        this.Money += this.Money * Convert.ToDecimal(0.20);
                    }
                    break;
                case "Premium":
                    if (this.Money > 100)
                    {
                        this.Money += this.Money * 2;
                    }
                    break;
            }

        }

        public bool IsValid(out string validation)
        {
            validation = "";

            if (string.IsNullOrEmpty(this.Name))
                //Validate if Name is null
                validation += "The name is required";
            if (string.IsNullOrEmpty(this.Email))
                //Validate if Email is null
                validation += " The email is required";
            if (string.IsNullOrEmpty(this.Address))
                //Validate if Address is null
                validation += " The address is required";
            if (string.IsNullOrEmpty(this.Phone))
                //Validate if Phone is null
                validation += " The phone is required";

            return string.IsNullOrEmpty(validation);

        }
    }
}
