using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Interfaces.Sms;

namespace SiaAdmin.Infrastructure.Services
{
    public class SmsService : ISmsService
    {
        static int _channelCode = 583;
        static string _username = "otpsiainsight";
        static string _password = "eYs5tAh9";
      
        
        public string Post2SmsOrigin(string Data2Post)
        {
            WebRequest request = WebRequest.Create("http://processor.smsorigin.com/xml/process.aspx");
            request.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(Data2Post);
            request.ContentType = "text/xml; charset=windows-1254";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string result = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();

            return result;
        }

        public double GetCredit()
        {
            throw new NotImplementedException();
        }

        public string GetSmsOriginStatusText(int StatusId)
        {
            throw new NotImplementedException();
        }

        public string ReturnSmsCodeErrorCode(string retval)
        {
            throw new NotImplementedException();
        }

        public double SendSmsOneToMany(  string Recipients,string randomNumber)
        {
            string originator = "SIAINSIGHT";
            string message = randomNumber + " şifresi ile Sia Live Panele girebilirsiniz. Şifre talebinde bulunmadıysanız, bu mesajı dikkate almayınız.";
            double retval = 0f;
            string request = string.Format(@"
<MainmsgBody>
    <Command>0</Command>
    <PlatformID>1</PlatformID>
    <ChannelCode>{0}</ChannelCode>
    <UserName>{1}</UserName>
    <PassWord>{2}</PassWord>
                <Mesgbody>{4}</Mesgbody>
                <Numbers>90{5}</Numbers>
    <Type>1</Type>
    <Concat>0</Concat>
    <Option>1</Option>
                <Originator>{3}</Originator>
</MainmsgBody>",
              _channelCode,
              _username,
              _password,
              originator,
              message,
              Recipients
              );
            string response = Post2SmsOrigin(request);
            if (response.StartsWith("ID:"))
            {
                double.TryParse(response.Substring(3), out retval);
            }
            return retval;
        }
    }
}
