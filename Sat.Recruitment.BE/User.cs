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
                        this.Money +=  this.Money * 0.12m;
                    }
                    if (this.Money < 100 && this.Money > 10)
                    {
                        this.Money += this.Money * 0.8m;
                    }
                    break;
                case "SuperUser":
                    if (this.Money > 100)
                    {
                        this.Money += this.Money * 0.20m;
                    }
                    break;
                case "Premium":
                    if (this.Money > 100)
                    {
                        this.Money += this.Money * 2m;
                    }
                    break;
            }

            //Normalizar EMAIL            
            string[] aux = this.Email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            int atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);
            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);
            this.Email = string.Join("@", new string[] { aux[0], aux[1] });

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
