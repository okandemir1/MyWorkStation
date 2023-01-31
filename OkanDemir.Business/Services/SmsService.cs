using System.Net;
using System.Text;

namespace OkanDemir.Business.Services
{
    public class SmsService
    {
        public string SendMessage(string phoneNumber, string message)
        {
            try
            {
                string smsResult = HTTPPoster(
                "<SingleTextSMS>" +
                "<UserName></UserName>" +
                "<PassWord></PassWord>" +
                "<Action>0</Action>" +
                "<Mesgbody>" + message + "</Mesgbody>" +
                "<Numbers>" + phoneNumber + "</Numbers>" +
                "<Originator>KeskeDeme</Originator>" +
                "<SDate></SDate>" +
                "<ExDate></ExDate>" +
                "</SingleTextSMS>"
                );

                return smsResult;
            }
            catch (Exception e)
            {
                return "hata";
            }
        }

        private string HTTPPoster(string prmSendData)
        {
            try
            {
                WebClient wUpload = new WebClient();
                wUpload.Proxy = null;
                byte[] bPostArray = Encoding.UTF8.GetBytes(prmSendData);
                byte[] bResponse = wUpload.UploadData("http://g3.iletimx.com", "POST", bPostArray);
                char[] sReturnChars = Encoding.UTF8.GetChars(bResponse);
                string sWebPage = new string(sReturnChars);
                return sWebPage;
            }
            catch
            {
                return "-1";
            }
        }
    }

}