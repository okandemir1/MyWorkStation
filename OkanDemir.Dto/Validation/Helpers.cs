using System.Net.Mail;

namespace OkanDemir.Dto.Validation
{
    public static class Helpers
    {
        public static bool CheckMobilePhone(string phone)
        {
            try
            {
                if (string.IsNullOrEmpty(phone))
                    return false;

                string numericPhone = new String(phone.Where(Char.IsDigit).ToArray());


                if (!string.IsNullOrEmpty(numericPhone) && numericPhone.Length == 11 && numericPhone.All(char.IsDigit))
                {
                    if (Int64.Parse(numericPhone) > 05000000000 && Int64.Parse(numericPhone) < 05999999999)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool CheckEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool IdNoValid(string idNo)
        {
            bool returnvalue = false;
            try
            {
                if (string.IsNullOrEmpty(idNo))
                    return returnvalue;

                if (idNo.Length == 11)
                {
                    Int64 ATCNO, BTCNO, TcNo;
                    long C1, C2, C3, C4, C5, C6, C7, C8, C9, Q1, Q2;

                    TcNo = Int64.Parse(idNo);

                    ATCNO = TcNo / 100;
                    BTCNO = TcNo / 100;

                    C1 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C2 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C3 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C4 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C5 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C6 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C7 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C8 = ATCNO % 10; ATCNO = ATCNO / 10;
                    C9 = ATCNO % 10; ATCNO = ATCNO / 10;
                    Q1 = ((10 - ((((C1 + C3 + C5 + C7 + C9) * 3) + (C2 + C4 + C6 + C8)) % 10)) % 10);
                    Q2 = ((10 - (((((C2 + C4 + C6 + C8) + Q1) * 3) + (C1 + C3 + C5 + C7 + C9)) % 10)) % 10);

                    returnvalue = ((BTCNO * 100) + (Q1 * 10) + Q2 == TcNo);
                }
            }
            catch (Exception ex)
            {
                return returnvalue;
            }
            
            return returnvalue;
        }
    }
}
